using UnityEngine;
using System.Collections;

public class TriggerNextLevel : MonoBehaviour {

    public GameObject player;
    public int nextLevel;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            Application.LoadLevel(nextLevel);
        }
    }
}
