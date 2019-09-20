define(['./template.js', '../lib/showdown/showdown.js', './clientStorage.js'],
    function (template, showdown, clientStorage) {
        var blogPostUrl = '/Blog/GetLatestPosts/';
        var blogGetUrl = '/Blog/LoadBlogPost/?id=';
        var blogMorePostsUrl = '/Blog/GetMoreBlogPosts/?actualListSize=';
        var offlineText = 'Sem conexão com a internet, exibindo posts offline.';
        var onlineText = 'Detectamos uma conexão com a internet. Exibindo últimos posts.';
        var slowInternetText = 'A conexão com a intenet está muito lenta, exibindo posts offline';

        var actualListSize = 0;

        function getLatestPosts() {
            loadData(blogPostUrl);
        }

        function loadBlogPost(id, url) {
            var converter = new showdown.Converter();

            fetch(blogGetUrl + id)
                .then(function (response) {
                    return response.text();
                }).then(function (data) {
                    var html = converter.makeHtml(data);
                    clientStorage.addPostBlog(id, data, url).then(function () {
                        template.showBlogItem(html, url);
                        window.location = '#' + url;
                    });
                }).catch(function () {
                    $('#connection-status').html(offlineText);
                    clientStorage.loadBlogPost(id, url).then(function (cacheData) {
                        if (!cacheData) {
                            $('#post-not-found').modal();
                        } else {
                            var html = converter.makeHtml(cacheData);
                            template.showBlogItem(html, url);
                            window.location = '#' + url;
                        }
                    });
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
                        resolve(onlineText);
                    }).catch(function () {
                        clientStorage.getBlogPosts()
                            .then(function (posts) {
                                template.appendBlogList(posts);
                                actualListSize += posts.length;
                            });
                        resolve(offlineText);
                    });

                setTimeout(function () {
                    resolve(slowInternetText);
                }, 1000);
            });
        }

        return { getLatestPosts: getLatestPosts, loadBlogPost: loadBlogPost, getMoreBlogPosts: getMoreBlogPosts };
    });