"use strict";
importScripts('lib/localforage/localforage.min.js');

var cacheName = 'v1Cache';

var blogCacheFiles = [
    '/',
    '/sw.js',
    '/lib/bootstrap/dist/css/bootstrap.css',
    '/lib/fontawesome/css/all.css',
    '/css/site.css',
    '/lib/jquery/dist/jquery.js',
    '/lib/bootstrap/dist/js/bootstrap.min.js',
    '/lib/es6-promise/es6-promise.js',
    '/lib/fetch/fetch.js',
    '/lib/systemjs/system.js',
    '/lib/localforage/localforage.min.js',
    '/lib/localforage/localforage-getitems.js',
    '/lib/localforage/localforage-setitems.js',
    '/lib/gauge/gauge.js',
    '/js/site.js',
    '/js/app.js',
    '/manifest.json',
    '/favicon.ico',
    '/js/blogService.js',
    '/js/swRegister.js',
    '/js/geolocationService.js',
    '/js/punchMeter.js',
    '/js/template.js',
    '/lib/showdown/showdown.js',
    '/js/clientStorage.js',
    '/images/icons/icon-72x72.png',
    '/images/icons/icon-96x96.png',
    '/images/icons/icon-128x128.png',
    '/images/icons/icon-144x144.png',
    '/images/icons/icon-152x152.png',
    '/images/icons/icon-192x192.png',
    '/images/icons/icon-384x384.png',
    '/images/icons/icon-512x512.png'
];

function timeout(ms, promise) {
    return new Promise(function (resolve, reject) {
        setTimeout(function () {
            reject();
        }, ms);
        promise.then(resolve, reject);
    });
}

self.addEventListener('install', (event) => {
    console.log("[Service Worker]: Installing event");
    event.waitUntil(
        caches.open(cacheName).then((cache) => {
            return cache.addAll(blogCacheFiles);
        })
            .then(() => self.skipWaiting())
    );
});


self.addEventListener('activate', (event) => {
    console.log('[Service Worker]: Activating...');
    self.clients.claim()
    event.waitUntil(() => {
        caches.keys()
            .then((cacheKeys) => {
                var deletePromises = [];

                for (var i = 0; i < cacheKeys.length; i++) {
                    if (cacheKeys[i] != cacheName) {
                        deletePromises.push(caches.delete(cacheKeys[i]));
                    }
                }

                return Promise.all(deletePromises);
            });
    });
});

self.addEventListener('fetch', (event) => {
    console.log('[Service Worker]: Fetch ' + event.request.url);

    event.respondWith(
        caches.open(cacheName)
            .then(cache => cache.match(event.request, { ignoreSearch: true }))
            .then(response => {
                return response || fetch(event.request);
            })
    );
});

self.addEventListener('backgroundfetchsuccess', (event) => {
    const bgFetch = event.registration;

    event.waitUntil(async function () {
        var blogInstance = localforage.createInstance({ name: 'blog' });

        const records = await bgFetch.matchAll();

        const promises = records.map(async (record) => {
            const response = await record.responseReady;

            response.text().then(function (text) {
                console.log('Text retrieved - storing in IndexedDB');
                blogInstance.setItem(bgFetch.id, text);
            });
        });

        await Promise.all(promises);
        event.updateUI({ title: 'Download Concluído' });
    });
});

self.addEventListener('push', (event) => {
    event.waitUntil(self.registration.showNotification('Blog FK!', {
        body: event.data.text(),
        icon: '/images/icons/icon-72x72.png'
    }));
});