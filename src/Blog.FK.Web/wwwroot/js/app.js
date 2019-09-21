var blogService = require('./blogService.js');
var serviceWorker = require('./swRegister.js');

let defferedPrompt;
window.addEventListener('beforeinstallprompt', (e) => {
    e.preventDefault();
    defferedPrompt = e;
    $('#install-blog-fk').modal();
});

window.addEventListener('appinstalled', (evt) => {
    console.log('Blog FK foi adicionado à sua home com sucesso. Aproveite!!');
});

window.pageEvents = {
    loadBlogPost: function (id, url) {
        $('#blog-list').hide();
        $('#show-more').hide();
        blogService.loadBlogPost(id, url);
    },
    getMoreBlogPosts: function () {
        blogService.getMoreBlogPosts();
    },
    tryAddHomeScreen: function () {
        defferedPrompt.prompt();
        defferedPrompt.userChoice.then((choiceResult) => {
            if (choiceResult.outcome == 'accepted') {
                console.log('App foi aceito para instalação o/');
            }
            defferedPrompt = null;
        });
    },
    navigateBack: function () {
        window.location = '#';
        $('#show-more').show();
        $('#blog-item-container').hide();
        $('#blog-list').show();
    }
};

blogService.getLatestPosts();