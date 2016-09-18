using UnityEngine;
using System.Collections;

public class ShadowFlicker : MonoBehaviour {

    public GameObject[] shadows;

    /** Time delay until flicker rate increase. */
    public float flickerIncreaseTime = 2f;

    /** Flicker rate. Number of shadows that appear at a time. Increases by 1 with rate. */
    public int flickerRate = 1;
    public int flickerRateIncrease = 1;
    public int flickerRateMax = 6;

    /** Min and max radius around player for shadows to appear. Increases with rate. */
    public float flickerMinRadius = 9f;
    public float flickerMinRadiusDecrease = .5f;
    public float flickerMaxRadius = 12f;
    public float flickerMaxRadiusDecrease = .6f;

    /** Max scale of shadows. Increases with rate. */
    public float flickerMinScale = .75f;
    public float flickerMinScaleIncrease = .05f;
    public float flickerMaxScale = 1.5f;
    public float flickerMaxScaleIncrease = .1f;

    public bool isActive = true;

    public GameObject player;

    protected float flickerRateTime = 0;

    // Use this for initialization
    void Start () {
        flickerRateTime = flickerIncreaseTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            // Animate the wall up. ... if you want a wall, anyways.
            flickerRateTime -= Time.deltaTime;
            if (flickerRateTime <= 0)
            {
                flickerRateTime = flickerIncreaseTime;
                flickerRate = Mathf.Min(flickerRateMax, flickerRate + flickerRateIncrease);
                flickerMinRadius -= flickerMinRadiusDecrease;
                flickerMaxRadius -= flickerMaxRadiusDecrease;
                flickerMinScale += flickerMinScaleIncrease;
                flickerMaxScale += flickerMaxScaleIncrease;
            }

            for (float i = flickerRate - 1; i >= 0; i--)
            {
                StartCoroutine(flickerClown());
            }
        }
    }

    protected IEnumerator flickerClown()
    {
        GameObject shadow = Instantiate(shadows[Random.Range(0, shadows.Length)]) as GameObject;
        shadow.SetActive(true);
        shadow.transform.parent = transform;
        float angle = Random.Range(0, Mathf.PI * 2);

        shadow.transform.position = new Vector3(
            player.transform.position.x + Mathf.Cos(angle) * Random.Range(flickerMinRadius, flickerMaxRadius),
            player.transform.localPosition.y + Mathf.Sin(angle) * Random.Range(flickerMinRadius, flickerMaxRadius), 
            0
        );

        float scale = Random.Range(flickerMinScale, flickerMaxScale);
        shadow.transform.localScale = new Vector3(scale * (Random.value > .5f ? -1 : 1), scale);

        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);
        shadow.transform.position += new Vector3(Random.value - .5f, Random.value - .5f, 0);
        yield return new WaitForSeconds(.04f);

        Destroy(shadow);
    }
}
