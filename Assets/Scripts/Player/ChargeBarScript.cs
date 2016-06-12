using UnityEngine;
using System.Collections;

public class ChargeBarScript : MonoBehaviour
{

	public int chargePercentage = 0;
	Animator _anim;

	void Start ()
	{
		_anim = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (chargePercentage > 100) {
			chargePercentage = 100;
		}
		if (chargePercentage < 0) {
			chargePercentage = 0;
		}
		_anim.SetInteger ("Charge", chargePercentage);
	}

	public void IncreaseChargePercentage (int charge)
	{
		Debug.Log ("Increase Charge!");
		chargePercentage += charge;
	}
}
