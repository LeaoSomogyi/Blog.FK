define(['./template.js', '../lib/showdown/showdown.js'], function (template, showdown) {
    var blogPostUrl = '/Blog/GetLatestPosts/';
    var blogGetUrl = '/Blog/LoadBlogPost/?id=';

    function getLatestPosts() {
        fetch(blogPostUrl)
            .then(function (response) {
                return response.json();
            }).then(function (data) {
                console.log(data);
                template.appendBlogList(data);
            });
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

    return { getLatestPosts: getLatestPosts, loadBlogPost: loadBlogPost };
});