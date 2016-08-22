using UnityEngine;
using System.Collections;

public class DisplayDieValue : MonoBehaviour {

    public LayerMask dieValueColliderLayer;

    public int currentValue = 1;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity, dieValueColliderLayer))
        {
            currentValue = hit.collider.GetComponent<DieNumberGenerator>().value;
        }
        Debug.Log("Current Die Value: " + currentValue);
	}

    void OnGUI()
    {
        GUILayout.Label(currentValue.ToString());
    }
}
