/**
 * Add an event listener to the button on window load
 */
window.addEventListener('load', () => {
    const factuurButton = document.getElementById('print-factuur-button');
    factuurButton.addEventListener('click', printFactuur)
});

/**
 * Activate the browser's 'print' functionality
 */
const printFactuur = () => {
    window.print();
};
