using UnityEngine;
using System.Collections;

public class RageMeter : MonoBehaviour {

    public SpriteRenderer outline;
    public SpriteRenderer chunk1;
    public SpriteRenderer chunk2;
    public SpriteRenderer chunk3;

    protected float rageLevel = 0;
    protected float rageLevelMax = 0;

    // Use this for initialization
    void Start () {
        // Start everything invisible.
        outline.material.color = new Color(1, 1, 1, 0);
        chunk1.material.color = new Color(1, 1, 1, 0);
        chunk2.material.color = new Color(1, 1, 1, 0);
        chunk3.material.color = new Color(1, 1, 1, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (rageLevel <= 0)
        {
            outline.material.color = new Color(1, 1, 1, outline.material.color.a - Time.deltaTime);
        }
	}

    public void setDirection (bool facingLeft)
    {
        if (facingLeft) {
            this.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void setRageLevelMax (float value)
    {
        rageLevelMax = value;
        updateMeter();
    }

    public void addRageLevel (float value)
    {
        if (rageLevel <= 0 && rageLevel + value > 0)
        {
            // Immediately show the full outline.
            outline.material.color = new Color(1, 1, 1, 1);
        }
        rageLevel += value;
        updateMeter();
    }

    public void subtractRageLevel (float value)
    {
        if (rageLevel > 0)
        {
            // Only subtract if there is rage to remove.
            rageLevel -= value;
            updateMeter();
        }
    }

    void updateMeter ()
    {
        float ratio = rageLevel / rageLevelMax;
        Debug.Log("Rage Meter Ratio: " + ratio);
        if (ratio > .66f) {
            // Above 2/3rd. Opacity on chunk #3.
            chunk3.material.color = new Color(1, 1, 1, (ratio - .66f) / .33f);
            chunk2.material.color = new Color(1, 1, 1, 1);
            chunk1.material.color = new Color(1, 1, 1, 1);
        } else if (ratio > .33f) {
            // Above 1/3rd. Opacity on chunk #2.
            chunk3.material.color = new Color(1, 1, 1, 0);
            chunk2.material.color = new Color(1, 1, 1, (ratio - .33f) / .33f);
            chunk1.material.color = new Color(1, 1, 1, 1);
        } else {
            // Below 1/3rd. Opacity on chunk #1.
            chunk3.material.color = new Color(1, 1, 1, 0);
            chunk2.material.color = new Color(1, 1, 1, 0);
            chunk1.material.color = new Color(1, 1, 1, ratio / .33f);
        }
    }
}
