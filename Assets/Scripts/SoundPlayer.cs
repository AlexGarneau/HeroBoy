using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

    AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(audio.isPlaying == false)
        {
        audio.Play();
        }
	}
}
