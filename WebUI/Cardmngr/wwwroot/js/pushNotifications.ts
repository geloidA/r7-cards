interface Window {
    blazorPushNotifications: {
        requestSubscription: () => Promise<any>;
    };
}

(function () {
    
    const applicationServerPublicKey = 'BEkue5xGF7tIMfiE_8a4XcImPvOMihiYLTE1F9kxtyVU98k4EJh38Fok6Vt-bwFmP2efctAPXtKvIRstZVOR8_g';

    window.blazorPushNotifications = {
        requestSubscription: async () => {
            const worker = await navigator.serviceWorker.getRegistration();
            if (worker == null) {
                throw new Error('Service worker not found');
            }
            const existingSubscription = await worker.pushManager.getSubscription();
            if (!existingSubscription) {
                const newSubscription = await subscribe(worker);
                if (newSubscription) {
                    return {
                        url: newSubscription.endpoint,
                        p256dh: arrayBufferToBase64(newSubscription.getKey('p256dh')),
                        auth: arrayBufferToBase64(newSubscription.getKey('auth'))
                    };
                }
            }
        }
    };

    async function subscribe(worker: ServiceWorkerRegistration) {
        try {
            return await worker.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: applicationServerPublicKey
            });
        } catch (error) {
            if (error.name === 'NotAllowedError') {
                return null;
            }
            throw error;
        }
    }

    function arrayBufferToBase64(buffer: ArrayBuffer | null) {
        // https://stackoverflow.com/a/9458996
        if (buffer == null) {
            throw new Error('buffer is null');
        }
        
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }
})();