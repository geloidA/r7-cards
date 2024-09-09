export function saveAs(bytesBase64, filename) {
    const a = document.createElement('a');
    a.style.display = 'none';
    a.href = "data:application/octet-stream;base64," + bytesBase64;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}
