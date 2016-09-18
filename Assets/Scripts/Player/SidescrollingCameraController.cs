using UnityEngine;
using System.Collections;

public class SidescrollingCameraController : MonoBehaviour {

    public GameObject cam;
    public GameObject player;

	void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("I'm inside this thing bro");
            cam.transform.position = Vector3.Lerp(cam.transform.position,
                new Vector3(player.transform.position.x, -1.6f, -20), 3f * Time.deltaTime);
        }
    }
}
