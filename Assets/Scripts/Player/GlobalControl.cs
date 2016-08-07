using UnityEngine;
using System.Collections;

public class GlobalControl : MonoBehaviour
{
	public int playerHP;
	public int playerCP;

    public bool mermCannonUnlocked = false;
    public bool maceOfTritUnlocked = false;
    public bool rarLaserUnlocked = false;
    public bool clownDrillUnlocked = false;

    public AudioSource source;
	public AudioClip[] musicClips;

	protected enum clips
	{
		fightLoop = 0,
		pirateLoop = 1,
        actionLoop = 2,
	}
	;

	private int lastLoaded = 0;
	//bool showGUI = false;

	private static GlobalControl GlobalInstance;

	public static GlobalControl instance {
		get {
			if (GlobalInstance == null) {
				GlobalInstance = GameObject.FindObjectOfType<GlobalControl> ();
				if (GlobalInstance != null) {
					DontDestroyOnLoad (GlobalInstance.gameObject);
				}
			}
			return GlobalInstance;
		}
	}

	void Awake ()
	{
		if (GlobalInstance != null && GlobalInstance != this) {
			Destroy (gameObject);
			return;
		} else {
			GlobalInstance = this;
		}
		DontDestroyOnLoad (gameObject);
	}
    void Start()
    {
        if(playerHP < 100)
        {
            playerHP = 100;
        }
    }

	void OnLevelWasLoaded ()
	{
		int level = Application.loadedLevel;
		if (lastLoaded == level) {
			return;
		}
		lastLoaded = level;

		source.volume = .4f;

		switch (level) {
		case 0:
			// splash
			source.Stop ();
			break;
		case 1:
			// menu
			source.Stop ();
			break;
		case 2:
			// gameover
			source.Stop ();
			break;
		case 3:
			// gameover
			source.Stop ();
			break;
		case 4:
			// gameover
			source.Stop ();
			break;
		case 5:
			// gameover
			source.Stop ();
			break;
		case 6:
			// gameover
			source.Stop ();
			break;
		case 10:
			// Bully fight
			source.clip = musicClips [(int)clips.fightLoop];
			source.Play ();
			break;
		case 11:
			// Open pirate book
			source.Stop ();
			break;
		case 12:
			// Board ship
			source.clip = musicClips [(int)clips.pirateLoop];
			source.Play ();
			break;
        case 15:
            // Checkpoint
            source.clip = musicClips[(int)clips.pirateLoop];
            source.Play();
            break;
            case 18:
			// Find cannon
			source.Stop ();
			break;
		case 19:
			// Got cannon
			source.clip = musicClips [(int)clips.pirateLoop];
                mermCannonUnlocked = true;
                source.Play ();
			break;
        case 23:
            // Challenge Boss
            source.clip = musicClips[(int)clips.actionLoop];
            source.Stop();
            break;
		case 24:
			// Pirate boss
			source.clip = musicClips [(int)clips.pirateLoop];
			source.Play ();
			break;
		case 25:
			// Teacher
			source.Stop ();
			break;
		default:
			break;
		}
	}

	void Update ()
	{
		//DEBUG - Pressing Space Skips Scenes
		if (Input.GetKeyDown (KeyCode.P)) {
			// DEBUG: Skip the everything.
			Application.LoadLevel (Application.loadedLevel + 1);
		}

	}

    public void resetPlayerStats()
    {
        playerHP = 100;
        playerCP = 0;
    }
}
