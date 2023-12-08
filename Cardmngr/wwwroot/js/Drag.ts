/* draggable element */
const items: NodeListOf<HTMLElement> = document.querySelectorAll('.item');

items.forEach(item => item.addEventListener('dragstart', dragStart));

function dragStart(e: DragEvent) {
    e.dataTransfer?.setData('text/plain', (e.target as HTMLElement).id);
    setTimeout(() => {
        (e.target as HTMLElement).classList.add('hide');
    }, 0);
}

/* drop targets */
const boxes: NodeListOf<HTMLElement> = document.querySelectorAll('.box');

boxes.forEach(box => {
    box.addEventListener('dragenter', dragEnter)
    box.addEventListener('dragover', dragOver);
    box.addEventListener('dragleave', dragLeave);
    box.addEventListener('drop', drop);
});

function dragEnter(e: DragEvent) {
    e.preventDefault();
    (e.target as HTMLElement).classList.add('drag-over');
}

function dragOver(e: DragEvent) {
    e.preventDefault();
    (e.target as HTMLElement).classList.add('drag-over');
}

function dragLeave(e: DragEvent) {
    (e.target as HTMLElement).classList.remove('drag-over');
}

function drop(e: DragEvent) {
    (e.target as HTMLElement).classList.remove('drag-over');

    // get the draggable element
    const id: string = e.dataTransfer!.getData('text/plain');
    const draggable: HTMLElement = document.getElementById(id)!;

    // add it to the drop target
    (e.target as HTMLElement).appendChild(draggable);

    // display the draggable element
    draggable.classList.remove('hide');
}
