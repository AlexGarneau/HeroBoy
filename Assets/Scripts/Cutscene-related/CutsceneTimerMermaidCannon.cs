﻿using UnityEngine;
using System.Collections;

public class CutsceneTimerMermaidCannon : MonoBehaviour
{
	float timer = 0f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// Skip the cutscene.
			Application.LoadLevel (19);
		}
		timer += Time.deltaTime;
		if (timer > 23f) {
			Application.LoadLevel (19);
		}
	}
}
