using UnityEngine;
using System.Collections;

public class CutsceneTimerDCOutro : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Skip the cutscene.
            Application.LoadLevel(148);
        }
    }

    public void CutsceneNextLevel()
    {
        Application.LoadLevel(148);
    }
}
