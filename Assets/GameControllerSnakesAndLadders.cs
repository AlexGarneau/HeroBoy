using UnityEngine;
using System.Collections;

public class GameControllerSnakesAndLadders : MonoBehaviour {

    public DisplayDieValue die;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public int nextLevel = 1;

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
