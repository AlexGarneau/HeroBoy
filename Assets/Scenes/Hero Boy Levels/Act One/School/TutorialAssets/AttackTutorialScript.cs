using UnityEngine;
using System.Collections;

public class AttackTutorialScript : MonoBehaviour
{
	Animator _anim;

	void Start ()
	{
		_anim = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Z)) {
			_anim.SetTrigger ("ZPressed");
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			_anim.SetTrigger ("XPressed");
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			_anim.SetTrigger ("CPressed");
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			_anim.SetTrigger ("VPressed");
		}
	}
}
