using UnityEngine;
using System.Collections;

public class NightmareSchoolEvents : MonoBehaviour
{
	bool blue9 = false;
	bool blue6 = false;
	bool green9 = false;
	bool green6 = false;
	bool cafeteria = false;

	bool green6ChairTriggered = false;
	bool volcanoTriggered = false;
	bool chairsFallingTriggered = false;
	
	public enum SchoolEvents
	{
		enterOne,
		enterTwo,
		roundOne,
		roundTwo,
		goToLibrary,
		getOutOfThere,
		darkSoulsTime
	}
	;
	public SchoolEvents schoolState;

	public GameObject triggerColliders;
	public GameObject[] doors;
	public GameObject[] blackboards;
	public GameObject[] cafeteriaMenus;
	public GameObject[] postersAndMap;
	public GameObject[] bookShelves;
	public GameObject green9Chairs;
	public GameObject volcano;
	public GameObject green6Chair;
	public GameObject blue6Chair;
	public GameObject blue9Chair;
	public GameObject green9Clock;
	public GameObject frontDoors;
	public GameObject emblem;
    public GameObject doorPoundObject;

	public GameObject coverScreen;

	public Animator objective;

	public enum sfx
	{
		Bamf = 0,
		ChairFalling = 1,
		ChairsFallingOC = 2,
		ClockWhir = 3,
		DoorBreak = 4,
		DoorPound = 5,
		DoorSlam = 6,
		Volcano = 7,
		SchoolAC = 8,
		EerieLoop = 9,
		NightmareLoop = 10
	}
	;
	public AudioClip[] clips;
	public AudioClip[] narrationClips;
	public AudioSource[] sources;

	public enum narration
	{
		FirstClassroom = 0,
		FirstLibrary = 1,
		SearchAllA = 2,
		SearchAllB = 3,
		SecondLibrary = 4,
		ShouldNotBeHere = 5,
		Run = 6
	}
	;

	public SpawnZombie[] spawns;

	void Start ()
	{
		playSound (sfx.SchoolAC, true);
        doorPoundObject.SetActive(false);
        setState (SchoolEvents.enterOne);
	}

	void setState (SchoolEvents newState)
	{
		int i;
		switch (newState) {
		case SchoolEvents.enterOne:
			doors [1].SetActive (false);
			playNarration (narration.FirstClassroom);
			postersAndMap [0].SetActive (false);
			postersAndMap [1].SetActive (false);
			postersAndMap [2].SetActive (false);
			break;
		case SchoolEvents.enterTwo:
			//objective.SetTrigger ("AfterClassroom");
			playNarration (narration.FirstLibrary);
			doors [5].SetActive (false);
			break;
		case SchoolEvents.roundOne:
			blue9Chair.transform.localScale = new Vector3 (-1.4f, 1.4f, 1);
			green9Clock.GetComponent <Animator> ().SetTrigger ("Creepy");
			//objective.SetTrigger ("RoundOne");
			playNarration (narration.SearchAllA);

			doors [0].SetActive (false);
			doors [1].SetActive (false);
			doors [2].SetActive (false);
			doors [3].SetActive (false);
			doors [4].SetActive (false);
			doors [5].SetActive (false);
			break;
		case SchoolEvents.roundTwo:
			//objective.SetTrigger ("RoundTwo");
			StartCoroutine (screenFlicker ());
			playNarration (narration.SearchAllB);
			blue9 = false;
			blue6 = false;
			green9 = false;
			green6 = false;
			cafeteria = false;

			for (i = blackboards.Length - 1; i >= 0; i--) {
				blackboards [i].SetActive (true);
			}
			for (i = postersAndMap.Length - 1; i >= 0; i--) {
				postersAndMap [i].SetActive (true);
			}
			for (i = cafeteriaMenus.Length - 1; i >= 0; i--) {
				cafeteriaMenus [i].SetActive (false);
			}
			green9Chairs.GetComponent <Animator> ().SetTrigger ("Creepy");

			stopSound (sfx.SchoolAC);
			playSound (sfx.EerieLoop, true);
			
			doors [0].SetActive (false);
			doors [1].SetActive (false);
			doors [2].SetActive (false);
			doors [3].SetActive (false);
			doors [4].SetActive (false);
			doors [5].SetActive (true);
			break;
		case SchoolEvents.goToLibrary:
			playNarration (narration.SecondLibrary);
			blue9 = false;
			blue6 = false;
			green9 = false;
			green6 = false;
			cafeteria = false;

			playSound (sfx.DoorSlam, false);

			doors [0].SetActive (true);
			doors [1].SetActive (true);
			doors [2].SetActive (true);
			doors [3].SetActive (true);
			doors [4].SetActive (true);
			doors [5].SetActive (false);
			break;
		case SchoolEvents.getOutOfThere:
			//objective.SetTrigger ("GetOut");
			playNarration (narration.ShouldNotBeHere);
			for (i = bookShelves.Length - 1; i >= 0; i--) {
				bookShelves [i].GetComponent <Animator> ().SetTrigger ("Creepy");
			}

			stopSound (sfx.EerieLoop);
			playSound (sfx.Bamf, false);
            //playSound(sfx.DoorPound, true);
                doorPoundObject.SetActive(true);

			doors [0].SetActive (true);
			doors [1].SetActive (true);
			doors [2].SetActive (true);
			doors [3].SetActive (true);
			doors [4].SetActive (true);
			break;
		case SchoolEvents.darkSoulsTime:
			//objective.SetTrigger ("Run");
			emblem.GetComponent <Animator> ().SetTrigger ("CreepyLogo");

			for (i = spawns.Length - 1; i >= 0; i--) {
				spawns [i].autoSpawn = true;
				spawns [i].spawnDelay = Random.Range (2f, 8f);
			}

                //stopSound (sfx.DoorPound);
                Destroy(doorPoundObject);
            playSound (sfx.DoorBreak, false);
			playSound (sfx.NightmareLoop, true);

			doors [0].SetActive (true);
			doors [1].SetActive (true);
			doors [2].SetActive (true);
			doors [3].SetActive (true);
			doors [4].SetActive (true);
			doors [5].SetActive (true);
			frontDoors.SetActive (false);
			break;
		}
		schoolState = newState;
	}

