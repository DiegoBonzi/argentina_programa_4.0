const anchoVentana = window.innerWidth;
const altoVentana = window.innerHeight;
const bannerArray = document.querySelectorAll('.banner');
/* function extremosElementoPantalla(elemento){
    const {top, left, bottom, right} = elemento.getBoundingClientRect();
    const {scrollX, scrollY} = window;
    return {
        top: top - scrollY,
        left: left - scrollX,
        bottom: bottom - scrollY,
        right: right - scrollY,
    }
} */
function actualizarEstilos(elemento){
    const {top, left, bottom, right} = elemento.getBoundingClientRect();
    const {innerWidth, innerHeight} = window;
    const inViewport = !(
           (top < 0 && bottom < 0)
        || (top > innerHeight && bottom > innerHeight)
        || (left < 0 && right < 0)
        || (left > innerWidth && right > innerWidth));
    if(inViewport){
        console.log(inViewport);
    } else return;
}
function mouseMoveEventHandler(e){
    const posX = e.clientX / anchoVentana;
    const posY = e.clientY / altoVentana;
}
document.addEventListener('mousemove', mouseMoveEventHandler);