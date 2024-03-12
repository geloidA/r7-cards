export function requestPermission() {
    return new Promise((resolve, reject) => {
        Notification.requestPermission((permission) => {
            resolve(permission);
        });
    });
}
export function isSupported() {
    if (window.Notification)
        return true;
    return false;
}
export function create(title, options) {
    const notification = new Notification(title, options);
    notification.onclick = (e) => {
        e.preventDefault();
        if (options === null || options === void 0 ? void 0 : options.href) {
            window.location.href = options.href;
        }
    };
    return notification;
}
//# sourceMappingURL=notifications.js.map