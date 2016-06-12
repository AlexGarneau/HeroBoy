using UnityEngine;
using System.Collections;

public class CutsceneTimerEnterPirates: MonoBehaviour
{
	float timer = 0f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// Skip the cutscene.
			Application.LoadLevel (12);
		}
		timer += Time.deltaTime;
		if (timer > 66f) {
			Application.LoadLevel (12);
		}
	}
}
