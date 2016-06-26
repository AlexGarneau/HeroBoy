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
            case ClownForestStates.enter:
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
                break;
            case ClownForestStates.farEnd1:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.farEnd2:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.farEnd3:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                clownShadow.SetActive(true);
                break;
            case ClownForestStates.backToCentre:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.rightSide:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.centreAgain:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.leftCentre:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.upRight:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.down:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.upTrick:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.downAgain:
                treePack1.SetActive(false);
                treePack2.SetActive(true);
                treePack3.SetActive(true);
                treePack4.SetActive(true);
                treePack5.SetActive(true);
                treePack6.SetActive(false);
                pathCenter.SetActive(true);
                pathIntro.SetActive(true);
                pathLoop.SetActive(false);
                pathTrickLoop.SetActive(true);
                pathTrickLoop2.SetActive(true);
                pathSide.SetActive(false);
                pathDescent.SetActive(false);
                pathDescentTrick.SetActive(false);
                pathClownEntrance.SetActive(false);
                circusTent.SetActive(false);
                break;
            case ClownForestStates.upToTent:
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
                break;
        }
    }

    public void onPlayerTrigger(GameObject obj)
    {
        Debug.Log("ENTERED TRIGGER: " + obj.name);
        switch (obj.name)
        {
            case "EndEnter":
                if (forestState == ClownForestStates.enter)
                {
                    setState(ClownForestStates.enter);
                }
                break;
        }
    }
}
