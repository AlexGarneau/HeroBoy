using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
	public bool pauseGame = false;
	public GameObject pauseScreen;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetButtonDown("X360Start")) {
            // Pressed escape. Toggle pause.
            Pause(!pauseGame);
		}
		
		if (pauseGame && Input.GetKeyDown (KeyCode.Q)) {
            // Quit out of game in pause menu.
			pauseGame = false;
            Time.timeScale = 1;
            Resources.UnloadUnusedAssets();
            if (GlobalControl.instance != null) {
                GlobalControl.instance.resetPlayerStats();
            }
            Application.LoadLevel (1);
		}
	}

    void Pause (bool value)
    {
        if (value)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        } else
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
        pauseGame = value;
    }
}
