"use strict";

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
    '/js/site.js',
    '/js/app.js',
    '/manifest.json',
    '/favicon.ico',
    '/js/blogService.js',
    '/js/swRegister.js',
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

self.addEventListener('install', function (event) {
    console.log("SW: Evento de Instalacao");
    self.skipWaiting();
    event.waitUntil(
        caches.open(cacheName)
            .then(function (cache) {
                return cache.addAll(blogCacheFiles);
            })
    );
});

self.addEventListener('activate', function (event) {
    console.log('SW: Ativando...');
    self.clients.claim();
    event.waitUntil(
        caches.keys()
            .then(function (cacheKeys) {
                var deletePromises = [];

                for (var i = 0; i < cacheKeys.length; i++) {
                    if (cacheKeys[i] != cacheName) {
                        deletePromises.push(caches.delete(cacheKeys[i]));
                    }
                }

                return Promise.all(deletePromises);
            }));
});

self.addEventListener('fetch', function (event) {
    console.log('SW: Fetch ' + event.request.url);

    if (event.request.url.toLowerCase().includes("/blog")) {
        console.log('[ServiceWorker] online - get online ' + event.request.url);
        event.respondWith(fetch(event.request));
    } else {
        event.respondWith(
            timeout(1000, fetch(event.request)).catch(function () {
                console.log('[ServiceWorker] offline - get from cache: ' + event.request.url);
                return caches.match(event.request);
            })
        );
    }
});