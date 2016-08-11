using UnityEngine;
using System.Collections;

public class DieLaunchTest : MonoBehaviour {

    public float torque = 10.0f;
    public float forceAmount = 10.0f;
    public ForceMode forceMode;
    public bool rolling = false;
    public float timer;
    float timerReset = 0;
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

	void Update () {
        if (Input.GetButtonDown("InputA"))
        {
            if(rolling == false)
            {
                rolling = true;
                transform.position += new Vector3 (0, 10f, 0);
                rBody.AddForce(Random.onUnitSphere * forceAmount, forceMode);
                rBody.AddTorque(Random.onUnitSphere * torque, forceMode);
            }
        }
        diceTimer();
	}

    void diceTimer()
    {
        if(rolling == true && timer <= 7f)
        {
            timer += Time.deltaTime;
            if (timer >= 7f)
            {
                rolling = false;
                timer = timerReset;
            }
        }
    }
}
