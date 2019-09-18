define(['./template.js', '../lib/showdown/showdown.js'], function (template, showdown) {
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
        fetch(url)
            .then(function (response) {
                return response.json();
            }).then(function (data) {
                console.log(data);
                template.appendBlogList(data);
                actualListSize += data.length;
            });
    }

    return { getLatestPosts: getLatestPosts, loadBlogPost: loadBlogPost, getMoreBlogPosts: getMoreBlogPosts };
});