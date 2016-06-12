using UnityEngine;
using System.Collections;

public class CutsceneTimerEnterBullies : MonoBehaviour
{
	float timer = 0f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// Skip the cutscene.
			Application.LoadLevel (10);
		}
		timer += Time.deltaTime;
		if (timer > 46f) {
			Application.LoadLevel (10);
		}
	}
}
