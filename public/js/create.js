var bounds = new Phaser.Rectangle(0, 0, 1024, 620);
console.log(bounds);
var customBounds;

function create() {  
	//system
	game.world.setBounds(0, 0, 1024, 720);
	game.physics.startSystem(Phaser.Physics.P2JS);
	game.physics.p2.gravity.y   = 500;
	game.physics.p2.restitution = 0.8;

	// Custom bounds	
    customBounds = { left: null, right: null, top: null, bottom: null };
    createPreviewBounds(bounds.x, bounds.y, bounds.width, bounds.height);

    // Keys
	cursors    = game.input.keyboard.createCursorKeys();
	fireButton = game.input.keyboard.addKey(Phaser.Keyboard.Z);

	//player
	player                         = game.add.sprite(game.world.centerX, game.world.height - 128 , 'reimu');
	player.physicsBodyType         = Phaser.Physics.P2JS;
	player.scale.set(0.3, 0.3); //Must be before physics 
	player.anchor.setTo(0.5, 0.5);
	game.physics.p2.enable([ player ], true);

	player.body.collideWorldBounds = true;
	player.body.allowRotation      = false;	
	player.body.allowGravity       = false;
	player.body.kinematic          = true;

	player.animations.add('idle');

	//player bullets
	bullets                 = game.add.group();
	bullets.enableBody      = true;
	bullets.physicsBodyType = Phaser.Physics.P2JS;
    bullets.setAll('STATIC', true); //not working
	bullets.createMultiple(15, 'bullet');
	bullets.setAll('anchor.x', 0.5);
	bullets.setAll('outOfBoundsKill', true);
	bullets.setAll('checkWorldBounds', true); //wat
	bullets.setAll('scale.x', 1.5);
	bullets.setAll('angularForce', 0);

    //Ball
	ball = game.add.sprite(32, 32, 'ball');
	ball.physicsBodyType    = Phaser.Physics.P2JS;
	ball.scale.set(0.3, 0.3); //must be before physics
	game.physics.p2.enable([ ball ], true);
	ball.body.allowRotation = true;	
	ball.body.allowGravity  = true;
 	ball.anchor.setTo(0.5, 0.5);
	ball.body.setCircle(35);
	ball.body.x = 32;
	ball.body.y = 300;
	ball.body.velocity.x = 200;
	ball.body.velocity.y = -300;

    // debugs
	game.debug.body(ball);
	game.debug.bodyInfo(ball, 32, 32);
}

function createPreviewBounds(x, y, w, h) {

    var sim = game.physics.p2;

    //  If you want to use your own collision group then set it here and un-comment the lines below
    var mask = sim.boundsCollisionGroup.mask;

    customBounds.left = new p2.Body({ mass: 0, position: [ sim.pxmi(x), sim.pxmi(y) ], angle: 1.5707963267948966 });
    customBounds.left.addShape(new p2.Plane());
    // customBounds.left.shapes[0].collisionGroup = mask;

    customBounds.right = new p2.Body({ mass: 0, position: [ sim.pxmi(x + w), sim.pxmi(y) ], angle: -1.5707963267948966 });
    customBounds.right.addShape(new p2.Plane());
    // customBounds.right.shapes[0].collisionGroup = mask;

    customBounds.top = new p2.Body({ mass: 0, position: [ sim.pxmi(x), sim.pxmi(y) ], angle: -3.141592653589793 });
    customBounds.top.addShape(new p2.Plane());
    // customBounds.top.shapes[0].collisionGroup = mask;

    customBounds.bottom = new p2.Body({ mass: 0, position: [ sim.pxmi(x), sim.pxmi(y + h) ] });
    customBounds.bottom.addShape(new p2.Plane());
    // customBounds.bottom.shapes[0].collisionGroup = mask;

    sim.world.addBody(customBounds.left);
    sim.world.addBody(customBounds.right);
    sim.world.addBody(customBounds.top);
    sim.world.addBody(customBounds.bottom);
}
