var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
(function () {
    const applicationServerPublicKey = 'BEkue5xGF7tIMfiE_8a4XcImPvOMihiYLTE1F9kxtyVU98k4EJh38Fok6Vt-bwFmP2efctAPXtKvIRstZVOR8_g';
    window.blazorPushNotifications = {
        requestSubscription: () => __awaiter(this, void 0, void 0, function* () {
            const worker = yield navigator.serviceWorker.getRegistration();
            if (worker == null) {
                throw new Error('Service worker not found');
            }
            const existingSubscription = yield worker.pushManager.getSubscription();
            if (!existingSubscription) {
                const newSubscription = yield subscribe(worker);
                if (newSubscription) {
                    return {
                        url: newSubscription.endpoint,
                        p256dh: arrayBufferToBase64(newSubscription.getKey('p256dh')),
                        auth: arrayBufferToBase64(newSubscription.getKey('auth'))
                    };
                }
            }
        })
    };
    function subscribe(worker) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                return yield worker.pushManager.subscribe({
                    userVisibleOnly: true,
                    applicationServerKey: applicationServerPublicKey
                });
            }
            catch (error) {
                if (error.name === 'NotAllowedError') {
                    return null;
                }
                throw error;
            }
        });
    }
    function arrayBufferToBase64(buffer) {
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
//# sourceMappingURL=pushNotifications.js.map