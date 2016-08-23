using UnityEngine;

public class SNLPlayer : MonoBehaviour {

    public SNLGameSquare currentSpace = null;
    public int playerNumber = 0;

	// Use this for initialization
	public SNLPlayer () {}

    public int spaceNumber {
        get {
            return currentSpace.spaceNumber;
        }
    }
}
