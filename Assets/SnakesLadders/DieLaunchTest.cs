using UnityEngine;
using System.Collections;

public class DieLaunchTest : MonoBehaviour {

    public Vector3 initialPosition = new Vector3(1.838978f, 20f, -2.327846f);
    public float torque = 100.0f;
    public float forceAmount = 100.0f;
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
	}

    public void roll () {
        transform.localPosition = initialPosition;
        rBody.AddForce(Random.onUnitSphere * forceAmount, forceMode);
        rBody.AddTorque(Random.onUnitSphere * torque, forceMode);
    }
}