	public void onPlayerTrigger (GameObject obj)
	{
		Debug.Log ("ENTERED TRIGGER: " + obj.name);
		switch (obj.name) {
		case "EndEnterOne":
			if (schoolState == SchoolEvents.enterOne) {
				setState (SchoolEvents.enterTwo);
			}
			break;
		case "EndEnterTwo":
			if (schoolState == SchoolEvents.enterTwo) {
				setState (SchoolEvents.roundOne);
			}
			break;
		case "ExitLastRoom":
			stopSound (sfx.ClockWhir);
			if (schoolState == SchoolEvents.roundOne && blue9 && blue6 && green9 && green6 && cafeteria) {
				setState (SchoolEvents.roundTwo);
			}
			if (schoolState == SchoolEvents.roundTwo && blue9 && blue6 && green9 && green6 && cafeteria) {
				setState (SchoolEvents.goToLibrary);
			}
			break;
		case "BigChairFallOPTION":
			if ((schoolState == SchoolEvents.roundOne || schoolState == SchoolEvents.roundTwo) && !green6ChairTriggered) {
				green6Chair.GetComponent <Animator> ().SetTrigger ("Creepy");
				playSound (sfx.ChairFalling, false);
				green6ChairTriggered = true;
			}
			break;
		case "EndGoToLibrary":
			if (schoolState == SchoolEvents.goToLibrary) {
				setState (SchoolEvents.getOutOfThere);
			}
			break;
		case "EndGetOutOfHere":
			if (schoolState == SchoolEvents.getOutOfThere) {
				setState (SchoolEvents.darkSoulsTime);
			} else if (schoolState == SchoolEvents.roundTwo && !chairsFallingTriggered) {
				playSound (sfx.ChairsFallingOC, false);
				chairsFallingTriggered = true;
			}
			// TODO: Spawn yonder zombies.
			break;
		case "SurroundPlayer":
			break;
		case "Volcano":
			if (!volcanoTriggered) {
				volcano.GetComponent <Animator> ().SetTrigger ("Erupt");
				playSound (sfx.Volcano, false);
				volcanoTriggered = true;
			}
			break;
		case "Blue9":
			blue9 = true;
			break;
		case "Blue6":
			blue6 = true;
			if (schoolState == SchoolEvents.roundOne) {
				blue6Chair.GetComponent <Animator> ().SetTrigger ("Creepy");
			}
			break;
		case "Green9":
			green9 = true;
			if (schoolState == SchoolEvents.roundOne || schoolState == SchoolEvents.roundTwo) {
				playSound (sfx.ClockWhir, true);
			}
			break;
		case "Green6":
			green6 = true;
			break;
		case "Cafeteria":
			cafeteria = true;
			break;
		}
	}

	public IEnumerator screenFlicker ()
	{
		if (coverScreen != null) {
			float r;
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			r = Random.Range (.28f, .5f);
			coverScreen.transform.localScale = new Vector3 (r, r, 1);
			yield return new WaitForSeconds (.05f);
			coverScreen.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	public void playNarration (narration narration)
	{
		AudioSource audio = GetComponent<AudioSource> ();
		audio.loop = false;
		audio.clip = narrationClips [(int)narration];
		audio.Play ();
	}

	public void playSound (sfx sfx, bool loop)
	{
		if ((int)sfx >= clips.Length) {
			// Calling an out-of-range clip. No good.
			return;
		}
		AudioSource audio = sources [(int)sfx];
		audio.loop = loop;
		audio.clip = clips [(int)sfx];
		audio.Play ();
	}
	public void stopSound (sfx sfx)
	{
		AudioSource audio = sources [(int)sfx];
		if (audio != null) {
			audio.Stop ();
		}
	}
}