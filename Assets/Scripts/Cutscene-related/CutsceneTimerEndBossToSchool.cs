using UnityEngine;
using System.Collections;

public class CutsceneTimerEndBossToSchool : MonoBehaviour
{
	float timer = 0f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// Skip the cutscene.
			Application.LoadLevel (26);
		}
		timer += Time.deltaTime;
		if (timer > 73f) {
			Application.LoadLevel (26);
		}
	}
}
