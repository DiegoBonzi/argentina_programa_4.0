/*{
  "ISFVSN": "2",
  "DESCRIPTION": "Super Mario procedural con efectos dimensionales. El tamaño de Mario es ahora ilimitado, y el universo puede ser desgarrado por un Cisma, consumido por una Falla o aniquilado por la Convergencia.",
  "CREDIT": "HLorenzi (GLSL), Gemini (ISF, Arquitectura de Efectos), Claude (Mejoras Visuales), User (Concepto y Dirección)",
  "CATEGORIES": [
    "Generator", "Visual", "Glitch", "Experimental"
  ],
  "PERSISTENT_BUFFERS": [
    "feedbackBuffer" 
  ],
  "INPUTS": [
    { "NAME": "Velocidad", "TYPE": "float", "LABEL": "Velocidad", "DEFAULT": 60.0, "MIN": 0.0, "MAX": 240.0 },
    { "NAME": "Gigante", "TYPE": "float", "LABEL": "Mario Gigante/Deformado", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 1.0 },
    { "NAME": "AcidoGlobal", "TYPE": "float", "LABEL": "Ácido Global", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 1.0 },
    { "NAME": "CorrupcionMario", "TYPE": "float", "LABEL": "Corrupción (Solo Mario)", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 1.0 },
    { "NAME": "CismaFeedback", "TYPE": "float", "LABEL": "Feedback (Cisma)", "DEFAULT": 0.0, "MIN": -5.0, "MAX": 5.0 },
    { "NAME": "FuerzaFeedback", "TYPE": "float", "LABEL": "Feedback (Fuerza)", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 0.98 },
    { "NAME": "Falla", "TYPE": "float", "LABEL": "Falla (Aniquilación Global)", "DEFAULT": 0.0, "MIN": -5.0, "MAX": 5.0 },
    { "NAME": "Convergencia", "TYPE": "event", "LABEL": "CONVERGENCIA (Evento Final)"}
  ]
}*/

// Declare the feedback buffer
varying vec2 isf_FragNormCoord;
uniform sampler2D feedbackBuffer;

#define PI 3.14159265359
#define RGB(r,g,b) vec4(float(r)/255.0,float(g)/255.0,float(b)/255.0,1.0)
#define TRANSPARENT vec4(0.0)
#define SPRROW(x,a,b,c,d,e,f,g,h, i,j,k,l,m,n,o,p) (x <= 7 ? SPRROW_H(a,b,c,d,e,f,g,h) : SPRROW_H(i,j,k,l,m,n,o,p))
#define SPRROW_H(a,b,c,d,e,f,g,h) (a+4.0*(b+4.0*(c+4.0*(d+4.0*(e+4.0*(f+4.0*(g+4.0*(h))))))))
#define SECROW(x,a,b,c,d,e,f,g,h) (x <= 3 ? SECROW_H(a,b,c,d) : SECROW_H(e,f,g,h))
#define SECROW_H(a,b,c,d) (a+8.0*(b+8.0*(c+8.0*(d))))
#define SELECT(x,i) mod(floor(i/pow(4.0,float(x))),4.0)
#define SELECTSEC(x,i) mod(floor(i/pow(8.0,float(x))),8.0)
float hash(vec2 p) { vec3 p3 = fract(vec3(p.xyx) * 0.1031); p3 += dot(p3, p3.yzx + 33.33); return fract((p3.x + p3.y) * p3.z); }

