using UnityEngine;
using System.Collections;

public class DestructibleObject : AbstractClass {

    Animator _anim;
    public GameObject healthItem;
    public GameObject chargePart;
    public GameObject chargeFull;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AllyHazard" || other.tag == "EnemHazard")
        {
            _anim.SetTrigger("Attacked");
        }
    }

    void SpawnItem()
    {
        float itemRoulette = Random.Range(1, 10);
        Debug.Log(itemRoulette);
        if (itemRoulette <= 6)
        {
            Instantiate(healthItem, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        } else if (itemRoulette >= 7 && itemRoulette <= 9)
        {
            Instantiate(chargePart, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        } else if (itemRoulette == 10)
        {
            Instantiate(chargeFull, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
    }
}
