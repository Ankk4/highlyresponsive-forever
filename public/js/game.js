var game = new Phaser.Game(800, 600, Phaser.AUTO, 'game', { preload: preload, create: create, update: update });

var player;
var ball;
var cursors;
var fireButton;
var bullets;
var bulletTime = 0;

function preload() {
	game.load.spritesheet('reimu', 'assets/reimu_sheet.png', 32, 46);
	game.load.image('ball', 'assets/ball.png');
	game.load.image('bullet', 'assets/bullet.png');
}

function create() {  
	//system
	cursors = game.input.keyboard.createCursorKeys();
	fireButton = game.input.keyboard.addKey(Phaser.Keyboard.Z);
	game.physics.startSystem(Phaser.Physics.ARCADE);
	game.physics.arcade.gravity.y = 100;

	//player
	player = game.add.sprite(game.world.centerX, 600 - 23, 'reimu');
	player.physicsBodyType = Phaser.Physics.ARCADE;
	game.physics.enable(player, Phaser.Physics.ARCADE);
	player.body.collideWorldBounds = true;
	player.animations.add('idle');
	player.anchor.setTo(0.5, 0.5);
	player.body.allowRotation = false;
	player.body.allowGravity = false;
	player.speed = 5;	

	//player bullets
    bullets = game.add.group();
    bullets.enableBody = true;
    bullets.physicsBodyType = Phaser.Physics.ARCADE;
    bullets.createMultiple(15, 'bullet');
    bullets.setAll('anchor.x', 0.5);
    bullets.setAll('outOfBoundsKill', true);
    bullets.setAll('checkWorldBounds', true);
    bullets.setAll('scale.x', 1.5);

	//ball -- Pitää vaihtaa takas p2 engineen, koska arcade ei tue circleä -_-
	ball = game.add.sprite(32, 500, 'ball');
	ball.physicsBodyType = Phaser.Physics.ARCADE;
	game.physics.enable(ball, Phaser.Physics.ARCADE);
	ball.body.hasCollided = false;
	ball.body.collideWorldBounds = true;
	ball.body.allowRotation = true;
	ball.body.allowGravity = true;
	ball.body.gravity.set(0, 200);
	ball.body.bounce.x = 0.8;
	ball.body.bounce.y = 0.8;
 	ball.anchor.setTo(0.5, 0.5);	
	ball.scale.set(0.5, 0.5);
	//dirty solution to add circle?
	//var circle = new circle(ball.x, ball.y, ball.body.halfHeight);

	//initial velocity
    ball.body.angularVelocity = 400;
	ball.body.velocity.set(100, -150);
}

function update() {
	// Play idle animation
    player.animations.play('idle', 5);

    // Check keypresses - move to own file? "enums"
    if (cursors.left.isDown) {
    	player.x -= player.speed;
    } else if (cursors.right.isDown) {
    	player.x += player.speed;
    }    	
    if (fireButton.isDown) {
		fireBullet();
    }
    
    // Check side collisions - this needs to be function and some work
    if (ball.body.blocked.left) {
		ball.body.angularVelocity = 400;
	} else if (ball.body.blocked.right) {
		ball.body.angularVelocity = -400;
	} else if (ball.body.blocked.down) {
		ball.body.velocity.y -= 20;
	}

	// collisions
	game.physics.arcade.overlap(ball, bullets, bulletHitBall, null, this);
	game.physics.arcade.overlap(ball, player, ballHitPlayer, null, this);

    // debugs
	game.debug.body(ball);
	game.debug.bodyInfo(ball, 32, 32);
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

//obsolete?
function checkOverlapRectangle (spriteA, spriteB) {
    var boundsA = spriteA.getBounds();
    var boundsB = spriteB.getBounds();
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
	ball.body.velocity.y = -400;
	//bounce left or right depending on impact angle?
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

