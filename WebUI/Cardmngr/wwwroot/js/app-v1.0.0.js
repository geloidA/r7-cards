function reload() {
    window.location.reload();
}

function makeWheelScrollable(scrollContainer) {

    scrollContainer.addEventListener('wheel', (e) => {
        e.preventDefault();
        scrollContainer.scrollLeft += e.deltaY;    
    });
}
