function scrollToPosition(element: HTMLElement, pos: number, duration: number): void {
    let currentPos = element.scrollTop;
    let start: number;

    pos = +pos; duration = +duration;

    requestAnimationFrame(function step(timestamp) {
        start = !start ? timestamp : start;
        let progress = timestamp - start;
        if (currentPos < pos) {
            element.scrollTo(0, ((pos - currentPos) * progress / duration) + currentPos);
        } else {
            element.scrollTo(0, currentPos - ((currentPos - pos) * progress / duration));
        }
        if (progress < duration) {
            requestAnimationFrame(step);
        } else {
            scrollTo(0, pos);
        }
    })
}

function reloadPage() {
    window.location.reload();
}