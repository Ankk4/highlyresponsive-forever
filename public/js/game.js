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

	//ball
	ball = game.add.sprite(32, 500, 'ball');
	ball.physicsBodyType = Phaser.Physics.ARCADE;
	game.physics.enable(ball, Phaser.Physics.ARCADE);
	ball.body.collideWorldBounds = true;
	ball.body.allowRotation = true;
	ball.body.allowGravity = true;
	ball.body.gravity.set(0, 100);
	ball.body.bounce.x = 0.8;
	ball.body.bounce.y = 0.8;	
 	ball.anchor.setTo(0.5, 0.5);	
	ball.scale.set(0.2, 0.2);

	//initial velocity
    ball.body.angularVelocity = 400;
	ball.body.velocity.set(100, -150);

	//collisions
	game.physics.arcade.overlap(ball, bullets, bulletHitBall, null, this);
	game.physics.arcade.overlap(ball, player, ballHitPlayer)
}

var perkele = 5;
function update() {
	player.animations.play('idle', 5);

    if (cursors.left.isDown) {
    	player.x -= player.speed;
    } else if (cursors.right.isDown) {
    	player.x += player.speed;
    }
    
   	if (checkOverlap(player, ball)) {
   		//ball overlapping player
   		ballHitPlayer();
   	}
   	
    if (fireButton.isDown) {
		//Fire player bullet
    }

	if (ball.body.blocked.left) {
		ball.body.angularVelocity = 400;
		perkele *= -1;
	} else if (ball.body.blocked.right) {
		ball.body.angularVelocity = -400;
		perkele *= -1;
	} else if (ball.body.blocked.down) {
		ball.body.velocity.y -= 20;
	}

	//perkele = min speed - parempi tapa tehä tämä?
	ball.body.x += perkele;

	game.debug.body(ball);
	game.debug.bodyInfo(ball, 32, 32);

}

function checkOverlap(spriteA, spriteB) {
    var boundsA = spriteA.getBounds();
    var boundsB = spriteB.getBounds();
    return Phaser.Rectangle.intersects(boundsA, boundsB);
}

function ballHitPlayer () {
	//code for player gettin hit
	// only for ball or bullets as well?
}

function bulletHitBall (bullet) {
	ball.body.velocity.y -= 10;
	resetBullet(bullet);
	//maybe add animation for bullet destruction	
}

// Converts from degrees to radians.
Math.radians = function(degrees) {
  return degrees * Math.PI / 180;
};
 
// Converts from radians to degrees.
Math.degrees = function(radians) {
  return radians * 180 / Math.PI;
};

