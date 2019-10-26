var blogService = require('./blogService.js');
var serviceWorker = require('./swRegister.js');
var geolocationService = require('./geolocationService.js');

let defferedPrompt;
window.addEventListener('beforeinstallprompt', (e) => {
    e.preventDefault();
    defferedPrompt = e;
    $('#install-blog-fk').modal();
});

window.addEventListener('appinstalled', (evt) => {
    console.log('Blog FK foi adicionado à sua home com sucesso. Aproveite!!');
});

if ('BackgroundFetchManager' in self) {
    console.log('Este navegador suporta downloads em segundo plano!');
}

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
            if (choiceResult.outcome === 'accepted') {
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
    },
    setBackgroundFetch: function (id, url) {
        navigator.serviceWorker.ready.then(async (swReg) => {
            const bgFetch = await swReg.backgroundFetch.fetch(id+url, ['/Blog/LoadBlogPost/?id=' + id], {
                title: url,
                icons: [{
                    sizes: '192x192',
                    src: 'images/icons/icon-192x192.png',
                    type: 'image/png'
                }],
                downloadTotal: 15000
            });

            bgFetch.addEventListener('progress', () => {
                if (!bgFetch.downloadTotal) return;

                const percent = Math.round(bgFetch.downloaded / bgFetch.downloadTotal * 100);
                console.log('Download progress: ' + percent + '%');
                console.log('Download status: ' + bgFetch.result);

                $('.modal-footer').hide();
                $('#download-status').show();
                $('#download-status > .progress > .progress-bar').css('width', percent + '%');

                if (bgFetch.result === 'success') {
                    $('#download-status > .text-success').show();
                }
            });
        });
    },
    requestPushPermission: function () {
        serviceWorker.requestPushPermission();
    },
    getGeolocation: function () {
        geolocationService.getGeolocation();
    }
};

blogService.getLatestPosts();