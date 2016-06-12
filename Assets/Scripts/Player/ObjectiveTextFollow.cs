using UnityEngine;
using System.Collections;

public class ObjectiveTextFollow : AbstractClass
{
	public GameObject player;

	void FixedUpdate ()
	{
		base.Update ();
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - 0.3f, player.transform.position.z - 5);
	}

}
