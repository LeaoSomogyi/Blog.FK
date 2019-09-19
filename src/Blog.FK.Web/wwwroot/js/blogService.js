define(['./template.js', '../lib/showdown/showdown.js', './clientStorage.js'],
    function (template, showdown, clientStorage) {
        var blogPostUrl = '/Blog/GetLatestPosts/';
        var blogGetUrl = '/Blog/LoadBlogPost/?id=';
        var blogMorePostsUrl = '/Blog/GetMoreBlogPosts/?actualListSize=';

        var actualListSize = 0;

        function getLatestPosts() {
            loadData(blogPostUrl);
        }

        function loadBlogPost(id, url) {
            fetch(blogGetUrl + id)
                .then(function (response) {
                    return response.text();
                }).then(function (data) {
                    var converter = new showdown.Converter();
                    var html = converter.makeHtml(data);
                    template.showBlogItem(html, url);
                    window.location = '#' + url;
                });
        }

        function getMoreBlogPosts() {
            loadData(blogMorePostsUrl + actualListSize);
        }

        function loadData(url) {
            fetchPromise(url)
                .then(function (status) {
                    $('#connection-status').html(status);
                });
        }

        function fetchPromise(url) {
            return new Promise(function (resolve, reject) {
                fetch(url)
                    .then(function (response) {
                        return response.json();
                    }).then(function (data) {
                        clientStorage.addPosts(data);
                        template.appendBlogList(data);
                        actualListSize += data.length;
                        resolve('Detectamos uma conexão com a internet. Exibindo últimos posts.');
                    }).catch(function (e) {
                        clientStorage.getBlogPosts()
                            .then(function (posts) {
                                template.appendBlogList(posts);
                                actualListSize += posts.length;
                            });
                        resolve('Sem conexão com a internet, exibindo posts offline.');
                    });

                setTimeout(function () {
                    resolve('A conexão com a intenet está muito lenta, exibindo posts offline');
                }, 1000);
            });
        }

        return { getLatestPosts: getLatestPosts, loadBlogPost: loadBlogPost, getMoreBlogPosts: getMoreBlogPosts };
    });