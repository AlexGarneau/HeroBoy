using UnityEngine;
using System.Collections;

public class NightmareClownForestEvents : MonoBehaviour {

    int progress = 0;
    public GameObject player;

    public GameObject wall;
    public float flickerRateBoost = 2f;
    public float flickerRateTime = 2f;
    public int flickerRate = 1;
    public float flickerMinRad = 9f;
    public float flickerMaxRad = 12f;
    public float flickerScale = 1.5f;

    public enum ClownForestStates
    {
        enter,
        farEnd1,
        farEnd2,
        farEnd3,
        backToCentre,
        rightSide,
        centreAgain,
        upRight,
        down,
        upTrick,
        downAgain,
        upToTent
    }
    ;
    public ClownForestStates forestState;

    public AudioClip[] clips;
    public AudioClip[] narrationClips;
    public AudioSource[] sources;

    public GameObject triggerColliders;

    public GameObject treePack1;
    public GameObject treePack2;
    public GameObject treePack3;
    public GameObject treePack4;
    public GameObject treePack5;
    public GameObject treePack6;

    public GameObject pathCenter;
    public GameObject pathIntro;
    public GameObject pathLoop;
    public GameObject pathTrickLoop;
    public GameObject pathTrickLoop2;
    public GameObject pathSide;
    public GameObject pathDescent;
    public GameObject pathDescentTrick;
    public GameObject pathClownEntrance;
    public GameObject circusTent;

    public GameObject clownShadow;
    public GameObject clownShadow2;
    public GameObject clownCrowd;
    public GameObject spawnPoints;

    public GameObject bound1;
    public GameObject bound2;
    public GameObject bound3;
    public GameObject bound4;
    public GameObject bound5;
    public GameObject bound6;
    public GameObject bound7;
    public GameObject bound8;

    public enum sfx
    {

    }

    public GameObject forestAmbiance;
    public GameObject eerieMusic;
    public GameObject intenseMusic;
    public GameObject pulsingLight;

