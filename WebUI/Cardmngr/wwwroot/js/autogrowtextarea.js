export function autoGrowHeight(textareaId) {
    const fluentTextArea = document.getElementById(textareaId);
    if (fluentTextArea) {
        const shadowRoot = fluentTextArea.shadowRoot;
        if (shadowRoot) {
            const textarea = shadowRoot.querySelector('textarea');
            if (textarea.style.overflow !== 'hidden') {
                textarea.style.overflow = 'hidden';
            }
            textarea.style.height = 'auto';
            textarea.style.height = textarea.scrollHeight + 'px';
        }
    }
}
