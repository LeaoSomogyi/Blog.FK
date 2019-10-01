define([], function () {

    var blogInstance = localforage.createInstance({ name: 'blog' });

    var actualListSize = null;
    var limit = 3;

    function addPosts(posts) {
        return new Promise(function (resolve, reject) {
            var keyPair = [];

            posts.map(function (item) {
                keyPair.push({ key: item.id, value: item });
            });

            blogInstance.setItems(keyPair)
                .then(function () {
                    resolve();
                });
        });
    }

    function getBlogPosts() {
        return new Promise(function (resolve, reject) {
            blogInstance.keys().then(function (keys) {
                keys = keys.filter(function (k) { return k.length === 36; });

                var index = keys.indexOf(actualListSize);

                if (index == -1) {
                    index = keys.length;
                }

                if (index == 0) {
                    resolve([]);
                    return;
                }

                var start = index - limit;
                var limitAdjusted = start < 0 ? index : limit;
                var _keys = keys.splice(Math.max(0, start), limitAdjusted);

                blogInstance.getItems(_keys).then(function (results) {

                    var posts = Object.keys(results).map(function (k) {
                        return results[k];
                    }).reverse();

                    actualListSize = String(posts[posts.length - 1].id);

                    resolve(posts);
                });
            });
        });
    }

    function getBlogListSize() {
        return actualListSize;
    }

    function loadBlogPost(id, url) {
        return new Promise(function (resolve, reject) {
            blogInstance.getItem(id + url).then(function (text) {
                resolve(text);
            });
        });
    }

    function addPostBlog(id, text, url) {
        return new Promise(function (resolve, reject) {
            blogInstance.setItem(id + url, text).then(function () {
                resolve();
            });
        });
    }

    return {
        addPosts: addPosts,
        getBlogPosts: getBlogPosts,
        getBlogListSize: getBlogListSize,
        loadBlogPost: loadBlogPost,
        addPostBlog: addPostBlog
    };
});