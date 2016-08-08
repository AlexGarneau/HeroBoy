using UnityEngine;
using System.Collections;

public class CutsceneTimerMermaidCannon : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Skip the cutscene.
            Application.LoadLevel(10);
        }
    }

    public void CutsceneNextLevel()
    {
        Application.LoadLevel(10);
    }
}
