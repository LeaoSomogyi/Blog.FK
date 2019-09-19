define([], function () {

    var blogInstance = localforage.createInstance({ name: 'blog' });

    var actualListSize = null;
    var limit = 3;

    function addPosts(posts) {
        return new Promise(function (resolve, reject) {
            var keyPair = [];

            posts.map(function (item) {
                keyPair.push({ key: item.id, value: item });

                actualListSize += 1;
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

                    if (Object.keys(results).length === actualListSize) {
                        resolve([]);
                    }
                    else {
                        var posts = Object.keys(results).map(function (k) {
                            return results[k];
                        }).reverse();

                        actualListSize = + posts.length;

                        resolve(posts);
                    }
                });
            });
        });
    }

    function getBlogListSize() {
        return actualListSize;
    }

    return { addPosts: addPosts, getBlogPosts: getBlogPosts, getBlogListSize: getBlogListSize };
});