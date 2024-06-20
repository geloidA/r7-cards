function scrollToPosition(element: HTMLElement, targetPosition: number, duration: number): void {
    const startPosition = element.scrollTop;
    const distance = targetPosition - startPosition;
    const startTime = performance.now();

    function animateScroll(currentTime: number): void {
        const timeElapsed = currentTime - startTime;
        const scrollProgress = Math.min(timeElapsed / duration, 1);

        element.scrollTop = startPosition + distance * scrollProgress;

        if (targetPosition !== element.scrollTop) {
            requestAnimationFrame(animateScroll);
        }
    }

    requestAnimationFrame(animateScroll);
}

function scrollToBottom(element: HTMLElement, duration: number): void {
    scrollToPosition(element, element.scrollHeight - element.clientHeight, duration);
}

function reloadPage() {
    window.location.reload();
}