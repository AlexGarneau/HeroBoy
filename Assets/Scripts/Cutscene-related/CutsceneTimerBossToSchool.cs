using UnityEngine;
using System.Collections;

public class CutsceneTimerBossToSchool : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Skip the cutscene.
            Application.LoadLevel(18);
        }
    }

    public void CutsceneNextLevel()
    {
        Application.LoadLevel(18);
    }
}
