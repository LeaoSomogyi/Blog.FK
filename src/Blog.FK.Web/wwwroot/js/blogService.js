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

            fetchPromise(id, blogGetUrl, url, true)
                .then((response) => {
                    return response.text();
                })
                .then((text) => {
                    var html = converter.makeHtml(data);
                    clientStorage.addPostBlog(id, data, url).then(() => {
                        template.showBlogItem(html, url);
                        window.location = '#' + url;
                    });
                })
                .catch(() => {
                    $('#connection-status').html(offlineText);
                    clientStorage.loadBlogPost(id, url).then((cacheData) => {
                        if (!cacheData) {
                            var modalContent = $('#post-not-found').html().replace(/{{Id}}/g, id).replace(/{{Link}}/g, url);

                            $('.modal-footer').show();
                            $('#download-status').hide();

                            $('#post-not-found').html(modalContent).modal();
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
            fetchPromise(null, url, false)
                .then((status) => {
                    clientStorage.getBlogPosts()
                        .then((posts) => {
                            template.appendBlogList(posts);
                            actualListSize += posts.length;
                        });
                });
        }

        function fetchPromise(id, apiUrl, url, text) {
            return new Promise((resolve, reject) => {
                var endpoint = id === null ? apiUrl : apiUrl + id;

                fetch(endpoint)
                    .then((data) => {

                        var resolveSuccess = function () {
                            resolve(onlineText);
                        };

                        if (text) {
                            data.text().then((text) => {
                                clientStorage.addPostBlog(id, text, url).then(resolveSuccess);
                            });
                        }
                        else {
                            data.json().then((jsonData) => {
                                clientStorage.addPosts(jsonData).then(resolveSuccess);
                            });
                        }

                    }).catch(() => {
                        resolve(offlineText);
                    });

                setTimeout(() => {
                    resolve(slowInternetText);
                }, 5000);
            });
        }

        return { getLatestPosts: getLatestPosts, loadBlogPost: loadBlogPost, getMoreBlogPosts: getMoreBlogPosts };
    });