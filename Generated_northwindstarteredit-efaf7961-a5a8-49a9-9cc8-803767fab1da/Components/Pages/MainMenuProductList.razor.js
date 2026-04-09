export function downloadFile(filename, base64) {
    const link = document.createElement("a");
    link.href = `data:text/csv;base64,${base64}`;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
