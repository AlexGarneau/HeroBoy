using UnityEngine;
using System.Collections;

public class SuperMermaidCannon : MonoBehaviour {

    public GameObject mermBomb;
    public GameObject targetBoss;
    public GameObject player;

    public PirateParrot greenParrot;
    public PirateParrot redParrot;
    public PirateParrot greyParrot;

    float reloadTimer;
    float reloadTimerMax = 30f;
    float reloadTimerReset = 0f;

    public GameObject clockHand;
    Vector3 clockHandPos1;
    Vector3 clockHandPos2;

    Animator _anim;

    public int bossState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _anim = GetComponent<Animator>();

        reloadTimer = reloadTimerReset;
        bossState = 0;

        clockHandPos1 = new Vector3(0, 0, 360);
        clockHandPos2 = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if(reloadTimer >= reloadTimerMax)
        {
            reloadTimer = reloadTimerMax;
        }
        if(reloadTimer < reloadTimerMax)
        {
            reloadTimer += Time.deltaTime;
        }

        _anim.SetFloat("Timer", reloadTimer);

        clockHand.transform.eulerAngles = (reloadTimer / reloadTimerMax) * (clockHandPos2 - clockHandPos1) + clockHandPos1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "AllyHazard")
        {
            if(reloadTimer >= reloadTimerMax)
            {
                Instantiate(mermBomb, transform.position, transform.rotation);
                reloadTimer = reloadTimerReset;
                bossState += 1;
                clockHand.transform.eulerAngles = clockHandPos1;
                StartCoroutine(LaunchParrot());
            }
        }
    }

    IEnumerator LaunchParrot()
    {
        yield return new WaitForSeconds(3);
        switch (bossState)
        {
            case 1:
                greenParrot.flyAtTarget(player);
                break;
            case 2:
                redParrot.flyAtTarget(player);
                break;
            case 3:
                greyParrot.flyAtTarget(player);
                break;
            default:
                break;
        }
    }
}
