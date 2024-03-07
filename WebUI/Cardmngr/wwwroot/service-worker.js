self.addEventListener('install', async event => {
    console.log('Installing service worker...');
    self.skipWaiting();
});