    Animator _anim;
    AudioSource _audio;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        setState(ClownForestStates.enter);
    }

    IEnumerator Vanish()
    {
        if(clownShadow.activeInHierarchy == true)
        {
        yield return new WaitForSeconds(1);
        clownShadow.SetActive(false);

        }
    }

    void setState(ClownForestStates newState)
    {
        switch (newState)
        {
            case ClownForestStates.enter:
                pulsingLight.SetActive(false);
                intenseMusic.SetActive(false);
                eerieMusic.SetActive(false);
                treePack1.SetActive(true);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(false);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(true);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                clownShadow.SetActive(false);
                clownShadow2.SetActive(false);
                clownCrowd.SetActive(false);
                bound1.SetActive(true);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                spawnPoints.SetActive(false);
                break;
            case ClownForestStates.farEnd1:
                eerieMusic.SetActive(true);
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(true);
                pathCenter.SetActive(false);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(true);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                break;
            case ClownForestStates.farEnd2:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(true);
                pathCenter.SetActive(false);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(true);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                break;
            case ClownForestStates.farEnd3:
                treePack1.SetActive(true);
                treePack2.SetActive(false);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(false);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(true);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(true);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                clownShadow.SetActive(true);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(true);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                StartCoroutine(Vanish());
                break;
            case ClownForestStates.backToCentre:
                treePack1.SetActive(true);
                treePack2.SetActive(false);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(false);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(true);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(true);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(true);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                break;
            case ClownForestStates.rightSide:
                treePack1.SetActive(true);
                treePack2.SetActive(false);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(false);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(false);
                pathLoop.SetActive(true);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(true);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(true);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                break;
            case ClownForestStates.centreAgain:
                treePack1.SetActive(true);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(true);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(false);
                break;
            case ClownForestStates.upRight:
                forestAmbiance.SetActive(false);
                treePack1.SetActive(true);
                treePack2.SetActive(true);
                treePack3.SetActive(false);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(false);
                pathDescent.SetActive(true);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(true);
                bound7.SetActive(false);
                bound8.SetActive(false);
                break;
            case ClownForestStates.down:
                treePack1.SetActive(true);
                treePack2.SetActive(true);
                treePack3.SetActive(false);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(true);
                pathCenter.SetActive(false);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(true);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(true);
                bound8.SetActive(false);
                break;
            case ClownForestStates.upTrick:
                treePack1.SetActive(true);
                treePack2.SetActive(true);
                treePack3.SetActive(false);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(true);
                pathCenter.SetActive(false);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(true);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                clownCrowd.SetActive(true);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(true);
                bound8.SetActive(false);
                break;
            case ClownForestStates.upToTent:
                pulsingLight.SetActive(true);
                eerieMusic.SetActive(false);
                intenseMusic.SetActive(true);
                treePack1.SetActive(true);
                treePack2.SetActive(true);
                treePack3.SetActive(false);
                treePack4.SetActive(false);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(false);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(false);
                pathTrickLoop2.SetActive(false);
                pathSide.SetActive(false);
                pathDescent.SetActive(true);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(true);
                circusTent.SetActive(true);
                //spawnPoints.SetActive(true);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(true);

                CreateClownWall();
                break;
        }
        forestState = newState;
    }

    public void onPlayerTrigger(GameObject obj)
    {
        Debug.Log("ENTERED TRIGGER: " + obj.name);
        switch (obj.name)
        {
            case "Trigger1":
                if (forestState == ClownForestStates.enter)
                {
                    setState(ClownForestStates.farEnd1);
                }
                break;
            case "Trigger2":
                if (forestState == ClownForestStates.farEnd1)
                {
                    setState(ClownForestStates.farEnd2);
                }
                break;
            case "Trigger3":
                if (forestState == ClownForestStates.farEnd2)
                {
                    setState(ClownForestStates.farEnd3);
                }
                break;
            case "Trigger4":
                if (forestState == ClownForestStates.farEnd3)
                {
                    setState(ClownForestStates.backToCentre);
                }
                break;
            case "Trigger5":
                if (forestState == ClownForestStates.backToCentre)
                {
                    setState(ClownForestStates.rightSide);
                }
                break;
            case "Trigger6":
                if (forestState == ClownForestStates.rightSide)
                {
                    setState(ClownForestStates.centreAgain);
                }
                break;
            case "Trigger7":
                if (forestState == ClownForestStates.centreAgain)
                {
                    setState(ClownForestStates.upRight);
                }
                break;
            case "Trigger8":
                if (forestState == ClownForestStates.upRight)
                {
                    setState(ClownForestStates.down);
                }
                if (forestState == ClownForestStates.upTrick)
                {
                    setState(ClownForestStates.upToTent);
                }
                break;
            case "Trigger9":
                if (forestState == ClownForestStates.down)
                {
                    setState(ClownForestStates.upTrick);
                }
                break;
            case "EnterTentTrigger":
                //Application.LoadLevel();
                break;
        }
    }

    public void CreateClownWall () {
        // Create this big rigidbody collider to push the player upwards. Mass of clowns to disguise it.
        wall = new GameObject();
        BoxCollider2D coll = wall.AddComponent<BoxCollider2D>();
        Rigidbody2D rgd = wall.AddComponent<Rigidbody2D>();
        rgd.isKinematic = true;
        coll.size = new Vector2(25f, 5f);
        wall.transform.position = new Vector3(-8f, -70f, 0);
        float timer = 40;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Application.LoadLevel(54);
        }
    }

    public void Update () {
        if (wall != null) {
            // Animate the wall up. ... if you want a wall, anyways.
            /*
            wall.transform.Translate(0, Time.deltaTime, 0);
            StartCoroutine(flashClown());
            */

            flickerRateTime -= Time.deltaTime;
            if (flickerRateTime <= 0) {
                flickerRateTime = flickerRateBoost;
                flickerRate = Mathf.Min(6, flickerRate + 1);
                flickerMinRad = Mathf.Max(2f, flickerMinRad - .5f);
                flickerScale = Mathf.Min(3f, flickerScale + .1f);
            }

            for (int i = flickerRate-1; i >= 0; i--) {
                StartCoroutine(flickerClown());
            }
        }
    }

    protected IEnumerator flashClown () {
        GameObject shadow = Instantiate(Random.value > .5f ? clownShadow : clownShadow2) as GameObject;
        shadow.SetActive(true);
        shadow.transform.parent = wall.transform;
        shadow.transform.localPosition = new Vector3(Random.Range(-25f, 25f), Random.Range(-5f, 5f));
        yield return new WaitForSeconds(.3f);
        Destroy(shadow);
    }

    protected IEnumerator flickerClown () {
        GameObject shadow = Instantiate(Random.value > .5f ? clownShadow : clownShadow2) as GameObject;
        shadow.SetActive(true);
        shadow.transform.parent = transform;
        float angle = Random.Range(0, Mathf.PI * 2);
        
        shadow.transform.position = player.transform.position + new Vector3(Mathf.Cos(angle) * Random.Range(flickerMinRad, flickerMaxRad), Mathf.Sin(angle) * Random.Range(flickerMinRad, flickerMaxRad), 0);

        float scale = Random.Range(flickerScale/2f, flickerScale);
        shadow.transform.localScale = new Vector3(scale * (Random.value > .5f ? -1 : 1), scale);

        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);

        Destroy(shadow);
    }
}
