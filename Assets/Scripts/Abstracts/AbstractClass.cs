using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractClass : MonoBehaviour
{
	public const string SOUND_WALK = "walk";

	public enum sfx
	{
		walk = 0,
		light = 1,
		heavy = 2,
		kick = 3,
		upper = 4,
		noSound = 5,
		slash = 6,
		hurt = 7,
		stomp = 8,
		powerkick = 9,
		oraora = 10,
		mermaid = 11,
		omnom = 12,
        clowndrill = 13,
        clowndrillrev = 14,
        clowndrillend = 15
    }
	;

	public AudioClip[] clips;

	// Use this for initialization
	protected virtual void Start ()
	{
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.y);
	}

	public void playSound (sfx sfx, bool loop)
	{
		if ((int)sfx >= clips.Length) {
			// Calling an out-of-range clip. No good.
			return;
		}

		AudioSource audio = GetComponent<AudioSource> ();
		AudioClip clip = clips [(int)sfx];

		audio.loop = loop;
		audio.clip = clip;
		audio.Play ();
	}

    public void playSoundClip (AudioClip clip, bool loop) {
        AudioSource audio = GetComponent<AudioSource>();
        audio.loop = loop;
        audio.clip = clip;
        audio.Play();
    }

	public virtual void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{

	}

	public virtual void drop (GameObject drop)
	{
		GameObject newDrop = Instantiate (drop, transform.position, transform.rotation) as GameObject;
		newDrop.transform.parent = transform.parent;
	}

	public virtual void randomdrop (GameObject drop)
	{
		int dropRaffle = Random.Range (1, 100);
        if (dropRaffle >= 70 && drop != null) {
			GameObject newDrop = Instantiate (drop, transform.position, transform.rotation) as GameObject;
			newDrop.transform.parent = transform.parent;
		}
	}
}
