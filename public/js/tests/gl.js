function glStart() {
    var canvas = document.getElementById("glcanvas");
    initGL(canvas);
    initShaders();
    initBuffers();

    gl.clearColor(0.0, 0.0, 0.0, 1.0);
    gl.enable(gl.DEPTH_TEST);

	drawScene();
}

var triangleVertexPositionBuffer;
var squareVertexPositionBuffer;

function initBuffers() {
	triangleVertexPositionBuffer = gl.createBuffer();
}