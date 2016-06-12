using UnityEngine;
using System.Collections;

public class CutsceneTimerOpening : MonoBehaviour
{
	float timer = 0f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
            // Skip the cutscene.
			Application.LoadLevel (8);
		}
		timer += Time.deltaTime;
		if (timer > 25f) {
            Application.LoadLevel (8);
		}
	}
}
