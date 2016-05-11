var Bullet = function (game, key) {
	Phaser.sprite.call(this, game, 0, 0, key);
	this.texture.baseTexture.scaleMode = PIXI.scaleModes.NEAREST;
	this.anchor.set(0.5);

	this.checkWorldBounds = true;
	htis.outOfBoundsKill  = true;
	this.exists           = false;	
	this.tracking         = false;
	this.scaleSpeed       = 0;
}