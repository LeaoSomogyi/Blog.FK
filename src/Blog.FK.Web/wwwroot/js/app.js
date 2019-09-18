var blogService = require('./blogService.js');

blogService.getLatestPosts();

window.pageEvents = {
    loadBlogPost: function (id, url) {
        blogService.loadBlogPost(id, url);
    },
    getMoreBlogPosts: function () {
        blogService.getMoreBlogPosts();
    }
};