vec4 sprMarioJump(int x, int y){float col=0.;if(y==15)col=SPRROW(x,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,2.,2.,2.);if(y==14)col=SPRROW(x,0.,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,0.,0.,2.,2.,2.);if(y==13)col=SPRROW(x,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,2.,2.);if(y==12)col=SPRROW(x,0.,0.,0.,0.,0.,3.,3.,3.,2.,2.,3.,2.,0.,3.,3.,3.);if(y==11)col=SPRROW(x,0.,0.,0.,0.,3.,2.,3.,2.,2.,2.,3.,2.,2.,3.,3.,3.);if(y==10)col=SPRROW(x,0.,0.,0.,0.,3.,2.,3.,3.,2.,2.,2.,3.,2.,2.,2.,3.);if(y==9)col=SPRROW(x,0.,0.,0.,0.,3.,3.,2.,2.,2.,2.,3.,3.,3.,3.,3.,0.);if(y==8)col=SPRROW(x,0.,0.,0.,0.,0.,0.,2.,2.,2.,2.,2.,2.,2.,3.,0.,0.);if(y==7)col=SPRROW(x,0.,0.,3.,3.,3.,3.,3.,1.,3.,3.,3.,1.,3.,0.,0.,0.);if(y==6)col=SPRROW(x,0.,3.,3.,3.,3.,3.,3.,3.,1.,3.,3.,3.,1.,0.,0.,3.);if(y==5)col=SPRROW(x,2.,2.,3.,3.,3.,3.,3.,3.,1.,1.,1.,1.,1.,0.,0.,3.);if(y==4)col=SPRROW(x,2.,2.,2.,0.,1.,1.,3.,1.,1.,2.,1.,1.,2.,1.,3.,3.);if(y==3)col=SPRROW(x,0.,2.,0.,3.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,3.,3.);if(y==2)col=SPRROW(x,0.,0.,3.,3.,3.,1.,1.,1.,1.,1.,1.,1.,1.,1.,3.,3.);if(y==1)col=SPRROW(x,0.,3.,3.,3.,1.,1.,1.,1.,1.,1.,1.,0.,0.,0.,0.,0.);if(y==0)col=SPRROW(x,0.,3.,0.,0.,1.,1.,1.,1.,0.,0.,0.,0.,0.,0.,0.,0.);col=SELECT(mod(float(x),8.0),col);if(col==0.)return TRANSPARENT;if(col==1.)return RGB(177,52,37);if(col==2.)return RGB(227,157,37);if(col==3.)return RGB(106,107,4);return TRANSPARENT;}
vec4 sprMarioWalk1(int x,int y){float col=0.;if(y==15)col=SPRROW(x,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.);if(y==14)col=SPRROW(x,0.,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,0.,0.,0.,0.,0.);if(y==13)col=SPRROW(x,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.,0.);if(y==12)col=SPRROW(x,0.,0.,0.,0.,0.,3.,3.,3.,2.,2.,3.,2.,0.,0.,0.,0.);if(y==11)col=SPRROW(x,0.,0.,0.,0.,3.,2.,3.,2.,2.,2.,3.,2.,2.,2.,0.,0.);if(y==10)col=SPRROW(x,0.,0.,0.,0.,3.,2.,3.,3.,2.,2.,2.,3.,2.,2.,2.,0.);if(y==9)col=SPRROW(x,0.,0.,0.,0.,3.,3.,2.,2.,2.,2.,3.,3.,3.,3.,0.,0.);if(y==8)col=SPRROW(x,0.,0.,0.,0.,0.,0.,2.,2.,2.,2.,2.,2.,2.,0.,0.,0.);if(y==7)col=SPRROW(x,0.,0.,0.,0.,0.,3.,3.,3.,3.,1.,3.,0.,2.,0.,0.,0.);if(y==6)col=SPRROW(x,0.,0.,0.,0.,2.,3.,3.,3.,3.,3.,3.,2.,2.,2.,0.,0.);if(y==5)col=SPRROW(x,0.,0.,0.,2.,2.,1.,3.,3.,3.,3.,3.,2.,2.,0.,0.,0.);if(y==4)col=SPRROW(x,0.,0.,0.,3.,3.,1.,1.,1.,1.,1.,1.,1.,0.,0.,0.,0.);if(y==3)col=SPRROW(x,0.,0.,0.,3.,1.,1.,1.,1.,1.,1.,1.,1.,0.,0.,0.,0.);if(y==2)col=SPRROW(x,0.,0.,3.,3.,1.,1.,1.,0.,1.,1.,1.,0.,0.,0.,0.,0.);if(y==1)col=SPRROW(x,0.,0.,3.,0.,0.,0.,0.,3.,3.,3.,0.,0.,0.,0.,0.,0.);if(y==0)col=SPRROW(x,0.,0.,0.,0.,0.,0.,0.,3.,3.,3.,3.,0.,0.,0.,0.,0.);col=SELECT(mod(float(x),8.0),col);if(col==0.)return TRANSPARENT;if(col==1.)return RGB(177,52,37);if(col==2.)return RGB(227,157,37);if(col==3.)return RGB(106,107,4);return TRANSPARENT;}
vec4 sprMarioWalk2(int x,int y){float col=0.;if(y==15)col=SPRROW(x,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,0.,0.,0.,0.,0.,0.);if(y==14)col=SPRROW(x,0.,0.,0.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.,0.,0.);if(y==13)col=SPRROW(x,0.,0.,0.,0.,3.,3.,3.,2.,2.,3.,2.,0.,0.,0.,0.,0.);if(y==12)col=SPRROW(x,0.,0.,0.,3.,2.,3.,2.,2.,2.,3.,2.,2.,2.,0.,0.,0.);if(y==11)col=SPRROW(x,0.,0.,0.,3.,2.,3.,3.,2.,2.,2.,3.,2.,2.,2.,0.,0.);if(y==10)col=SPRROW(x,0.,0.,0.,3.,3.,2.,2.,2.,2.,3.,3.,3.,3.,0.,0.,0.);if(y==9)col=SPRROW(x,0.,0.,0.,0.,0.,2.,2.,2.,2.,2.,2.,2.,0.,0.,0.,0.);if(y==8)col=SPRROW(x,0.,0.,0.,0.,3.,3.,1.,3.,3.,3.,0.,0.,0.,0.,0.,0.);if(y==7)col=SPRROW(x,0.,0.,0.,3.,3.,3.,3.,1.,1.,3.,3.,0.,0.,0.,0.,0.);if(y==6)col=SPRROW(x,0.,0.,0.,3.,3.,3.,1.,1.,2.,1.,1.,2.,0.,0.,0.,0.);if(y==5)col=SPRROW(x,0.,0.,0.,3.,3.,3.,3.,1.,1.,1.,1.,1.,0.,0.,0.,0.);if(y==4)col=SPRROW(x,0.,0.,0.,1.,3.,3.,2.,2.,2.,1.,1.,1.,0.,0.,0.,0.);if(y==3)col=SPRROW(x,0.,0.,0.,0.,1.,3.,2.,2.,1.,1.,1.,0.,0.,0.,0.,0.);if(y==2)col=SPRROW(x,0.,0.,0.,0.,0.,1.,1.,1.,3.,3.,3.,0.,0.,0.,0.,0.);if(y==1)col=SPRROW(x,0.,0.,0.,0.,0.,3.,3.,3.,3.,3.,3.,3.,0.,0.,0.,0.);if(y==0)col=SPRROW(x,0.,0.,0.,0.,0.,3.,3.,3.,3.,0.,0.,0.,0.,0.,0.,0.);col=SELECT(mod(float(x),8.0),col);if(col==0.)return TRANSPARENT;if(col==1.)return RGB(177,52,37);if(col==2.)return RGB(227,157,37);if(col==3.)return RGB(106,107,4);return TRANSPARENT;}
vec4 sprMarioWalk3(int x,int y){float col=0.;if(y==15)col=SPRROW(x,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,0.,0.,0.,0.,0.,0.);if(y==14)col=SPRROW(x,0.,0.,0.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.,0.,0.);if(y==13)col=SPRROW(x,0.,0.,0.,0.,3.,3.,3.,2.,2.,3.,2.,0.,0.,0.,0.,0.);if(y==12)col=SPRROW(x,0.,0.,0.,3.,2.,3.,2.,2.,2.,3.,2.,2.,2.,0.,0.,0.);if(y==11)col=SPRROW(x,0.,0.,0.,3.,2.,3.,3.,2.,2.,2.,3.,2.,2.,2.,0.,0.);if(y==10)col=SPRROW(x,0.,0.,0.,3.,3.,2.,2.,2.,2.,3.,3.,3.,3.,0.,0.,0.);if(y==9)col=SPRROW(x,0.,0.,0.,0.,0.,2.,2.,2.,2.,2.,2.,2.,0.,0.,0.,0.);if(y==8)col=SPRROW(x,0.,0.,3.,3.,3.,3.,1.,1.,3.,3.,0.,0.,0.,0.,0.,0.);if(y==7)col=SPRROW(x,2.,2.,3.,3.,3.,3.,1.,1.,1.,3.,3.,3.,2.,2.,2.,0.);if(y==6)col=SPRROW(x,2.,2.,2.,0.,3.,3.,1.,2.,1.,1.,1.,3.,3.,2.,2.,0.);if(y==5)col=SPRROW(x,2.,2.,0.,0.,1.,1.,1.,1.,1.,1.,1.,0.,0.,3.,0.,0.);if(y==4)col=SPRROW(x,0.,0.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,3.,3.,0.,0.);if(y==3)col=SPRROW(x,0.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,3.,3.,0.,0.);if(y==2)col=SPRROW(x,0.,3.,3.,1.,1.,1.,0.,0.,0.,1.,1.,1.,3.,3.,0.,0.);if(y==1)col=SPRROW(x,0.,3.,3.,3.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.);if(y==0)col=SPRROW(x,0.,0.,3.,3.,3.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.);col=SELECT(mod(float(x),8.0),col);if(col==0.)return TRANSPARENT;if(col==1.)return RGB(177,52,37);if(col==2.)return RGB(227,157,37);if(col==3.)return RGB(106,107,4);return TRANSPARENT;}
vec4 sprGround(int x,int y){float col=0.;if(y==15)col=SPRROW(x,1.,0.,0.,0.,0.,0.,0.,0.,0.,2.,1.,0.,0.,0.,0.,1.);if(y==14)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,2.);if(y==13)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,2.);if(y==12)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,2.);if(y==11)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,2.,1.,1.,1.,2.);if(y==10)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,1.,2.,2.,2.,2.,1.);if(y==9)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,0.,0.,0.,0.,2.);if(y==8)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,2.);if(y==7)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,2.);if(y==6)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,2.);if(y==5)col=SPRROW(x,2.,2.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,1.,2.);if(y==4)col=SPRROW(x,0.,0.,2.,2.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,1.,2.);if(y==3)col=SPRROW(x,0.,1.,0.,0.,2.,2.,2.,2.,0.,1.,1.,1.,1.,1.,1.,2.);if(y==2)col=SPRROW(x,0.,1.,1.,1.,0.,0.,0.,2.,0.,1.,1.,1.,1.,1.,1.,2.);if(y==1)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,2.,0.,1.,1.,1.,1.,1.,2.,2.);if(y==0)col=SPRROW(x,1.,2.,2.,2.,2.,2.,2.,1.,0.,2.,2.,2.,2.,2.,2.,1.);col=SELECT(mod(float(x),8.0),col);if(col==0.)return RGB(247,214,181);if(col==1.)return RGB(231,90,16);return RGB(0,0,0);}
vec4 sprQuestionBlock(int x,int y){float col=0.;if(y==15)col=SPRROW(x,3.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,3.);if(y==14)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,2.);if(y==13)col=SPRROW(x,0.,1.,2.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,2.,1.,2.);if(y==12)col=SPRROW(x,0.,1.,1.,1.,1.,0.,0.,0.,0.,0.,1.,1.,1.,1.,1.,2.);if(y==11)col=SPRROW(x,0.,1.,1.,1.,0.,0.,2.,2.,2.,0.,0.,1.,1.,1.,1.,2.);if(y==10)col=SPRROW(x,0.,1.,1.,1.,0.,0.,2.,1.,1.,0.,0.,2.,1.,1.,1.,2.);if(y==9)col=SPRROW(x,0.,1.,1.,1.,0.,0.,2.,1.,1.,0.,0.,2.,1.,1.,1.,2.);if(y==8)col=SPRROW(x,0.,1.,1.,1.,1.,2.,2.,1.,0.,0.,0.,2.,1.,1.,1.,2.);if(y==7)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,0.,0.,2.,2.,2.,1.,1.,1.,2.);if(y==6)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,0.,0.,2.,1.,1.,1.,1.,1.,2.);if(y==5)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,2.,2.,1.,1.,1.,1.,1.,2.);if(y==4)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,0.,0.,0.,1.,1.,1.,1.,1.,2.);if(y==3)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,0.,0.,2.,1.,1.,1.,1.,1.,2.);if(y==2)col=SPRROW(x,0.,1.,2.,1.,1.,1.,1.,1.,2.,2.,1.,1.,1.,2.,1.,2.);if(y==1)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,2.);if(y==0)col=SPRROW(x,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.,2.);if(y<0||y>15)return RGB(107,140,255);col=SELECT(mod(float(x),8.),col);if(col==0.)return RGB(231,90,16);if(col==1.)return RGB(255,165,66);if(col==2.)return RGB(0,0,0);return RGB(107,140,255);}
vec4 sprUsedBlock(int x,int y){float col=0.;if(y==15)col=SPRROW(x,3.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,3.);if(y==14)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==13)col=SPRROW(x,0.,1.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.,1.,0.);if(y==12)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==11)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==10)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==9)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==8)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==7)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==6)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==5)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==4)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==3)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==2)col=SPRROW(x,0.,1.,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.,1.,0.);if(y==1)col=SPRROW(x,0.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,1.,0.);if(y==0)col=SPRROW(x,3.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,3.);if(y<0||y>15)return RGB(107,140,255);col=SELECT(mod(float(x),8.),col);if(col==0.)return RGB(0,0,0);if(col==1.)return RGB(231,90,16);return RGB(107,140,255);}
vec4 getTile(int t,int x,int y){if(t==0)return RGB(107,140,255);if(t==1)return sprGround(x,y);if(t==2)return sprQuestionBlock(x,y);if(t==3)return sprUsedBlock(x,y);return RGB(107,140,255);}
int getSection(int s,int x,int y){float col=0.;if(s==0){if(y==6)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==5)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==4)col=SECROW(x,0.,0.,3.,3.,3.,0.,0.,0.);if(y==3)col=SECROW(x,0.,0.,2.,2.,2.,0.,0.,0.);if(y==2)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==1)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y<=0)col=SECROW(x,1.,1.,1.,1.,1.,1.,1.,1.);}if(s==1){if(y==6)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==5)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==4)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==3)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==2)col=SECROW(x,0.,0.,0.,0.,0.,1.,0.,0.);if(y==1)col=SECROW(x,0.,0.,0.,1.,1.,1.,0.,0.);if(y<=0)col=SECROW(x,1.,1.,1.,1.,1.,1.,1.,1.);}if(s==2){if(y==6)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==5)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==4)col=SECROW(x,0.,0.,3.,0.,0.,3.,0.,0.);if(y==3)col=SECROW(x,0.,0.,2.,0.,0.,2.,0.,0.);if(y==2)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==1)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y<=0)col=SECROW(x,1.,1.,1.,1.,1.,1.,1.,1.);}if(s==3){if(y==6)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==5)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==4)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==3)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==2)col=SECROW(x,0.,0.,0.,1.,1.,0.,0.,0.);if(y==1)col=SECROW(x,0.,0.,0.,1.,1.,1.,0.,0.);if(y<=0)col=SECROW(x,1.,1.,1.,1.,1.,1.,1.,1.);}if(s==4){if(y==6)col=SECROW(x,0.,0.,0.,0.,3.,0.,0.,0.);if(y==5)col=SECROW(x,0.,0.,0.,0.,2.,0.,0.,0.);if(y==4)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==3)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==2)col=SECROW(x,0.,0.,0.,1.,1.,1.,0.,0.);if(y==1)col=SECROW(x,0.,0.,0.,1.,1.,1.,0.,0.);if(y<=0)col=SECROW(x,1.,1.,1.,1.,1.,1.,1.,1.);}if(s==5){if(y==6)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==5)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==4)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==3)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==2)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y==1)col=SECROW(x,0.,0.,0.,0.,0.,0.,0.,0.);if(y<=0)col=SECROW(x,1.,1.,1.,0.,0.,1.,1.,1.);}return int(SELECTSEC(mod(float(x),4.0),col));}
int getBlock(int x,int y){if(y>6)return 0;int section=int(hash(vec2(int(float(x)/8.0),3.0))*6.0);int sectionX=int(mod(float(x),8.0));return getSection(section,sectionX,y-int(hash(vec2(section,2.0))*0.0));}
bool isSolid(int b){return(b!=0);}

