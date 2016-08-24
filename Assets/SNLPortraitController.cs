using UnityEngine;
using System.Collections;

public class SNLPortraitController : MonoBehaviour {

    Animator anim;
    public SNLPlayer player;

    /*The goal of this script is to read the assigned player's position relative to the other players. If they are in the lead, the animation plays "Winning". If in last place, "Losing". If neither, "Normal".*/

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
