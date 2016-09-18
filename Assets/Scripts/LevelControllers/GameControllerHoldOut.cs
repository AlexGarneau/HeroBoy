using UnityEngine;
using System.Collections;

public class GameControllerHoldOut : MonoBehaviour {

    public int nextLevel;

    public float holdOutTimerMax;
    float holdOutTimer;
    float holdOutTimerReset = 0f;

    public GameObject clockHand;
    Vector3 clockHandPos1;
    Vector3 clockHandPos2;

    // Use this for initialization
    void Start () {
        clockHandPos1 = new Vector3(0, 0, 360);
        clockHandPos2 = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (holdOutTimer >= holdOutTimerMax)
        {
            Application.LoadLevel(nextLevel);
        }
        if (holdOutTimer < holdOutTimerMax)
        {
            holdOutTimer += Time.deltaTime;
        }

        clockHand.transform.eulerAngles = (holdOutTimer / holdOutTimerMax) * (clockHandPos2 - clockHandPos1) + clockHandPos1;
    }
}
