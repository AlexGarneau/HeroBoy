using UnityEngine;
using System.Collections;

public class SchoolToBathroomTrigger : MonoBehaviour
{

	Collider2D player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<BoxCollider2D> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other == player) {
			Application.LoadLevel (9);
		}
	}
}