mat2 rotar(float a) { return mat2(cos(a), -sin(a), sin(a), cos(a)); }

// Función de simulación de Mario separada para mayor claridad
void simularMario(float screenX, out float marioX, out float marioY, out bool onGround) {
    marioX = screenX;
    marioY = 16.0;
    float marioYSpd = 0.0;
    
    // Simulación simplificada para encontrar la posición Y
    for(int i = 1; i < 4; i++) {
        if (!isSolid(getBlock(int(marioX / 16.0), i))) { marioY = float(i) * 16.0; break; }
    }
    
    // Lógica de física de salto y movimiento
    const int simSteps = 20; // 20 pasos de simulación son suficientes
    float lastY = marioY;
    onGround = false;
    for(int sim = 0; sim < simSteps; sim++) {
        lastY = marioY;
        onGround = false;
        if (isSolid(getBlock(int(marioX / 16.0) + 1, int(marioY / 16.0)))) marioY = (floor(marioY / 16.0) * 16.0) + 16.0;
        marioYSpd -= 2.5;
        marioY += marioYSpd;
        if (marioYSpd > 0.0) {
            if (isSolid(getBlock(int(floor((marioX + 12.0) / 16.0)), int(floor((marioY + 15.9) / 16.0))))) { marioYSpd *= -0.5; marioY = (floor(marioY / 16.0) * 16.0); }
        }
        if (marioYSpd < 0.0) {
            if (isSolid(getBlock(int(floor((marioX) / 16.0)), int(floor(marioY / 16.0)))) || isSolid(getBlock(int(floor((marioX + 15.9) / 16.0)), int(floor(marioY / 16.0))))) { marioYSpd = 0.0; marioY = (floor(marioY / 16.0) * 16.0) + 16.0; onGround = true; }
        }
        marioX += 4.0;
        if (onGround) {
            if (!isSolid(getBlock(int((marioX) / 16.0) + 1,0))) marioYSpd = 1.5;
            else if (isSolid(getBlock(int((marioX + 36.0) / 16.0), int((marioY + 24.0) / 16.0)))) marioYSpd = 15.5;
            else if (isSolid(getBlock(int((marioX) / 16.0) + 2, int((marioY + 8.0) / 16.0)))) marioYSpd = 12.5;
            else if (getBlock(int((marioX) / 16.0) + 1, int((marioY + 8.0) / 16.0) + 2) == 2) marioYSpd = 15.5;
        }
    }
    marioY = mix(lastY,marioY,mod(TIME * 15.0,1.0)) - 1.0;
}

