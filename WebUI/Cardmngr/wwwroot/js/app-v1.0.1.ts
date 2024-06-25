function scrollToPosition(element: HTMLElement, targetPosition: number, duration: number, dotNetObjectRef?: DotNetObjectRef): void {
    if (element.scrollTop === undefined || targetPosition === element.scrollTop) {
        console.log('Scroll finished');
        dotNetObjectRef?.invokeMethodAsync('ScrollFinished');
        return;
    }
    
    const startPosition = element.scrollTop;
    const distance = targetPosition - startPosition;
    const startTime = performance.now();

    function animateScroll(currentTime: number): void {
        const timeElapsed = currentTime - startTime;
        const scrollProgress = Math.min(timeElapsed / duration, 1);

        element.scrollTop = startPosition + distance * scrollProgress;

        if (targetPosition !== element.scrollTop) {
            requestAnimationFrame(animateScroll);
        } else {
            console.log('Scroll finished');
            dotNetObjectRef?.invokeMethodAsync('ScrollFinished');
        }
    }

    requestAnimationFrame(animateScroll);
}

function scrollToBottom(element: HTMLElement, duration: number, dotNetObjectRef: DotNetObjectRef): void {
    scrollToPosition(element, element.scrollHeight - element.clientHeight, duration, dotNetObjectRef);
}

function reloadPage() {
    window.location.reload();
}