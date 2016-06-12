using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractTrigger : MonoBehaviour
{
	// Use this for initialization
	public void Start ()
	{
	}

	protected void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.GetComponent<PlayerControl> () != null) {
			SendMessageUpwards ("onPlayerTrigger", this.gameObject, SendMessageOptions.RequireReceiver);
		}
	}
}
