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
        nightmareLoop = 3,
        eerieLoop = 4
	}
	;

	private int lastLoaded = 0;
    public int getLastLoaded()
    {
        return lastLoaded;
    }
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

        if (level != 2) {
            lastLoaded = level;
        }

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
			// Board ship
			source.clip = musicClips [(int)clips.pirateLoop];
			source.Play ();
			break;
            case 4:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 5:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 6:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 7:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 8:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 9:
			// Find cannon
			source.Stop ();
			break;
		case 10:
			// Got cannon
			source.clip = musicClips [(int)clips.pirateLoop];
            mermCannonUnlocked = true;
            source.Play ();
			break;
            case 11:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 12:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 13:
                // Board ship
                source.clip = musicClips[(int)clips.pirateLoop];
                source.Play();
                break;
            case 14:
            // Challenge Boss
            source.clip = musicClips[(int)clips.actionLoop];
            source.Stop();
            break;
		case 15:
			// Pirate boss
			source.clip = musicClips [(int)clips.pirateLoop];
			source.Play ();
			break;
		case 16:
			// Teacher
			source.Stop ();
			break;
            case 17:
                //nightmare school
                mermCannonUnlocked = false;
                source.Stop();
                break;
        case 18:
            // bandit attack
            mermCannonUnlocked = false;
            clownDrillUnlocked = false;
                source.clip = musicClips[(int)clips.nightmareLoop];
                source.Play();
            break;
            case 19:
                // bandit attack
                source.clip = musicClips[(int)clips.nightmareLoop];
                source.Play();
                break;
            case 20:
                // bandit attack
                source.clip = musicClips[(int)clips.nightmareLoop];
                source.Play();
                break;
            case 21:
                // bandit attack
                source.clip = musicClips[(int)clips.nightmareLoop];
                source.Play();
                break;
            case 22:
                // bandit attack
                source.clip = musicClips[(int)clips.nightmareLoop];
                source.Play();
                break;
            case 23:
                // clown forest
            clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.eerieLoop];
                source.Play();
            break;
            case 24:
                // clown tent
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.nightmareLoop];
                source.Play();
                break;
        default:
			break;
		}
	}

    public void PlayLastPlayedMusic()
    {
        source.Play();
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
