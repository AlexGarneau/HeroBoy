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
        eerieLoop = 4,
        clownLoop = 5
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
                //clown forest NEW
                clownDrillUnlocked = true;
                source.Stop();
                break;
            case 19:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 20:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 21:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 22:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 23:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 24:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 25:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 26:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 27:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 28:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 29:
                //TBC
                source.Stop();
                break;
            case 30:
                //TBC
                source.Stop();
                break;
            case 31:
                //TBC
                source.Stop();
                break;
            case 32:
                //TBC
                source.Stop();
                break;
            case 33:
                //TBC
                source.Stop();
                break;
            case 34:
                //TBC
                source.Stop();
                break;
            case 35:
                //TBC
                source.Stop();
                break;
            case 36:
                //TBC
                source.Stop();
                break;
            case 37:
                //TBC
                source.Stop();
                break;
            case 38:
                //TBC
                source.Stop();
                break;
            case 39:
                //TBC
                source.Stop();
                break;
            case 40:
                //TBC
                source.Stop();
                break;
            case 41:
                //TBC
                source.Stop();
                break;
            case 42:
                //TBC
                source.Stop();
                break;
            case 43:
                //TBC
                source.Stop();
                break;
            case 44:
                //TBC
                source.Stop();
                break;
            case 45:
                //TBC
                source.Stop();
                break;
            case 46:
                //TBC
                source.Stop();
                break;
            case 47:
                //TBC
                source.Stop();
                break;
            case 48:
                //TBC
                source.Stop();
                break;
            case 49:
                //TBC
                source.Stop();
                break;
            case 50:
                //TBC
                source.Stop();
                break;
            case 51:
                //TBC
                source.Stop();
                break;
            case 52:
                //TBC
                source.Stop();
                break;
            case 53:
                //clown forest NEW
                clownDrillUnlocked = true;
                source.Stop();
                break;
            case 54:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 55:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 56:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 57:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 58:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 59:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 60:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 61:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 62:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 63:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 64:
                //clown tent NEW
                mermCannonUnlocked = true;
                clownDrillUnlocked = true;
                source.clip = musicClips[(int)clips.clownLoop];
                source.Play();
                break;
            case 65:
                //TBC
                source.Stop();
                break;
            case 66:
                //TBC
                source.Stop();
                break;
            case 67:
                //TBC
                source.Stop();
                break;
            case 68:
                //TBC
                source.Stop();
                break;
            case 69:
                //TBC
                source.Stop();
                break;
            case 70:
                //TBC
                source.Stop();
                break;
            case 71:
                //TBC
                source.Stop();
                break;
            case 72:
                //TBC
                source.Stop();
                break;
            case 73:
                //TBC
                source.Stop();
                break;
            case 74:
                //TBC
                source.Stop();
                break;
            case 75:
                //TBC
                source.Stop();
                break;
            case 76:
                //TBC
                source.Stop();
                break;
            case 77:
                //TBC
                source.Stop();
                break;
            case 78:
                //TBC
                source.Stop();
                break;
            case 79:
                //TBC
                source.Stop();
                break;
            case 80:
                //TBC
                source.Stop();
                break;
            case 81:
                //TBC
                source.Stop();
                break;
            case 82:
                //TBC
                source.Stop();
                break;
            case 83:
                //TBC
                source.Stop();
                break;
            case 84:
                //TBC
                source.Stop();
                break;
            case 85:
                //TBC
                source.Stop();
                break;
            case 86:
                //TBC
                source.Stop();
                break;
            case 87:
                //TBC
                source.Stop();
                break;
            case 88:
                //TBC
                source.Stop();
                break;
            case 89:
                //TBC
                source.Stop();
                break;
            case 90:
                //TBC
                source.Stop();
                break;
            case 91:
                //TBC
                source.Stop();
                break;
            case 92:
                //TBC
                source.Stop();
                break;
            case 93:
                //TBC
                source.Stop();
                break;
            case 94:
                //TBC
                source.Stop();
                break;
            case 95:
                //TBC
                source.Stop();
                break;
            case 96:
                //TBC
                source.Stop();
                break;
            case 97:
                //TBC
                source.Stop();
                break;
            case 98:
                //TBC
                source.Stop();
                break;
            case 99:
                //TBC
                source.Stop();
                break;
            case 100:
                //TBC
                source.Stop();
                break;
            case 101:
                //TBC
                source.Stop();
                break;
            case 102:
                //TBC
                source.Stop();
                break;
            case 103:
                //TBC
                source.Stop();
                break;
            case 104:
                //TBC
                source.Stop();
                break;
            case 105:
                //TBC
                source.Stop();
                break;
            case 106:
                //TBC
                source.Stop();
                break;
            case 107:
                //TBC
                source.Stop();
                break;
            case 108:
                //TBC
                source.Stop();
                break;
            case 109:
                //TBC
                source.Stop();
                break;
            case 110:
                //TBC
                source.Stop();
                break;
            case 111:
                //TBC
                source.Stop();
                break;
            case 112:
                //TBC
                source.Stop();
                break;
            case 113:
                //TBC
                source.Stop();
                break;
            case 114:
                //TBC
                source.Stop();
                break;
            case 115:
                //TBC
                source.Stop();
                break;
            case 116:
                //TBC
                source.Stop();
                break;
            case 117:
                //TBC
                source.Stop();
                break;
            case 118:
                //TBC
                source.Stop();
                break;
            case 119:
                //TBC
                source.Stop();
                break;
            case 120:
                //TBC
                source.Stop();
                break;
            case 121:
                //TBC
                source.Stop();
                break;
            case 122:
                //TBC
                source.Stop();
                break;
            case 123:
                //TBC
                source.Stop();
                break;
            case 124:
                //TBC
                source.Stop();
                break;
            case 125:
                //TBC
                source.Stop();
                break;
            case 126:
                //TBC
                source.Stop();
                break;
            case 127:
                //TBC
                source.Stop();
                break;
            case 128:
                //TBC
                source.Stop();
                break;
            case 129:
                //TBC
                source.Stop();
                break;
            case 130:
                //TBC
                source.Stop();
                break;
            case 131:
                //TBC
                source.Stop();
                break;
            case 132:
                //TBC
                source.Stop();
                break;
            case 133:
                //TBC
                source.Stop();
                break;
            case 134:
                //TBC
                source.Stop();
                break;
            case 135:
                //TBC
                source.Stop();
                break;
            case 136:
                //TBC
                source.Stop();
                break;
            case 137:
                //TBC
                source.Stop();
                break;
            case 138:
                //TBC
                source.Stop();
                break;
            case 139:
                //TBC
                source.Stop();
                break;
            case 140:
                //TBC
                source.Stop();
                break;
            case 141:
                //TBC
                source.Stop();
                break;
            case 142:
                //TBC
                source.Stop();
                break;
            case 143:
                //TBC
                source.Stop();
                break;
            case 144:
                //TBC
                source.Stop();
                break;
            case 145:
                //TBC
                source.Stop();
                break;
            case 146:
                //TBC
                source.Stop();
                break;
            case 147:
                //TBC
                source.Stop();
                break;
            case 148:
                source.Stop();
                break;
            case 149:
                //TBC
                source.Stop();
                break;
            case 150:
                //TBC
                source.Stop();
                break;
            case 151:
                //TBC
                source.Stop();
                break;
            case 152:
                //TBC
                source.Stop();
                break;
            case 153:
                //TBC
                source.Stop();
                break;
            case 154:
                //TBC
                source.Stop();
                break;
            case 155:
                //TBC
                source.Stop();
                break;
            case 156:
                //TBC
                source.Stop();
                break;
            case 157:
                //TBC
                source.Stop();
                break;
            case 158:
                //TBC
                source.Stop();
                break;
            case 159:
                //TBC
                source.Stop();
                break;
            case 160:
                //TBC
                source.Stop();
                break;
            case 161:
                //TBC
                source.Stop();
                break;
            case 162:
                //TBC
                source.Stop();
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
