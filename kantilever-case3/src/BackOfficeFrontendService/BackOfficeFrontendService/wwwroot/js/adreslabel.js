/**
 * Add an event listener to the button on window load
 */
window.addEventListener('load', () => {
    const factuurButton = document.getElementById('print-adreslabel-button');
    factuurButton.addEventListener('click', printAdreslabel);
});

/**
 * Activate the browser's 'print' functionality
 */
const printAdreslabel = () => {
    window.print();
};