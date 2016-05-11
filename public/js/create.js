var bounds = new Phaser.Rectangle(0, 0, 1024, 620);
var customBounds;

var playerBullets;
var player;
var ball;
var cursors;
var fireButton;
var playerCG;
var ballCG;
var bulletCG;

function create() {  
	//system
	game.world.setBounds(0, 0, 1024, 720);
	game.physics.startSystem(Phaser.Physics.P2JS);
	game.physics.p2.setImpactEvents(true);
	game.physics.p2.gravity.y   = 800;
	game.physics.p2.restitution = 0.99;

	// Custom bounds	
    customBounds = { left: null, right: null, top: null, bottom: null };
    createPreviewBounds(bounds.x, bounds.y, bounds.width, bounds.height);

    // Keys
	cursors    = game.input.keyboard.createCursorKeys();
	fireButton = game.input.keyboard.addKey(Phaser.Keyboard.Z);
	fireButton.onDown.add(fireBullet, this)

	//player
	player                         = game.add.sprite(game.world.centerX, game.world.height - 128 , 'reimu');
	player.physicsBodyType         = Phaser.Physics.P2JS;
	player.scale.set(0.3, 0.3); //Must be before physics 
	player.anchor.setTo(0.5, 0.5);
	game.physics.p2.enable([ player ], true);
	player.body.allowRotation      = false;	
	player.body.allowGravity       = false;
	player.body.kinematic          = true;	
	player.body.collideWorldBounds = true;
	player.animations.add('idle');
	player.body.setCircle(30);
	
	//player bullets
	var bullet;	
	playerBullets = game.add.group();	
	playerBullets.physicsBodyType = Phaser.Physics.P2JS;

	for (var i = 15 - 1; i >= 0; i--) {
		bullet                  = game.add.sprite(0,0,'bullet');
		bullet.scale.set(1.5);
		game.physics.p2.enable(bullet,false);
		bullet.physicsBodyType  = Phaser.Physics.P2JS;
		bullet.exists           = false;
		bullet.body.kinematic   = true;
		bullet.checkOutOfBounds = false;
		bullet.outOfBoundsKill  = false;
		bullet.body.mass        = 20;
		playerBullets.add(bullet);
	}

    //Ball
	ball = game.add.sprite(32, 32, 'ball');
	ball.physicsBodyType    = Phaser.Physics.P2JS;
	ball.scale.set(0.2, 0.2); //must be before physics
	game.physics.p2.enable([ ball ], true);
	ball.body.allowRotation = true;	
	ball.body.allowGravity  = true;
 	ball.anchor.setTo(0.5, 0.5);
	ball.body.setCircle(25);
	ball.body.x = 32;
	ball.body.y = 300;
	ball.body.velocity.x = 200;
	ball.body.velocity.y = -300;
	ball.body.mass = 50;

    //Collision groups - jatka tästä.
	playerCG = game.physics.p2.createCollisionGroup(); 
	player.body.setCollisionGroup(playerCG);
	player.body.collides([ballCG])

	//this.ballCG   = game.physics.p2.createCollisionGroup();
	//this.bulletCG = game.physics.p2.createCollisionGroup();
 	//game.physics.p2.updateBoundsCollisionGroup();

	//  Just to display the bounds
    var graphics = game.add.graphics(bounds.x, bounds.y);
    graphics.lineStyle(4, 0xFFFFFF, 1);
    graphics.drawRect(0, 0, bounds.width, bounds.height);
}

function createPreviewBounds(x, y, w, h) {

    var sim = game.physics.p2;

    //  If you want to use your own collision group then set it here and un-comment the lines below
    var boundsCG = sim.boundsCollisionGroup.mask;

    customBounds.left = new p2.Body({ mass: 0, position: [ sim.pxmi(x), sim.pxmi(y) ], angle: 1.5707963267948966 });
    customBounds.left.addShape(new p2.Plane());
    customBounds.left.shapes[0].collisionGroup = boundsCG;

    customBounds.right = new p2.Body({ mass: 0, position: [ sim.pxmi(x + w), sim.pxmi(y) ], angle: -1.5707963267948966 });
    customBounds.right.addShape(new p2.Plane());
    customBounds.right.shapes[0].collisionGroup = boundsCG;

    customBounds.top = new p2.Body({ mass: 0, position: [ sim.pxmi(x), sim.pxmi(y) ], angle: -3.141592653589793 });
    customBounds.top.addShape(new p2.Plane());
    customBounds.top.shapes[0].collisionGroup = boundsCG;

    customBounds.bottom = new p2.Body({ mass: 0, position: [ sim.pxmi(x), sim.pxmi(y + h) ] });
    customBounds.bottom.addShape(new p2.Plane());
    customBounds.bottom.shapes[0].collisionGroup = boundsCG;

    sim.world.addBody(customBounds.left);
    sim.world.addBody(customBounds.right);
    sim.world.addBody(customBounds.top);
    sim.world.addBody(customBounds.bottom);
}
