using UnityEngine;
using System.Collections;

public class NightmareClownForestEvents : MonoBehaviour {

    int progress = 0;

    public enum ClownForestEvents
    {
        enter,
        farEnd1,
        farEnd2,
        farEnd3,
        backToCentre,
        rightSide,
        centreAgain,
        leftCentre,
        upRight,
        down,
        upTrick,
        downAgain,
        upToTent
    }
    ;

    public enum sfx
    {

    }
    ;

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

    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void setState(ClownForestEvents newState)
    {
        switch (newState)
        {
            case ClownForestEvents.enter:

                break;
            case ClownForestEvents.farEnd1:

                break;
            case ClownForestEvents.farEnd2:

                break;
            case ClownForestEvents.farEnd3:

                break;
            case ClownForestEvents.backToCentre:

                break;
            case ClownForestEvents.rightSide:

                break;
            case ClownForestEvents.centreAgain:

                break;
            case ClownForestEvents.leftCentre:

                break;
            case ClownForestEvents.upRight:

                break;
            case ClownForestEvents.down:

                break;
            case ClownForestEvents.upTrick:

                break;
            case ClownForestEvents.downAgain:

                break;
            case ClownForestEvents.upToTent:

                break;
        }
    }

    public void onPlayerTrigger(GameObject obj)
    {
        Debug.Log("ENTERED TRIGGER: " + obj.name);
        switch (obj.name)
        {
            case "EndEnter":
                if (schoolState == SchoolEvents.enterOne)
                {
                    setState(SchoolEvents.enterTwo);
                }
                break;
        }
    }
}
