var game = new Phaser.Game(800, 600, Phaser.AUTO, 'game', { preload: preload, create: create, update: update });

var player;
var ball;
var cursors;

function preload() {
	game.load.spritesheet('reimu', 'assets/reimu_sheet.png', 32, 46);
	game.load.image('ball', 'assets/ball.png');
}

function create() {    
	//system
	cursors = game.input.keyboard.createCursorKeys();
	game.physics.startSystem(Phaser.Physics.P2JS);
	game.physics.p2.gravity.y = 100;
    game.physics.p2.restitution = 1;
    game.physics.p2.setImpactEvents(true);

	//player
	player = game.add.sprite(game.world.centerX, 600 - 23, 'reimu');
	game.physics.p2.enable(player, true);
	player.animations.add('idle');
	//player.scale.set(1.2, 1.2);
	player.anchor.setTo(0.5, 0.5);
	player.body.fixedRotation = true;
	player.body.setZeroDamping();
	player.speed = 500;
	

	//ball
	ball = game.add.sprite(0, 500, 'ball');
	game.physics.p2.enable(ball, true);
	ball.scale.set(0.2, 0.2);
    ball.anchor.setTo(0.5, 0.5);
	ball.body.setCircle(20);
	ball.body.angularVelocity += 5;

	//collisions
    var playerCollisionGroup = game.physics.p2.createCollisionGroup();
    var ballCollisionGroup = game.physics.p2.createCollisionGroup();
    ball.body.setCollisionGroup(ballCollisionGroup);
   	player.body.setCollisionGroup(playerCollisionGroup);
   	player.body.collides([ballCollisionGroup]);
    game.physics.p2.updateBoundsCollisionGroup(); //lastly update bouds collision group
}

var oldX;
function update() {
	player.animations.play('idle', 5);
	player.body.setZeroVelocity();

    if (cursors.left.isDown) {
    	player.body.moveLeft(400);
    } else if (cursors.right.isDown) {
    	player.body.moveRight(400);
    }

	game.debug.spriteInfo(player, 32, 32);
}	

// Converts from degrees to radians.
Math.radians = function(degrees) {
  return degrees * Math.PI / 180;
};
 
// Converts from radians to degrees.
Math.degrees = function(radians) {
  return radians * 180 / Math.PI;
};

