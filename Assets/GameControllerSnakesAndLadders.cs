using UnityEngine;
using System.Collections;

public class GameControllerSnakesAndLadders : MonoBehaviour {

    public DisplayDieValue die;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public enum WhoseTurn
    {
        p1,
        p2,
        p3,
        p4,
        end
    }
    ;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
