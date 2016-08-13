using UnityEngine;
using System.Collections;

public class GameControllerSnakesAndLadders : MonoBehaviour {

    public DisplayDieValue die;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public int nextLevel = 1;

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
	    
	}

    void RollDice(int dieValue)
    {
        dieValue = die.currentValue;
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
        yield return new WaitForSeconds(1.5f);
        RollDice(die.currentValue);
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
