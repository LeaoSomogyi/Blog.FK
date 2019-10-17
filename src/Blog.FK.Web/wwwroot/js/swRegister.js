define(['./notificationService.js'], function (notificationService) {
    let _serviceWorkerRegistration;

    function requestPermission() {
        return new Promise((resolve, reject) => {
            const permissionResult = Notification.requestPermission((result) => {
                resolve(result);
            });

            if (permissionResult)
                permissionResult.then(resolve, reject);
        });
    }

    function subscribe() {
        notificationService.retrievePublicKey().then((key) => {
            var subscribeOptions = {
                userVisibleOnly: true,
                applicationServerKey: key
            };

            return _serviceWorkerRegistration.pushManager.subscribe(subscribeOptions)
                .then((pushSubscription) => {
                    notificationService.storePushSubscription(pushSubscription)
                        .then((response) => {
                            if (response.status === 201) {
                                console.log('Subscrito nas notificações push');
                            } else if (respon.status === 204) {
                                console.log('Notificações push já registradas');
                            } else {
                                console.log('Falha ao submeter subscrição');
                            }
                        }).catch((error) => {
                            console.log('Falha ao comunicar: ' + error);
                        });
                });
        });
    }

    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/sw.js').then((registration) => {
            console.log('ServiceWorker registrado com sucesso no escopo: ', registration.scope);
            _serviceWorkerRegistration = registration;

            if ('PushManager' in window) {
                console.log('This browser supports push notification.');

                if (Notification.permission === 'granted') {
                    console.log('Permissão para push notification concedida');
                    return;
                }

                $('#notification-subscribe-section').show();
            }

        }, (err) => {
            console.log('Falha no registro do ServiceWorker: ', err);
        });
    }

    return {
        requestPushPermission: function () {
            requestPermission().then((permissionResult) => {
                if (permissionResult !== 'granted')
                    throw new Error('Permission not granted.');

                subscribe();
            });
        }
    };
});

