export function autoGrowHeight(textareaId: string) {
    
    const fluentTextArea = document.getElementById(textareaId) as HTMLTextAreaElement;

    if (fluentTextArea) {
        const shadowRoot = fluentTextArea.shadowRoot;

        if (shadowRoot) {
            const textarea = shadowRoot.querySelector('textarea') as HTMLTextAreaElement;
            if (textarea.style.overflow !== 'hidden') {
                textarea.style.overflow = 'hidden';
            }
            textarea.style.height = 'auto'
            textarea.style.height = textarea.scrollHeight + 'px';
        }
    }
}
