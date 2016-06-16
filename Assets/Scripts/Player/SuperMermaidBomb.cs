using UnityEngine;
using System.Collections;

public class SuperMermaidBomb : MonoBehaviour {

    public Animator bossAnim;
    public Animator bossHealthBar;
    public SuperMermaidCannon cannon;
    AudioSource audio;

    void Start()
    {
        bossAnim = GameObject.Find("Pirate Captain New").GetComponent<Animator>();
        bossHealthBar = GameObject.Find("Pirate Captain HealthBar").GetComponent<Animator>();
        cannon = GameObject.Find("SuperMermaidCannon").GetComponent<SuperMermaidCannon>();
        audio = GetComponent<AudioSource>();
    }

    void PlaySound()
    {
        audio.Play();
    }

    void Stagger()
    {
        bossAnim.SetTrigger("Stagger");
    }

    void DecreaseHealth()
    {
        bossHealthBar.SetTrigger("PirateIsHit");
    }

    void Exploded()
    {
        Destroy(gameObject);
    }

    void FinishLevel()
    {
        if(cannon.bossState == 4)
        {
            Application.LoadLevel(25);
        }
    }
}
