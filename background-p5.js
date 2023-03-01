let noiseScale = 0.006;
let noiseStrength = 1000;
let noiseX = 0;

function setup() {
  var canvas = createCanvas(windowWidth, windowHeight);
  canvas.parent('fondo');
}

function draw() {
  background(13, 20, 27);
  noiseX += -0.001 * 5;

  //strokeWeight(3) 
  for (let y = 0; y < height; y += 2) {
    let noiseY = y * noiseScale;
    let noiseValue = noise(noiseX, noiseY) * noiseStrength * 2;
    stroke(255, 70, 124);
    line(0, -y + 300, width, -y + noiseValue) / 100;
  }  
}