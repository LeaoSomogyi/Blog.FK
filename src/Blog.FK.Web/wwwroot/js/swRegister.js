if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/sw.js').then(function (registration) {
        console.log('ServiceWorker registrado com sucesso no escopo: ', registration.scope);
    }, function (err) {
        console.log('Falha no registro do ServiceWorker: ', err);
    });
}