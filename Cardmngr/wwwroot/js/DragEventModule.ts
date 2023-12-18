class DragEventModule {
    private htmlElement: Element;
    private callbackObj: DotNetObjectRef;

    constructor(element: Element, callbackObj: DotNetObjectRef) {
        this.htmlElement = element;
        this.callbackObj = callbackObj;
    }

    public onDragOver(methodName: string, delay: number, stopPropagation: boolean = false, preventDefault: boolean = false) {
        this.registerHandlerDelayed("dragover", methodName, delay, stopPropagation, preventDefault);
    }

    public onDragEnter(methodName: string, stopPropagation: boolean = false, preventDefault: boolean = false) {
        this.registerHandler("dragenter", methodName, stopPropagation, preventDefault);
    }

    public onDragLeave(methodName: string, stopPropagation: boolean = false, preventDefault: boolean = false) {
        this.registerHandler("dragleave", methodName, stopPropagation, preventDefault);
    }

    public dispose() {
        this.callbackObj.dispose();
    }

    private registerHandlerDelayed(eventName: string, methodName: string, delay: number,
        stopPropagation: boolean = false, 
        preventDefault: boolean = false
        ) {
        
        this.htmlElement.addEventListener(eventName, (event: Event) => 
            this.throttle(() => {
                if (stopPropagation) {
                    event.stopPropagation();
                }
                if (preventDefault) {
                    event.preventDefault();
                }
                this.callbackObj.invokeMethodAsync(methodName);
            }, delay), );
    }

    private registerHandler(eventName: string, methodName: string, 
        stopPropagation: boolean = false,
        preventDefault: boolean = false
        ) {
        
        this.htmlElement.addEventListener(eventName, (event: Event) => {
            if (stopPropagation) {
                event.stopPropagation();
            }
            if (preventDefault) {
                event.preventDefault();
            }
            this.callbackObj.invokeMethodAsync(methodName);
        });
    }

    private throttle(fun: Function, delay: number) {
        let flag = true;
        return () => {
            if (flag) {
                fun();
                flag = false;
                setTimeout(() => flag = true, delay);
            }
        }
    }
}

export const initializeModule = (element: Element, callbackObj: DotNetObjectRef) => {
    return new DragEventModule(element, callbackObj);
}
