using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    public GlobalControl gc;
	public Button goRestart;
	public Button goQuit;

	void Start ()
	{
		goRestart = goRestart.GetComponent<Button> ();
		goQuit = goQuit.GetComponent<Button> ();
	}

	public void StartLevel ()
	{
		Application.LoadLevel (GlobalControl.instance.getLastLoaded());
        GlobalControl.instance.PlayLastPlayedMusic();
	}

	public void ExitLevel ()
	{
		Application.Quit ();
	}
}