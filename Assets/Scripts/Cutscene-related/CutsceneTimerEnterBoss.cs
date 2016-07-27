using UnityEngine;
using System.Collections;

public class CutsceneTimerEnterBoss: MonoBehaviour
{
	float timer = 0f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// Skip the cutscene.
			Application.LoadLevel (15);
		}
		timer += Time.deltaTime;
		if (timer > 25f) {
			Application.LoadLevel (15);
		}
	}
}
