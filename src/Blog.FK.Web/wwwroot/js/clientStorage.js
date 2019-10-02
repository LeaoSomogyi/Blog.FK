define([], function () {

    var blogInstance = localforage.createInstance({ name: 'blog' });

    var actualListSize = null;
    var limit = 3;

    function addPosts(posts) {
        return new Promise((resolve, reject) => {
            var keyPair = [];

            posts.map((item) => {
                keyPair.push({ key: item.id, value: item });
            });

            blogInstance.setItems(keyPair)
                .then(() => {
                    resolve();
                });
        });
    }

    function getBlogPosts() {
        return new Promise((resolve, reject) => {
            blogInstance.keys().then((keys) => {
                keys = keys.filter((k) => { return k.length === 36; });

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

                blogInstance.getItems(_keys).then((results) => {

                    var posts = Object.keys(results).map((k) => {
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
        return new Promise((resolve, reject) => {
            blogInstance.getItem(id + url).then((text) => {
                resolve(text);
            });
        });
    }

    function addPostBlog(id, text, url) {
        return new Promise((resolve, reject) => {
            blogInstance.setItem(id + url, text).then(() => {
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