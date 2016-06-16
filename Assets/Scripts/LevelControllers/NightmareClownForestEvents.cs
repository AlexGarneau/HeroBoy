using UnityEngine;
using System.Collections;

public class NightmareClownForestStates : MonoBehaviour {

    int progress = 0;

    public enum ClownForestStates
    {
        enterOne,
        enterTwo,
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
    public ClownForestStates forestState;

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

    void setState(ClownForestStates newState)
    {
        switch (newState)
        {
            case ClownForestStates.enterOne:

                break;
            case ClownForestStates.enterTwo:

                break;
            case ClownForestStates.farEnd1:

                break;
            case ClownForestStates.farEnd2:

                break;
            case ClownForestStates.farEnd3:

                break;
            case ClownForestStates.backToCentre:

                break;
            case ClownForestStates.rightSide:

                break;
            case ClownForestStates.centreAgain:

                break;
            case ClownForestStates.leftCentre:

                break;
            case ClownForestStates.upRight:

                break;
            case ClownForestStates.down:

                break;
            case ClownForestStates.upTrick:

                break;
            case ClownForestStates.downAgain:

                break;
            case ClownForestStates.upToTent:

                break;
        }
    }

    public void onPlayerTrigger(GameObject obj)
    {
        Debug.Log("ENTERED TRIGGER: " + obj.name);
        switch (obj.name)
        {
            case "EndEnter":
                if (forestState == ClownForestStates.enterOne)
                {
                    setState(ClownForestStates.enterTwo);
                }
                break;
        }
    }
}
