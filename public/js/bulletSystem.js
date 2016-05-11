var Bullet = function (game, graphic) {
	this = game.add.sprite(key);
	this.texture.baseTexture.scaleMode = PIXI.scaleModes.NEAREST;
	this.anchor.set(0.5);

	this.checkWorldBounds = true;
	htis.outOfBoundsKill  = true;
	this.exists           = false;	
	this.tracking         = false;
	this.scaleSpeed       = 0;
}

Bullet.createBulletGroup = function (size, graphic, game) {
	var bulletsCollisionGroup = game.physics.p2.createCollisionGroup();
	for (var i = size - 1; i >= 0; i--) {
		bullet                 = new Bullet(game, graphic);
		bullet.enableBody      = true;
		bullet.physicsBodyType = Phaser.Physics.P2JS;
		bullet.body.kinematic  = true;
		bullet.checkWorldBounds = true;
		bullet.outOfBoundsKill  = true;
		bulletsCollisionGroup.add(bullet);
	}
	return bulletsCollisionGroup;
}