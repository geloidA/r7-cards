export function registerHandlers(contentId, buttonId) {
    const content = document.getElementById(contentId);
    const button = document.getElementById(buttonId);
    if (content && button) {
        content.onscroll = function () {
            scrollFunction();
        };
    }
    function scrollFunction() {
        if (!content || !button)
            return;
        if (content.scrollTop > 20) {
            button.style.display = "flex";
        }
        else {
            button.style.display = "none";
        }
    }
}