void renderWorld(vec2 fragCoord, out vec4 backgroundColor, out vec4 marioColor) {
    marioColor = TRANSPARENT;
    float x = fragCoord.x / 2.0;
    float y = fragCoord.y / 2.0;
    if (RENDERSIZE.y >= 640.0) { x /= 2.0; y /= 2.0; }
    if (RENDERSIZE.y < 200.0) { x *= 2.0; y *= 2.0; }
    y -= 8.0;

    int firstBlockX = int((TIME * Velocidad) / 16.0);
    int blockX = int((x + TIME * Velocidad) / 16.0);
    int blockY = int(y / 16.0);
    int block = getBlock(blockX,blockY);
    
    int subx = int(mod((x + TIME * Velocidad),16.0));
    int suby = int(mod(y,16.0));
    
    // Lógica de bloques de pregunta
    if (block == 2) {
        if (blockX - firstBlockX == 5) suby -= int(max(0.0,(sin(mod((TIME * Velocidad / 16.0),1.0) * PI * 1.5) * 8.0)));
        if ((floor((x + TIME * Velocidad) / 16.0) - (TIME * Velocidad) / 16.0) < 4.25) block = 3;
    } else if (block == 3) {
        block = 2;
        suby += 16;
        if (blockX - firstBlockX == 5) suby -= int(max(0.0,(sin(mod((TIME * Velocidad / 16.0),1.0) * PI * 1.5) * 8.0)));
    }
    backgroundColor = getTile(block,subx,suby);
    
    // --- LÓGICA DE MARIO REFACTORIZADA ---
    float screenX = TIME * Velocidad;
    float marioX, marioY;
    bool onGround;
    simularMario(screenX, marioX, marioY, onGround);
    
    // Se calcula el tamaño y la posición de renderizado ANTES de comprobar los límites.
    float marioScale = 1.0 + Gigante * 8.0;
    float marioWidth = 16.0 * marioScale;
    float marioHeight = 16.0 * marioScale;
    float marioRenderX = 80.0 - (marioWidth - 16.0) * 0.5; // Mario permanece centrado en su "caja" original
    float marioRenderY = marioY - (marioHeight - 16.0);

    // La condición de renderizado ahora es dinámica, basada en el tamaño real de Mario.
    if (x >= marioRenderX && x < marioRenderX + marioWidth && y >= marioRenderY && y < marioRenderY + marioHeight) {
        float normalizedX = (x - marioRenderX) / marioWidth;
        float normalizedY = (y - marioRenderY) / marioHeight;
        
        // Deformación de Mario Gigante
        if (Gigante > 0.0) {
            float waveX = sin(normalizedY * PI * 3.0 + TIME * 2.0) * 0.1 * Gigante;
            float waveY = sin(normalizedX * PI * 2.0 + TIME * 1.5) * 0.08 * Gigante;
            normalizedX += waveX;
            normalizedY += waveY;
        }
        
        int marioSubX = int(normalizedX * 16.0);
        int marioSubY = int(normalizedY * 16.0);

        vec4 spr = TRANSPARENT;
        if (onGround) {
            int f = int(mod(TIME * 10.0, 3.0));
            if (f == 0) spr = sprMarioWalk1(marioSubX, marioSubY);
            if (f == 1) spr = sprMarioWalk2(marioSubX, marioSubY);
            if (f == 2) spr = sprMarioWalk3(marioSubX, marioSubY);
        } else {
            spr = sprMarioJump(marioSubX, marioSubY);
        }

        // Efecto de Corrupción solo para Mario
        if (CorrupcionMario > 0.0 && spr.a > 0.5) {
            float noise = hash(vec2(normalizedX, normalizedY) * 10.0 + TIME);
            spr.rgb = mix(spr.rgb, vec3(noise), CorrupcionMario);
        }

        if (spr.a > 0.5) marioColor = spr;
    }
}

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE;

    // Read feedback buffer properly
    vec4 memoria = texture2D(feedbackBuffer, uv);
    float energia_evento = memoria.a;
    
    vec4 backgroundColor;
    vec4 marioColor;
    renderWorld(gl_FragCoord.xy, backgroundColor, marioColor);

    // Efecto Ácido Global (se mantiene sin cambios)
    if (AcidoGlobal > 0.0) {
        float colorShift = TIME * 2.0 * AcidoGlobal;
        vec3 originalBg = backgroundColor.rgb;
        backgroundColor.r = originalBg.r * cos(colorShift) - originalBg.g * sin(colorShift);
        backgroundColor.g = originalBg.r * sin(colorShift) + originalBg.g * cos(colorShift);
        backgroundColor.b = originalBg.b * cos(colorShift * 0.7) + originalBg.r * sin(colorShift * 0.7) * 0.5;
        if (marioColor.a > 0.5) {
            vec3 originalMario = marioColor.rgb;
            marioColor.r = originalMario.r * cos(colorShift + 1.0) - originalMario.g * sin(colorShift + 1.0);
            marioColor.g = originalMario.r * sin(colorShift + 1.0) + originalMario.g * cos(colorShift + 1.0);
        }
    }
    
    vec4 compositedColor = mix(backgroundColor, marioColor, marioColor.a);

    // Feedback basado en Cisma Dimensional
    if (FuerzaFeedback > 0.01) {
        float angulo_cisma = TIME * 0.5 + CismaFeedback;
        vec2 dir_cisma = vec2(cos(angulo_cisma), sin(angulo_cisma));
        vec2 dislocacion = dir_cisma * 0.02 * (0.5 + sin(TIME * 5.0) * 0.5);
        
        // Use texture2D for feedback buffer reading
        vec4 feedback = texture2D(feedbackBuffer, uv + dislocacion);
        
        // El feedback ahora es más oscuro y con más contraste, menos "flashero"
        feedback.rgb = pow(feedback.rgb, vec3(1.2)) * 0.95;
        compositedColor = mix(compositedColor, feedback, FuerzaFeedback);
    }
    
    // Efecto Global de Falla / Aniquilación
    if (abs(Falla) > 0.0) {
        vec2 centro = uv - 0.5;
        centro.x *= RENDERSIZE.x / RENDERSIZE.y;
        float angulo_falla = TIME * 0.25 + Falla;
        vec2 dir_perp = vec2(-cos(angulo_falla), sin(angulo_falla));
        float dist_a_falla = dot(centro, dir_perp);
        float intensidad_aniquilacion = pow(1.0 - smoothstep(0.0, 0.05, abs(dist_a_falla)), 4.0);
        compositedColor.rgb *= (1.0 - intensidad_aniquilacion); // La Falla consume la luz
    }

    // Evento de Convergencia desde los bordes
    float dist_a_borde_y = 0.5 - abs(uv.y - 0.5);
    float radio_onda = energia_evento * 0.5;
    float onda = smoothstep(0.02, 0.0, abs(dist_a_borde_y - radio_onda));
    float intensidad_evento = clamp(onda * 2.0, 0.0, 1.0);
    // La onda empuja el color hacia su negativo, un efecto oscuro e interesante
    compositedColor.rgb = mix(compositedColor.rgb, 1.0 - compositedColor.rgb, intensidad_evento);

    float nueva_energia = Convergencia ? 1.0 : max(0.0, energia_evento - 0.04);
    
    // Salida final: se guarda el color en RGB y la energía del evento en el Alfa.
    gl_FragColor = vec4(clamp(compositedColor.rgb, 0.0, 1.0), nueva_energia);
}