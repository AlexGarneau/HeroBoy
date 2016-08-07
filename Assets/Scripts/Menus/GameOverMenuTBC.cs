using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenuTBC : MonoBehaviour
{
	public Button goRestart;
	public Button goQuit;

	void Start ()
	{
		goRestart = goRestart.GetComponent<Button> ();
		goQuit = goQuit.GetComponent<Button> ();
	}

	public void StartLevel ()
	{
		Application.LoadLevel (1);
	}

	public void ExitLevel ()
	{
		Application.Quit ();
	}
}