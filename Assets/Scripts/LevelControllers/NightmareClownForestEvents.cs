using UnityEngine;
using System.Collections;

public class NightmareClownForestEvents : MonoBehaviour {

    int progress = 0;

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
                spawnPoints.SetActive(true);
                bound1.SetActive(false);
                bound2.SetActive(false);
                bound3.SetActive(false);
                bound4.SetActive(false);
                bound5.SetActive(false);
                bound6.SetActive(false);
                bound7.SetActive(false);
                bound8.SetActive(true);
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
}
