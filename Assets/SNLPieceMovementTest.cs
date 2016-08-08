using UnityEngine;
using System.Collections;

public class SNLPieceMovementTest : MonoBehaviour {

    public int placeOnBoard;
    float spaceToMove = 2.37f;

	// Use this for initialization
	void Start () {
        placeOnBoard = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            placeOnBoard += 1;
            if(placeOnBoard < 11)
            {
                transform.position = transform.position += new Vector3(spaceToMove, 0, 0);
            }
            if (placeOnBoard == 11)
            {
                transform.position = transform.position += new Vector3(0, 0, spaceToMove);
            }
            if (placeOnBoard > 11 && placeOnBoard < 21)
            {
                transform.position = transform.position += new Vector3(-spaceToMove, 0, 0);
            }
            if (placeOnBoard == 21)
            {
                transform.position = transform.position += new Vector3(0, 0, spaceToMove);
            }
            if (placeOnBoard > 21 && placeOnBoard < 31)
            {
                transform.position = transform.position += new Vector3(spaceToMove, 0, 0);
            }
        }
	}
}
