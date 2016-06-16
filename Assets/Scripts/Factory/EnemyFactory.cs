using UnityEngine;

public static class EnemyFactory
{
	public static Zombie createZombie ()
	{
		GameObject go = new GameObject ();

		Zombie zom = go.AddComponent<Zombie> ();
		/*
		Rigidbody2D rb = go.AddComponent<Rigidbody2D> ();
		rb.isKinematic = true;
		rb.freezeRotation = true;

		BoxCollider2D bc = go.AddComponent <BoxCollider2D> ();
		bc.size = new Vector2 (2.5, 0.7);
		bc.offset = new Vector2 (0, 0.29);

		Animator anim = go.AddComponent <Animator> ();
		*/

		return zom;
	}

	public static Pirate createPirate ()
	{
		GameObject go = new GameObject ();
		
		Pirate pir = go.AddComponent<Pirate> ();
		/*
		Rigidbody2D rb = go.AddComponent<Rigidbody2D> ();
		rb.isKinematic = true;
		rb.freezeRotation = true;

		BoxCollider2D bc = go.AddComponent <BoxCollider2D> ();
		bc.size = new Vector2 (2.5, 0.7);
		bc.offset = new Vector2 (0, 0.29);

		Animator anim = go.AddComponent <Animator> ();
		*/
		
		return pir;
	}
}


