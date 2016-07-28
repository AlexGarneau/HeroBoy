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
	}

	public void StartLevel ()
	{
		Application.LoadLevel (1);
	}
}