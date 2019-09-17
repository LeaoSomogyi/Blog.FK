define([], function () {
    function generateBlogItem(item) {
        var template = $('#blog-card').html();

        template = template.replace('{{PostId}}', item.id.substring(24, 36));
        template = template.replace('{{Id}}', item.id);
        template = template.replace('{{Title}}', item.title);
        template = template.replace('{{ShortDescription}}', item.short_description);
        template = template.replace('{{Link}}', item.link);

        return template;
    }

    function appendBlogList(items) {
        var cardHtml = '';

        for (var i = 0; i < items.length; i++) {
            cardHtml += generateBlogItem(items[i]);
        }

        $('.blog-list').append(cardHtml);
    }

    function showBlogItem(html, url) {
        var template = $('#blog-item').html();
        template = template.replace('{{Link}}', url);
        template = template.replace('{{Content}}', html);

        $('#blog-item-container').html(template);
    }

    return { generateBlogItem: generateBlogItem, appendBlogList: appendBlogList, showBlogItem: showBlogItem };
});