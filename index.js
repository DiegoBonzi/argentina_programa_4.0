const anchoVentana = window.innerWidth;
const altoVentana = window.innerHeight;

// determina si un elemento se esta mostrando en la pantalla
function elementoEnPantalla(elemento){
    const {top, left, bottom, right} = elemento.getBoundingClientRect();
    const {innerWidth, innerHeight} = window;
    return !(
           (top < 0 && bottom < 0)
        || (top > innerHeight && bottom > innerHeight)
        || (left < 0 && right < 0)
        || (left > innerWidth && right > innerWidth)
    );
}
// manejador de eventos de movimiento de mouse, calcula un factor de corrimiento
// de acuerdo a la posicion
function mouseMoveEventHandler(e){
    const factorX = (e.clientX - anchoVentana / 2) / anchoVentana;
    const factorY = (e.clientY - altoVentana / 2) / altoVentana;
    actualizarElementos(factorX, factorY)
}
document.addEventListener('mousemove', mouseMoveEventHandler);

// array para guardar posiciones iniciales
const initials = [];
// selectores de elementos
const bannerArray = document.querySelectorAll('.banner');
const bannerTitleArray = document.querySelectorAll('.banner h2');
const heroElement = document.querySelector('.hero');
function actualizarElementos(factorX, factorY){
    // corriemientos en x e y de 3 tipos para distintos elementos
    const calculatedOffset1X = 10 * factorX;
    const calculatedOffset1Y = 10 * factorY;
    const calculatedOffset2X = 15 * factorX;
    const calculatedOffset2Y = 15 * factorY;
    const calculatedOffset3X = 5 * factorX;
    const calculatedOffset3Y = 5 * factorY;

    for(let i = 0; i < bannerArray.length; i++){
        // para todos los elementos tipo .banner, actualizar posicion solo si estan en pantalla
        if(elementoEnPantalla(bannerArray[i])){
            // dividir los 3 valores de posicion en X e Y de los 3 backgrounds (2 graficos y un gradiente)
            const valuesX = window.getComputedStyle(bannerArray[i]).backgroundPositionX.split(',');
            const valuesY = window.getComputedStyle(bannerArray[i]).backgroundPositionY.split(',');
            // si no fueron guardados los valores iniciles, hacerlo
            if(!initials[`banner-${i}`]){
                initials[`banner-${i}`] = {
                    x1: valuesX[0], 
                    y1: valuesY[0],
                    x2: valuesX[1], 
                    y2: valuesY[1],
                };
            }
            // formar strings con funcion calc() para sumar el corrimiento de las posiciones
            const backgroundPositionXitem1 = `calc(${initials[`banner-${i}`].x1} + ${calculatedOffset2X.toFixed(2)}px)`;
            const backgroundPositionXitem2 = `calc(${initials[`banner-${i}`].x2} + ${calculatedOffset2X.toFixed(2)}px)`;
            const backgroundPositionYitem1 = `calc(${initials[`banner-${i}`].y1} + ${calculatedOffset2Y.toFixed(2)}px)`;
            const backgroundPositionYitem2 = `calc(${initials[`banner-${i}`].y2} + ${calculatedOffset2Y.toFixed(2)}px)`;
            // aplicar nuevas posiciones para los 3 elementos background de .banner
            bannerArray[i].style.backgroundPositionX = `${backgroundPositionXitem1}, ${backgroundPositionXitem2}, ${valuesX[2]}`;
            bannerArray[i].style.backgroundPositionY = `${backgroundPositionYitem1}, ${backgroundPositionYitem2}, ${valuesY[2]}`;
            // aplicar nueva posicion a titulos de banners utilizando translate
            bannerTitleArray[i].style.transform = `translate(${calculatedOffset3X}px, ${calculatedOffset3Y}px)`
        }
    }
    // guardar valores iniciales de posicion de background
    if(!initials['perfil']){
        initials['perfil'] = {
            x: window.getComputedStyle(heroElement).backgroundPositionX,
            y: window.getComputedStyle(heroElement).backgroundPositionY
        }
    }
    // aplicar corrimiento a background de .hero
    heroElement.style.backgroundPositionX = `calc(${initials[`perfil`].x} + ${calculatedOffset1X.toFixed(2)}px)`;
    heroElement.style.backgroundPositionY = `calc(${initials[`perfil`].y} + ${calculatedOffset1Y.toFixed(2)}px)`;
}