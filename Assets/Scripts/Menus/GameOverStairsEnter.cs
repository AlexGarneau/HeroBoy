﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverStairsEnter : MonoBehaviour
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
		Application.LoadLevel (15);
	}

	public void ExitLevel ()
	{
		Application.Quit ();
	}
}