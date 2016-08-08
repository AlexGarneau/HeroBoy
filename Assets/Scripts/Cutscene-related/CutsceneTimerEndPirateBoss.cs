using UnityEngine;
using System.Collections;

public class CutsceneTimerEndPirateBoss : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Skip the cutscene.
            Application.LoadLevel(26);
        }
    }

    public void CutsceneNextLevel()
    {
        Application.LoadLevel(26);
    }
}
