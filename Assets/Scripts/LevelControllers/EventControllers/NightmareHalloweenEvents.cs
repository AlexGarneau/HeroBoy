using UnityEngine;
using System.Collections;

public class NightmareHalloweenEvents : MonoBehaviour {

    PlayerControl _player;

    float timerActivate;
    float timerReset = 5f;

    int progress = 0;

    public enum NightmareHalloweenStates
    {
        enter,
        candy1,
        candy2,
        candy3,
        candy4,
        transform1,
        transform2,
        transform3
    }
    ;
    public NightmareHalloweenStates halloweenState;

    public enum sfx
    {

    }
    ;

    public AudioClip[] clips;
    public AudioClip[] narrationClips;
    public AudioSource[] sources;

    public GameObject triggerColliders;

    public PlayerControl player;
    public GameObject maxGhost;
    public GameObject mom;
    public GameObject roadBlock;

    public GameObject spawnPoints;

    public Animator house1;
    public Animator house2;
    public Animator house3;
    public Animator house4;
    public Animator house5;
    public Animator house6;
    public Animator house7;
    public Animator house8;
    public Animator house9;
    public Animator house10;
    public Animator house11;
    public Animator house12;
    public Animator house13;
    public Animator house14;
    public Animator house15;

    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
        setState(NightmareHalloweenStates.enter);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        if (_player.playerHealth <= 0)
        {
            ActivateScreen();
        }
    }


    void setState(NightmareHalloweenStates newState)
    {
        switch (newState)
        {
            case NightmareHalloweenStates.enter:
                spawnPoints.SetActive(false);
                mom.SetActive(true);
                mom.transform.position = new Vector2(-25, 0);
                break;
            case NightmareHalloweenStates.candy1:
                mom.transform.position = new Vector2(-20, 0);
                break;
            case NightmareHalloweenStates.candy2:
                mom.transform.position = new Vector2(-15, 0);
                break;
            case NightmareHalloweenStates.candy3:
                mom.transform.position = new Vector2(-8, 0);
                break;
            case NightmareHalloweenStates.candy4:

                break;
            case NightmareHalloweenStates.transform1:
                player.canAttack = true;
                maxGhost.SetActive(false);
                mom.SetActive(false);
                roadBlock.SetActive(false);
                house1.SetTrigger("Spook");
                house2.SetTrigger("Spook");
                house3.SetTrigger("Spook");
                house4.SetTrigger("Spook");
                house5.SetTrigger("Spook");
                break;
            case NightmareHalloweenStates.transform2:
                house6.SetTrigger("Spook");
                house7.SetTrigger("Spook");
                house8.SetTrigger("Spook");
                house9.SetTrigger("Spook");
                house10.SetTrigger("Spook");
                break;
            case NightmareHalloweenStates.transform3:
                house1.SetTrigger("SuperSpook");
                house2.SetTrigger("SuperSpook");
                house3.SetTrigger("SuperSpook");
                house4.SetTrigger("SuperSpook");
                house5.SetTrigger("SuperSpook");
                house6.SetTrigger("SuperSpook");
                house7.SetTrigger("SuperSpook");
                house8.SetTrigger("SuperSpook");
                house9.SetTrigger("SuperSpook");
                house10.SetTrigger("SuperSpook");
                house11.SetTrigger("SuperSpook");
                house12.SetTrigger("SuperSpook");
                house13.SetTrigger("SuperSpook");
                house14.SetTrigger("SuperSpook");
                house15.SetTrigger("SuperSpook");
                spawnPoints.SetActive(true);
                break;
        }
                halloweenState = newState;
    }

    public void onPlayerTrigger(GameObject obj)
    {
        Debug.Log("ENTERED TRIGGER: " + obj.name);
        switch (obj.name)
        {
            case "Candy1":
                if (halloweenState == NightmareHalloweenStates.enter)
                {
                    setState(NightmareHalloweenStates.candy1);
                }
                break;
            case "Candy2":
                if (halloweenState == NightmareHalloweenStates.candy1)
                {
                    setState(NightmareHalloweenStates.candy2);
                }
                break;
            case "Candy3":
                if (halloweenState == NightmareHalloweenStates.candy2)
                {
                    setState(NightmareHalloweenStates.candy3);
                }
                break;
            case "Candy4":
                if (halloweenState == NightmareHalloweenStates.candy3)
                {
                    setState(NightmareHalloweenStates.candy4);
                }
                break;
            case "Transform1":
                if (halloweenState != NightmareHalloweenStates.transform1 &&
                    halloweenState != NightmareHalloweenStates.transform2 &&
                    halloweenState != NightmareHalloweenStates.transform3)
                {
                    setState(NightmareHalloweenStates.transform1);
                }
                break;
            case "Transform2":
                if (halloweenState == NightmareHalloweenStates.transform1)
                {
                    setState(NightmareHalloweenStates.transform2);
                }
                break;
            case "Transform3":
                if (halloweenState != NightmareHalloweenStates.transform3)
                {
                    setState(NightmareHalloweenStates.transform3);
                }
                break;
        }
    }

    void ActivateScreen()
    {
        if (timerActivate > 0)
        {
            timerActivate -= 1 * Time.deltaTime;
            if (timerActivate <= 0)
            {
                Application.LoadLevel(29);
                timerActivate = 0;
            }
        }
    }
}
