"vec2 cMul(vec2 a, vec2 b) {",
	"return vec2(a.x*b.x -  a.y*b.y,a.x*b.y + a.y * b.x);",
"}",

"vec2 cInverse(vec2 a) {",
	"return	vec2(a.x,-a.y)/dot(a,a);",
"}",


"vec2 cDiv(vec2 a, vec2 b) {",
	"return cMul( a,cInverse(b));",
"}",


"vec2 cPower(vec2 z, float n) {",
	"float r2 = dot(z,z);",
	"return pow(r2,n/2.0)*vec2(cos(n*atan(z.y/z.x)),sin(n*atan(z.y/z.x)));",
"}",

"void mainImage( out vec4 fragColor, in vec2 fragCoord )",
"{",
	"vec2 p = fragCoord.xy / iResolution.xy;",
	
	"float t = iGlobalTime;",

	"float zPower = 1.16578;",
	"float aa = -0.15000;",
	"float bb = 0.89400;",
	"float cc = -0.05172;",
	"float dd = 0.10074;",
	"int i = 0;",

	"vec2 A = vec2(aa, aa);",
	"vec2 B = vec2(bb, bb);",
	"vec2 C = vec2(cc, cc);",
	"vec2 D = vec2(dd, dd);",
	
	"float speed = 0.25;",
	"vec2 c = vec2(cos(t*speed), sin(t*speed));",
	"float s = 2.5;",
    "vec2 z = s*((-1.0 + 2.0*p)*vec2(iResolution.x/(iResolution.y),1.0));",
	"const int iter = 96;",
	"float e = 128.0;",

    "for( int j=0; j<iter; j++ )",
    "{",
		"z = cPower(z, zPower);",
		"z = abs(z);",
		"z = cMul(z,z) + c;",
		
		"z = cDiv((cMul(A, z) + B), (cMul(z,C) + D));",
		
    	"if (dot(z,z) > e) break;",
		"i++;",
	"}",
	
	"float ci = float(i) + 1.0 - log2(0.5*log2(dot(z,z)));",

	"float red = 0.5 + 0.5*cos(6.0*ci+0.0);",
	"float green = 0.5+0.5*cos(6.0*ci+0.4);",
	"float blue = 0.5+0.5*cos(6.0*ci+0.8);",

	"fragColor = vec4(red, green, blue, 1.0);",
"}"
