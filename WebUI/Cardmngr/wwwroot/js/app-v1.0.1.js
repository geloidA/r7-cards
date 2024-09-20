function scrollToPosition(element, targetPosition, duration, dotNetObjectRef) {
    if (element.scrollTop === undefined || targetPosition === element.scrollTop) {
        dotNetObjectRef === null || dotNetObjectRef === void 0 ? void 0 : dotNetObjectRef.invokeMethodAsync('ScrollFinished');
        return;
    }
    const startPosition = element.scrollTop;
    const distance = targetPosition - startPosition;
    const startTime = performance.now();
    function animateScroll(currentTime) {
        const timeElapsed = currentTime - startTime;
        const scrollProgress = Math.min(timeElapsed / duration, 1);
        element.scrollTop = startPosition + distance * scrollProgress;
        if (targetPosition !== element.scrollTop) {
            requestAnimationFrame(animateScroll);
        }
        else {
            dotNetObjectRef === null || dotNetObjectRef === void 0 ? void 0 : dotNetObjectRef.invokeMethodAsync('ScrollFinished');
        }
    }
    requestAnimationFrame(animateScroll);
}
function scrollToBottom(element, duration, dotNetObjectRef) {
    scrollToPosition(element, element.scrollHeight - element.clientHeight, duration, dotNetObjectRef);
}
function reloadPage() {
    window.location.reload();
}
function measureHeight(element) {
    return element?.offsetHeight ?? 0;
}
