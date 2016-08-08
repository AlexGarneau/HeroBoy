using UnityEngine;
using System.Collections;

public class AbstractBullet : AbstractClass
{
	private static WWW www;
	private static WWW explosionWWW;
	
	public Vector2 direction = Vector2.right;
	public float velocity = 1;
	public int damage = 1;
	public int knockback = 0;
	public float life = 10;
	public float explosionTime = 1f;
	public AudioClip explosionClip;
	protected AudioSource source;

	// Vars for obtaining bullet sprite.
	//public SpriteRenderer renderer;

	// Explosion. For everything that wants it.
	protected bool isExploding = false;

	public virtual void Start ()
	{
		source = gameObject.GetComponent<AudioSource> ();
	}

	public virtual void Update ()
	{
        base.Update();
        /*
		if (isExploding) {
			if (life > explosionTime * 2 / 3) {
				life -= Time.deltaTime;
				if (life <= explosionTime * 2 / 3) {
					AbstractAssetController.applyImage (renderer, AbstractAssetController.EXPLOSION_A_2);
				}
			} else if (life > explosionTime / 3) {
				life -= Time.deltaTime;
				if (life <= explosionTime / 3) {
					AbstractAssetController.applyImage (renderer, AbstractAssetController.EXPLOSION_A_3);
				}
			} else if (life > 0) {
				life -= Time.deltaTime;
				if (life <= 0) {
					destroy ();
				}
			}
		}
		*/
    }

	public virtual void destroy ()
	{
		//Texture2D.DestroyImmediate (renderer.sprite.texture, true);
		Destroy (this.gameObject);
	}

	public virtual void OnTriggerEnter2D (Collider2D collider)
	{
		AbstractClass ac = collider.GetComponent<AbstractClass> ();
		if (ac) {
			ac.damage (damage, AbstractDamageCollider.DamageType.light, knockback);
			destroy ();
		}
	}

	public virtual void explode ()
	{
		life = explosionTime;
		isExploding = true;

		// TODO: Play Explosion Animation

		//AbstractAssetController.applyImage (renderer, AbstractAssetController.EXPLOSION_A_1);
		//BoxCollider2D box = renderer.GetComponent<BoxCollider2D> ();
		//box.size = new Vector2 (box.size.x, box.size.y / 3);
	}
}


