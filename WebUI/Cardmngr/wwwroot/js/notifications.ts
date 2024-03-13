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

export function create(title: string, options: MyNotificationOptions | undefined) {

    const notification = new Notification(title, options);
    notification.onclick = (e) => {
        e.preventDefault();
        if (options?.href) {
            window.open(options.href);
            window.focus();
        }
    }

    return notification;
}

interface MyNotificationOptions {
    badge?: string;
    body?: string;
    data?: any;
    dir?: NotificationDirection;
    icon?: string;
    lang?: string;
    requireInteraction?: boolean;
    silent?: boolean | null;
    tag?: string;
    href?: string;
}