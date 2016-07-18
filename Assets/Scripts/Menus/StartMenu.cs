﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public GlobalControl gc;

    public GameObject titleScreen;
    public GameObject optionsScreen;
    public GameObject levelSelectScreen;

    public GameObject title;
    public Button start;
    public Button options;
    public Button quit;

    public Button resolutionOp1;
    public Button resolutionOp2;
    public Button resolutionOp3;

    public Button fpsOp1;
    public Button fpsOp2;

    public Button fsOp1;
    public Button fsOp2;

    public Button levelSelect1;
    public Button levelSelect2;
    public Button levelSelect3;
    public Button levelSelect4;

    void Start()
    {
        // TODO: Check this out for research potential: http://docs.unity3d.com/Manual/MobileOptimisation.html
        start = start.GetComponent<Button>();
        options = options.GetComponent<Button>();
        quit = quit.GetComponent<Button>();

        resolutionOp1 = resolutionOp1.GetComponent<Button>();
        resolutionOp2 = resolutionOp1.GetComponent<Button>();
        resolutionOp3 = resolutionOp1.GetComponent<Button>();

        fpsOp1 = fpsOp1.GetComponent<Button>();
        fpsOp2 = fpsOp2.GetComponent<Button>();

        fsOp1 = fsOp1.GetComponent<Button>();
        fsOp2 = fsOp2.GetComponent<Button>();

        Application.targetFrameRate = 60;

        BackToTitle();
    }

    public void StartLevel()
    {
        Time.timeScale = 1;
        // Start fresh.
        if (GlobalControl.instance != null)
        {
            GlobalControl.instance.resetPlayerStats();
        }
        Application.LoadLevel(7);
    }

    public void ShowLevelSelect()
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

	public void ShowOptions ()
	{
		titleScreen.SetActive (false);
		optionsScreen.SetActive (true);
        levelSelectScreen.SetActive(false);
    }

	public void BackToTitle ()
	{
		titleScreen.SetActive (true);
		optionsScreen.SetActive (false);
        levelSelectScreen.SetActive(false);
    }

	public void SetResolution800x600 ()
	{
		Screen.SetResolution (800, 600, Screen.fullScreen);
	}

	public void SetResolution1280x720 ()
	{
		Screen.SetResolution (1280, 720, Screen.fullScreen);
	}

	public void SetResolution1920x1080 ()
	{
		Screen.SetResolution (1920, 1080, Screen.fullScreen);
	}

	public void FullscreenOn ()
	{
		Screen.fullScreen = true;
	}

	public void FullscreenOff ()
	{
		Screen.fullScreen = false;
	}

	public void FPS30 ()
	{
		Application.targetFrameRate = 30;
	}

	public void FPS60 ()
	{
		Application.targetFrameRate = 60;
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}
}
