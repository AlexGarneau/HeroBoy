using UnityEngine;
using System.Collections;

public static class BulletFactory
{
	/*
	public static AbstractBullet createBullet ()
	{
		GameObject go = new GameObject ();
		AbstractBullet bul = go.AddComponent<AbstractBullet> ();
		AudioSource source = go.AddComponent <AudioSource> ();

		SpriteRenderer renderer = addSpriteRenderer (bul);

		BoxCollider2D bc = renderer.gameObject.AddComponent <BoxCollider2D> ();
		bc.size = new Vector2 (2.5f, 0.7f);
		bc.offset = new Vector2 (0, 0.29f);
		bc.isTrigger = true;

		return bul;
	}
	
	public static PirateBullet createPirateBullet ()
	{
		GameObject go = new GameObject ();
		PirateBullet bul = go.AddComponent<PirateBullet> ();
		AudioSource source = go.AddComponent <AudioSource> ();
		
		SpriteRenderer renderer = addSpriteRenderer (bul);
		AbstractAssetController.applyImage (renderer, AbstractAssetController.BULLET_A);
		
		BoxCollider2D bc = renderer.gameObject.AddComponent <BoxCollider2D> ();
		bc.size = new Vector2 (0.2f, 0.2f);
		bc.offset = new Vector2 (0.2f, -1.5f);
		bc.isTrigger = true;

		return bul;
	}
	
	public static PirateBomb createPirateBomb ()
	{
		GameObject go = new GameObject ();
		PirateBomb bomb = go.AddComponent<PirateBomb> ();
		AudioSource source = go.AddComponent <AudioSource> ();
		
		SpriteRenderer renderer = addSpriteRenderer (bomb);
		AbstractAssetController.applyImage (renderer, AbstractAssetController.BOMB_A);
		
		BoxCollider2D bc = renderer.gameObject.AddComponent <BoxCollider2D> ();
		bc.size = new Vector2 (1.24f, 1.27f);
		bc.offset = new Vector2 (0.36f, 0.5f);
		bc.isTrigger = true;
		
		return bomb;
	}

	public static MermaidBomb createMermaidBomb ()
	{
		GameObject go = new GameObject ();
		MermaidBomb bomb = go.AddComponent<MermaidBomb> ();
		AudioSource source = go.AddComponent <AudioSource> ();

		SpriteRenderer renderer = addSpriteRenderer (bomb);
		if (Random.value >= 0.5) {
			AbstractAssetController.applyImage (renderer, AbstractAssetController.MERMAID_A);
		} else {
			AbstractAssetController.applyImage (renderer, AbstractAssetController.MERMAID_B);
		}

		BoxCollider2D bc = renderer.gameObject.AddComponent <BoxCollider2D> ();
		bc.size = new Vector2 (1.54f, 2.64f);
		bc.offset = new Vector2 (0.78f, 1.3f);
		bc.isTrigger = true;
		
		return bomb;
	}

	private static SpriteRenderer addSpriteRenderer (AbstractBullet ab)
	{
		Rigidbody2D rb = ab.gameObject.AddComponent<Rigidbody2D> ();
		rb.isKinematic = true;
		GameObject spriteContainer = new GameObject ();
		spriteContainer.name = "Sprite";
		spriteContainer.transform.parent = ab.transform;
		SpriteRenderer renderer = spriteContainer.AddComponent<SpriteRenderer> ();
		// ab.renderer = renderer;
		return renderer;
	}
	*/
}


