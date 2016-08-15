using UnityEngine;
using System.Collections;

public class GameControllerSnakesAndLadders : MonoBehaviour {

    public DieLaunchTest dieRoller;
    public DisplayDieValue die;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    protected int player1Pos = 0;
    protected int player2Pos = 0;
    protected int player3Pos = 0;
    protected int player4Pos = 0;

    public int nextLevel = 1;
    protected bool rolling = false;
    protected Vector3 targetSpace;

    Vector3[] Spaces = new Vector3[]
    {
        new Vector3(-9, 9.67f, -11),
        new Vector3(-6.63f, 9.67f, -11),
        new Vector3(-4.26f, 9.67f, -11),
        new Vector3(-1.89f, 9.67f, -11),
        new Vector3(0.48f, 9.67f, -11),
        new Vector3(2.85f, 9.67f, -11),
        new Vector3(5.22f, 9.67f, -11),
        new Vector3(7.59f, 9.67f, -11),
        new Vector3(9.96f, 9.67f, -11),
        new Vector3(12.33f, 9.67f, -11),
        new Vector3(12.33f, 9.67f, -8.63f),
        new Vector3(9.96f, 9.67f, -8.63f),
        new Vector3(7.59f, 9.67f, -8.63f),
        new Vector3(5.22f, 9.67f, -8.63f),
        new Vector3(2.85f, 9.67f, -8.63f),
        new Vector3(0.48f, 9.67f, -8.63f),
        new Vector3(-1.89f, 9.67f, -8.63f),
        new Vector3(-4.26f, 9.67f, -8.63f),
        new Vector3(-6.63f, 9.67f, -8.63f),
        new Vector3(-9, 9.67f, -8.63f),
        new Vector3(-9, 9.67f, -6.26f),
        new Vector3(-6.63f, 9.67f, -6.26f),
        new Vector3(-4.26f, 9.67f, -6.26f),
        new Vector3(-1.89f, 9.67f, -6.26f),
        new Vector3(0.48f, 9.67f, -6.26f),
        new Vector3(2.85f, 9.67f, -6.26f),
        new Vector3(5.22f, 9.67f, -6.26f),
        new Vector3(7.59f, 9.67f, -6.26f),
        new Vector3(9.96f, 9.67f, -6.26f),
        new Vector3(12.33f, 9.67f, -6.26f),
        new Vector3(12.33f, 9.67f, -3.89f),
        new Vector3(9.96f, 9.67f, -3.89f),
        new Vector3(7.59f, 9.67f, -3.89f),
        new Vector3(5.22f, 9.67f, -3.89f),
        new Vector3(2.85f, 9.67f, -3.89f),
        new Vector3(0.48f, 9.67f, -3.89f),
        new Vector3(-1.89f, 9.67f, -3.89f),
        new Vector3(-4.26f, 9.67f, -3.89f),
        new Vector3(-6.63f, 9.67f, -3.89f),
        new Vector3(-9, 9.67f, -3.89f),
        new Vector3(-9, 9.67f, -1.52f),
        new Vector3(-6.63f, 9.67f, -1.52f),
        new Vector3(-4.26f, 9.67f, -1.52f),
        new Vector3(-1.89f, 9.67f, -1.52f),
        new Vector3(0.48f, 9.67f, -1.52f),
        new Vector3(2.85f, 9.67f, -1.52f),
        new Vector3(5.22f, 9.67f, -1.52f),
        new Vector3(7.59f, 9.67f, -1.52f),
        new Vector3(9.96f, 9.67f, -1.52f),
        new Vector3(12.33f, 9.67f, -1.52f),
        new Vector3(12.33f, 9.67f, 0.85f),
        new Vector3(9.96f, 9.67f, 0.85f),
        new Vector3(7.59f, 9.67f, 0.85f),
        new Vector3(5.22f, 9.67f, 0.85f),
        new Vector3(2.85f, 9.67f, 0.85f),
        new Vector3(0.48f, 9.67f, 0.85f),
        new Vector3(-1.89f, 9.67f, 0.85f),
        new Vector3(-4.26f, 9.67f, 0.85f),
        new Vector3(-6.63f, 9.67f, 0.85f),
        new Vector3(-9, 9.67f, 0.85f),
        new Vector3(-9, 9.67f, 3.22f),
        new Vector3(-6.63f, 9.67f, 3.22f),
        new Vector3(-4.26f, 9.67f, 3.22f),
        new Vector3(-1.89f, 9.67f, 3.22f),
        new Vector3(0.48f, 9.67f, 3.22f),
        new Vector3(2.85f, 9.67f, 3.22f),
        new Vector3(5.22f, 9.67f, 3.22f),
        new Vector3(7.59f, 9.67f, 3.22f),
        new Vector3(9.96f, 9.67f, 3.22f),
        new Vector3(12.33f, 9.67f, 3.22f),
        new Vector3(12.33f, 9.67f, 5.59f),
        new Vector3(9.96f, 9.67f, 5.59f),
        new Vector3(7.59f, 9.67f, 5.59f),
        new Vector3(5.22f, 9.67f, 5.59f),
        new Vector3(2.85f, 9.67f, 5.59f),
        new Vector3(0.48f, 9.67f, 5.59f),
        new Vector3(-1.89f, 9.67f, 5.59f),
        new Vector3(-4.26f, 9.67f, 5.59f),
        new Vector3(-6.63f, 9.67f, 5.59f),
        new Vector3(-9, 9.67f, 5.59f),
        new Vector3(-9, 9.67f, 7.96f),
        new Vector3(-6.63f, 9.67f, 7.96f),
        new Vector3(-4.26f, 9.67f, 7.96f),
        new Vector3(-1.89f, 9.67f, 7.96f),
        new Vector3(0.48f, 9.67f, 7.96f),
        new Vector3(2.85f, 9.67f, 7.96f),
        new Vector3(5.22f, 9.67f, 7.96f),
        new Vector3(7.59f, 9.67f, 7.96f),
        new Vector3(9.96f, 9.67f, 7.96f),
        new Vector3(12.33f, 9.67f, 7.96f),
        new Vector3(12.33f, 9.67f, 10.33f),
        new Vector3(9.96f, 9.67f, 10.33f),
        new Vector3(7.59f, 9.67f, 10.33f),
        new Vector3(5.22f, 9.67f, 10.33f),
        new Vector3(2.85f, 9.67f, 10.33f),
        new Vector3(0.48f, 9.67f, 10.33f),
        new Vector3(-1.89f, 9.67f, 10.33f),
        new Vector3(-4.26f, 9.67f, 10.33f),
        new Vector3(-6.63f, 9.67f, 10.33f),
        new Vector3(-9, 9.67f, 10.33f),
    };

