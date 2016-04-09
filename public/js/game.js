var game = new Phaser.Game(1024, 768, Phaser.AUTO, 'game', { preload: preload, create: create, update: update });

var player;
var ball;
var cursors;
var fireButton;
var bullets;
var bulletTime = 0;


function update() {
	player.body.setZeroVelocity();
	keyPresses();

	// Play idle animation
    player.animations.play('idle', 5);

	// collisions
	game.physics.arcade.overlap(ball, bullets, bulletHitBall, null, this);
	game.physics.arcade.overlap(ball, player, ballHitPlayer, null, this);
	
	if (checkPlayerBounds(player, bounds)) {
		if (player.body.x > 512)
			player.body.x = 1024;
		else {
			player.body.x = 0;
		} 
	}
}

// HANDLER FOR PLAYER BULLETS
function fireBullet () {
    if (game.time.now > bulletTime) {
        //  Grab the first bullet we can from the pool
        bullet = bullets.getFirstExists(false);

        if (bullet) {
            //  And fire it
            bullet.reset(player.x, player.y + 8);
            bullet.body.velocity.y = -800;
            bulletTime = game.time.now + 100;
        }
    }
}

// Check for handlers
function resetBullet (bullet) {
    bullet.kill();
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
	//ball.body.velocity.y = -400;
	var defaultForce = 200;

	//bounce left or right depending on impact angle - Not working
	console.log("Distance: ", Phaser.Point.distance(bullete.body.position, balle.body.position));

	ball.body.velocity.y = -400;
	bullete.kill();
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
    	player.body.moveLeft(400);
    }
    else if (cursors.right.isDown) {
    	player.body.moveRight(400);
    }

    if (fireButton.isDown) {
		fireBullet();
    }
}