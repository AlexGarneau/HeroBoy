using UnityEngine;
using System.Collections;

public class CutsceneTimerEnterBoss: MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// Skip the cutscene.
			Application.LoadLevel (15);
		}
	}

    public void CutsceneNextLevel()
    {
        Application.LoadLevel(15);
    }
}
