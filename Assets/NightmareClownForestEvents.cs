using UnityEngine;
using System.Collections;

public class NightmareClownForestEvents : MonoBehaviour {

    int progress = 0;

    public enum ClownForestEvents
    {

    }

    public enum sfx
    {

    }

    public AudioClip[] clips;
    public AudioClip[] narrationClips;
    public AudioSource[] sources;

    public GameObject triggerColliders;
    public GameObject treePack1;

    void Start()
    {

    }

    void setState(ClownForestEvents newState)
    {
        
    }
}
