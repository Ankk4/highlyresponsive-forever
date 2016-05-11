var game = new Phaser.Game(1024, 720, Phaser.AUTO, 'game', { preload: preload, create: create, update: update });
var bulletTime = 0;

function update() {
	player.body.setZeroVelocity();
	keyPresses();

	// Play idle animation
    player.animations.play('idle', 5);

	// collisions
	game.physics.arcade.overlap(ball, playerBullets, bulletHitBall, null, this);
	game.physics.arcade.overlap(ball, player, ballHitPlayer, null, this);

	// Ball spinning "naturally"
	ball.body.angularVelocity = ball.body.velocity.x / 35;
    playerBullets = bulletOutOfBounds(playerBullets);
}

// HANDLER FOR PLAYER BULLETS
function fireBullet () {
    if (game.time.now > bulletTime) {
        //  Grab the first bullet we can from the pool
        bullet = playerBullets.getFirstExists(false);

        if (bullet) {
            //  And fire it
            bullet.reset(player.x, player.y + 8);
            bullet.body.velocity.y = -800;
            bulletTime = game.time.now + 100;
        }
    }
}

//Disable bullets for recyling when they leave the screen
function bulletOutOfBounds(bulletGroup) {
	for (var i = bulletGroup.children.length - 1; i >= 0; i--) {
		if(bulletGroup.children[i].position.y < 0) {
			bulletGroup.children[i].exists = false;
		}
	}
	return bulletGroup;
}

function checkPlayerBounds(r1, r2) {
	return !checkOverlapRectangle(r1, r2, {"a":true,"b":false});	
}

//obsolete?
function checkOverlapRectangle (objA, objB, params) {
	if(params.a) var boundsA = objA.getBounds();
	else var boundsA = objA;
	if(params.b) var boundsB = objB.getBounds();
	else var boundsB = objB;

	return Phaser.Rectangle.intersects(boundsA, boundsB);
}

function checkOverlapCircle (c, r) {
	return Phaser.Circle.intersectsRectangle(c,r);
}
// Is this necessery?
function checkOverlapDif (cir, rect) {
}

// Player hit detection callback
function ballHitPlayer () {
	console.log("player hit");
}

function bulletHitBall (balle, bullete) {
	console.log("Ball hit");
}

// Converts from degrees to radians.
Math.radians = function(degrees) {
    return degrees * Math.PI / 180;
};
 
// Converts from radians to degrees.
Math.degrees = function(radians) {
    return radians * 180 / Math.PI;
};

function keyPresses() {
    if (cursors.left.isDown){
    	player.body.moveLeft(500);
    }
    else if (cursors.right.isDown) {
    	player.body.moveRight(500);
    }
}
