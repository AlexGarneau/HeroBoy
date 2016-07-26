using UnityEngine;
using System.Collections;

public class TriggerNextLevel : MonoBehaviour {

    GameObject player;

    void start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
