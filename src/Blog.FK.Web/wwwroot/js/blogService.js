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
                .then(function (response) {
                    return response.text();
                })
                .then(function (text) {
                    var html = converter.makeHtml(data);
                    clientStorage.addPostBlog(id, data, url).then(function () {
                        template.showBlogItem(html, url);
                        window.location = '#' + url;
                    });
                })
                .catch(function () {
                    $('#connection-status').html(offlineText);
                    clientStorage.loadBlogPost(id, url).then(function (cacheData) {
                        if (!cacheData) {
                            var modalContent = $('#post-not-found').html().replace(/{{Id}}/g, id).replace(/{{Link}}/g, url);

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
                .then(function (status) {
                    $('#connection-status').html(status);

                    clientStorage.getBlogPosts()
                        .then(function (posts) {
                            template.appendBlogList(posts);
                            actualListSize += posts.length;
                        });
                });
        }

        function fetchPromise(id, apiUrl, url, text) {
            return new Promise(function (resolve, reject) {
                var endpoint = id === null ? apiUrl : apiUrl + id;

                fetch(endpoint)
                    .then(function (data) {

                        var resolveSuccess = function () {
                            resolve(onlineText);
                        };

                        if (text) {
                            data.text().then(function (text) {
                                clientStorage.addPostBlog(id, text, url).then(resolveSuccess);
                            });
                        }
                        else {
                            data.json().then(function (jsonData) {
                                clientStorage.addPosts(jsonData).then(resolveSuccess);
                            });
                        }

                    }).catch(function () {
                        resolve(offlineText);
                    });

                setTimeout(function () {
                    resolve(slowInternetText);
                }, 5000);
            });
        }

        return { getLatestPosts: getLatestPosts, loadBlogPost: loadBlogPost, getMoreBlogPosts: getMoreBlogPosts };
    });