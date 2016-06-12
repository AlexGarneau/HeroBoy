using UnityEngine;
using System.Collections;

public class SplashScreenTimer : MonoBehaviour
{
	float splashtimer = 4;

	// Update is called once per frame
	void Update ()
	{
		splashtimer -= Time.deltaTime;
		if (splashtimer < 0) {
			Application.LoadLevel (1);
		}
	}
}