    public enum WhoseTurn
    {
        p1,
        p2,
        p3,
        p4,
        end
    }
    ;
    protected WhoseTurn currentPlayer;

    void Start () {
	    
	}
	
	void Update () {
	    if (currentPlayer == WhoseTurn.p1 && !rolling && Input.GetButtonDown("InputA")) {
            StartCoroutine(PrepareRollDie());
        }

        if (targetSpace != Vector3.zero) {
            switch (currentPlayer) {
                case WhoseTurn.p1: Vector3.Lerp(player1.transform.position, targetSpace, Time.deltaTime); break;
                case WhoseTurn.p2: Vector3.Lerp(player2.transform.position, targetSpace, Time.deltaTime); break;
                case WhoseTurn.p3: Vector3.Lerp(player3.transform.position, targetSpace, Time.deltaTime); break;
                case WhoseTurn.p4: Vector3.Lerp(player4.transform.position, targetSpace, Time.deltaTime); break;
            }
        }
    }

    void RollDiceComplete(int dieValue)
    {
        rolling = false;
        dieValue = die.currentValue;

        // Move active player here.
        GameObject player;
        int pos;
        switch (currentPlayer) {
            case WhoseTurn.p1: player = player1; player1Pos += dieValue; pos = player1Pos; break;
            case WhoseTurn.p2: player = player2; player2Pos += dieValue; pos = player2Pos; break;
            case WhoseTurn.p3: player = player3; player3Pos += dieValue; pos = player3Pos; break;
            case WhoseTurn.p4: player = player4; player4Pos += dieValue; pos = player4Pos; break;
            default: player = player1; player1Pos += dieValue; pos = player1Pos; break;
        }
        StartCoroutine(MovePlayer(player, dieValue, pos));
    }

    IEnumerator MovePlayer(GameObject player, int spaces, int pos) {
        // Move selected player the specified number of spaces.
        for (int i = spaces - 1; i >= 0; i--) {
            targetSpace = Spaces[pos - i];
            yield return new WaitForSeconds(1f);
        }
        targetSpace = Vector3.zero;

        //TODO: If the spot the player landed on is a snake or ladder, move player to it.
    }
    
    void SetTurn(WhoseTurn player)
    {
        switch (player)
        {
            case WhoseTurn.p1:

                break;
            case WhoseTurn.p2:
                StartCoroutine(PrepareRollDie());
                break;
            case WhoseTurn.p3:
                StartCoroutine(PrepareRollDie());
                break;
            case WhoseTurn.p4:
                StartCoroutine(PrepareRollDie());
                break;
            case WhoseTurn.end:
                StartCoroutine(PrepareEnd());
                break;
        }
        currentPlayer = player;
    }

    IEnumerator PrepareRollDie ()
    {
        rolling = true;
        dieRoller.roll();
        yield return new WaitForSeconds(8f);
        RollDiceComplete(die.currentValue);
    }

    IEnumerator PrepareEnd ()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(nextLevel);
    }

    void EndTurn()
    {
        if(currentPlayer == WhoseTurn.p1)
        {
            SetTurn(WhoseTurn.p2);
        } else if (currentPlayer == WhoseTurn.p2)
        {
            SetTurn(WhoseTurn.p3);
        } else if (currentPlayer == WhoseTurn.p3)
        {
            SetTurn(WhoseTurn.p4);
        } else if (currentPlayer == WhoseTurn.p4)
        {
            SetTurn(WhoseTurn.p1);
        }
    }
}
