using UnityEngine;
using System.Collections;

public class ObjectiveTextFollow : MonoBehaviour
{
	public GameObject player;
    public GameObject camera;

	void FixedUpdate ()
	{
		transform.position = new Vector3 (player.transform.position.x + 2.3f, player.transform.position.y - 3.3f, camera.transform.position.z + 1);
	}
}
