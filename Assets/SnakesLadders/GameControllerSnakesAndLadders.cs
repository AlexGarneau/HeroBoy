using UnityEngine;
using System.Collections;

public class GameControllerSnakesAndLadders : MonoBehaviour {

    public DieLaunchTest dieRoller;
    public DisplayDieValue die;
    public Rigidbody dieRB;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    protected int player1Pos = 0;
    protected int player2Pos = 0;
    protected int player3Pos = 0;
    protected int player4Pos = 0;

    protected SNLGameSquare player1Square;
    protected SNLGameSquare player2Square;
    protected SNLGameSquare player3Square;
    protected SNLGameSquare player4Square;

    public int nextLevel = 1;
    protected bool isDieRolling = false;
    protected Vector3 targetSpace;

    SNLGameSquare[] Spaces = new SNLGameSquare[]
    {
        new SNLGameSquare(1, new Vector3(-9, 9.67f, -11)),
        new SNLGameSquare(2, new Vector3(-6.63f, 9.67f, -11)),
        new SNLGameSquare(3, new Vector3(-4.26f, 9.67f, -11)),
        new SNLGameSquare(4, new Vector3(-1.89f, 9.67f, -11)),
        new SNLGameSquare(5, new Vector3(0.48f, 9.67f, -11)),
        new SNLGameSquare(6, new Vector3(2.85f, 9.67f, -11)),
        new SNLGameSquare(7, new Vector3(5.22f, 9.67f, -11)),
        new SNLGameSquare(8, new Vector3(7.59f, 9.67f, -11)),
        new SNLGameSquare(9, new Vector3(9.96f, 9.67f, -11)),
        new SNLGameSquare(10, new Vector3(12.33f, 9.67f, -11)),
        new SNLGameSquare(11, new Vector3(12.33f, 9.67f, -8.63f)),
        new SNLGameSquare(12, new Vector3(9.96f, 9.67f, -8.63f)),
        new SNLGameSquare(13, new Vector3(7.59f, 9.67f, -8.63f)),
        new SNLGameSquare(14, new Vector3(5.22f, 9.67f, -8.63f)),
        new SNLGameSquare(15, new Vector3(2.85f, 9.67f, -8.63f)),
        new SNLGameSquare(16, new Vector3(0.48f, 9.67f, -8.63f)),
        new SNLGameSquare(17, new Vector3(-1.89f, 9.67f, -8.63f)),
        new SNLGameSquare(18, new Vector3(-4.26f, 9.67f, -8.63f)),
        new SNLGameSquare(19, new Vector3(-6.63f, 9.67f, -8.63f)),
        new SNLGameSquare(20, new Vector3(-9, 9.67f, -8.63f)),
        new SNLGameSquare(21, new Vector3(-9, 9.67f, -6.26f)),
        new SNLGameSquare(22, new Vector3(-6.63f, 9.67f, -6.26f)),
        new SNLGameSquare(23, new Vector3(-4.26f, 9.67f, -6.26f)),
        new SNLGameSquare(24, new Vector3(-1.89f, 9.67f, -6.26f)),
        new SNLGameSquare(25, new Vector3(0.48f, 9.67f, -6.26f)),
        new SNLGameSquare(26, new Vector3(2.85f, 9.67f, -6.26f)),
        new SNLGameSquare(27, new Vector3(5.22f, 9.67f, -6.26f)),
        new SNLGameSquare(28, new Vector3(7.59f, 9.67f, -6.26f)),
        new SNLGameSquare(29, new Vector3(9.96f, 9.67f, -6.26f)),
        new SNLGameSquare(30, new Vector3(12.33f, 9.67f, -6.26f)),
        new SNLGameSquare(31, new Vector3(12.33f, 9.67f, -3.89f)),
        new SNLGameSquare(32, new Vector3(9.96f, 9.67f, -3.89f)),
        new SNLGameSquare(33, new Vector3(7.59f, 9.67f, -3.89f)),
        new SNLGameSquare(34, new Vector3(5.22f, 9.67f, -3.89f)),
        new SNLGameSquare(35, new Vector3(2.85f, 9.67f, -3.89f)),
        new SNLGameSquare(36, new Vector3(0.48f, 9.67f, -3.89f)),
        new SNLGameSquare(37, new Vector3(-1.89f, 9.67f, -3.89f)),
        new SNLGameSquare(38, new Vector3(-4.26f, 9.67f, -3.89f)),
        new SNLGameSquare(39, new Vector3(-6.63f, 9.67f, -3.89f)),
        new SNLGameSquare(40, new Vector3(-9, 9.67f, -3.89f)),
        new SNLGameSquare(41, new Vector3(-9, 9.67f, -1.52f)),
        new SNLGameSquare(42, new Vector3(-6.63f, 9.67f, -1.52f)),
        new SNLGameSquare(43, new Vector3(-4.26f, 9.67f, -1.52f)),
        new SNLGameSquare(44, new Vector3(-1.89f, 9.67f, -1.52f)),
        new SNLGameSquare(45, new Vector3(0.48f, 9.67f, -1.52f)),
        new SNLGameSquare(46, new Vector3(2.85f, 9.67f, -1.52f)),
        new SNLGameSquare(47, new Vector3(5.22f, 9.67f, -1.52f)),
        new SNLGameSquare(48, new Vector3(7.59f, 9.67f, -1.52f)),
        new SNLGameSquare(49, new Vector3(9.96f, 9.67f, -1.52f)),
        new SNLGameSquare(50, new Vector3(12.33f, 9.67f, -1.52f)),
        new SNLGameSquare(51, new Vector3(12.33f, 9.67f, 0.85f)),
        new SNLGameSquare(52, new Vector3(9.96f, 9.67f, 0.85f)),
        new SNLGameSquare(53, new Vector3(7.59f, 9.67f, 0.85f)),
        new SNLGameSquare(54, new Vector3(5.22f, 9.67f, 0.85f)),
        new SNLGameSquare(55, new Vector3(2.85f, 9.67f, 0.85f)),
        new SNLGameSquare(56, new Vector3(0.48f, 9.67f, 0.85f)),
        new SNLGameSquare(57, new Vector3(-1.89f, 9.67f, 0.85f)),
        new SNLGameSquare(58, new Vector3(-4.26f, 9.67f, 0.85f)),
        new SNLGameSquare(59, new Vector3(-6.63f, 9.67f, 0.85f)),
        new SNLGameSquare(60, new Vector3(-9, 9.67f, 0.85f)),
        new SNLGameSquare(61, new Vector3(-9, 9.67f, 3.22f)),
        new SNLGameSquare(62, new Vector3(-6.63f, 9.67f, 3.22f)),
        new SNLGameSquare(63, new Vector3(-4.26f, 9.67f, 3.22f)),
        new SNLGameSquare(64, new Vector3(-1.89f, 9.67f, 3.22f)),
        new SNLGameSquare(65, new Vector3(0.48f, 9.67f, 3.22f)),
        new SNLGameSquare(66, new Vector3(2.85f, 9.67f, 3.22f)),
        new SNLGameSquare(67, new Vector3(5.22f, 9.67f, 3.22f)),
        new SNLGameSquare(68, new Vector3(7.59f, 9.67f, 3.22f)),
        new SNLGameSquare(69, new Vector3(9.96f, 9.67f, 3.22f)),
        new SNLGameSquare(70, new Vector3(12.33f, 9.67f, 3.22f)),
        new SNLGameSquare(71, new Vector3(12.33f, 9.67f, 5.59f)),
        new SNLGameSquare(72, new Vector3(9.96f, 9.67f, 5.59f)),
        new SNLGameSquare(73, new Vector3(7.59f, 9.67f, 5.59f)),
        new SNLGameSquare(74, new Vector3(5.22f, 9.67f, 5.59f)),
        new SNLGameSquare(75, new Vector3(2.85f, 9.67f, 5.59f)),
        new SNLGameSquare(76, new Vector3(0.48f, 9.67f, 5.59f)),
        new SNLGameSquare(77, new Vector3(-1.89f, 9.67f, 5.59f)),
        new SNLGameSquare(78, new Vector3(-4.26f, 9.67f, 5.59f)),
        new SNLGameSquare(79, new Vector3(-6.63f, 9.67f, 5.59f)),
        new SNLGameSquare(80, new Vector3(-9, 9.67f, 5.59f)),
        new SNLGameSquare(81, new Vector3(-9, 9.67f, 7.96f)),
        new SNLGameSquare(82, new Vector3(-6.63f, 9.67f, 7.96f)),
        new SNLGameSquare(83, new Vector3(-4.26f, 9.67f, 7.96f)),
        new SNLGameSquare(84, new Vector3(-1.89f, 9.67f, 7.96f)),
        new SNLGameSquare(85, new Vector3(0.48f, 9.67f, 7.96f)),
        new SNLGameSquare(86, new Vector3(2.85f, 9.67f, 7.96f)),
        new SNLGameSquare(87, new Vector3(5.22f, 9.67f, 7.96f)),
        new SNLGameSquare(88, new Vector3(7.59f, 9.67f, 7.96f)),
        new SNLGameSquare(89, new Vector3(9.96f, 9.67f, 7.96f)),
        new SNLGameSquare(90, new Vector3(12.33f, 9.67f, 7.96f)),
        new SNLGameSquare(91, new Vector3(12.33f, 9.67f, 10.33f)),
        new SNLGameSquare(92, new Vector3(9.96f, 9.67f, 10.33f)),
        new SNLGameSquare(93, new Vector3(7.59f, 9.67f, 10.33f)),
        new SNLGameSquare(94, new Vector3(5.22f, 9.67f, 10.33f)),
        new SNLGameSquare(95, new Vector3(2.85f, 9.67f, 10.33f)),
        new SNLGameSquare(96, new Vector3(0.48f, 9.67f, 10.33f)),
        new SNLGameSquare(97, new Vector3(-1.89f, 9.67f, 10.33f)),
        new SNLGameSquare(98, new Vector3(-4.26f, 9.67f, 10.33f)),
        new SNLGameSquare(99, new Vector3(-6.63f, 9.67f, 10.33f)),
        new SNLGameSquare(100, new Vector3(-9, 9.67f, 10.33f))
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
	    if (currentPlayer == WhoseTurn.p1 && !isDieRolling && Input.GetButtonDown("InputA")) {
            RollDie();
        } else if (isDieRolling && dieRB.IsSleeping()) {
            // Die finished rolling.
            RollDiceComplete();
        }

        if (targetSpace != Vector3.zero) {
            switch (currentPlayer) {
                case WhoseTurn.p1: Vector3.Lerp(player1.transform.position, targetSpace, Time.deltaTime); break;
                case WhoseTurn.p2: Vector3.Lerp(player2.transform.position, targetSpace, Time.deltaTime); break;
                case WhoseTurn.p3: Vector3.Lerp(player3.transform.position, targetSpace, Time.deltaTime); break;
                case WhoseTurn.p4: Vector3.Lerp(player4.transform.position, targetSpace, Time.deltaTime); break;
            }
        }
        
        Debug.Log("Is Sleeping: " + dieRB.IsSleeping());
    }

    void RollDiceComplete()
    {
        isDieRolling = false;
        int dieValue = die.currentValue;

        Debug.Log("Dice Roll Complete: " + dieValue);

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
            targetSpace = Spaces[pos - i].position;
            yield return new WaitForSeconds(1f);
        }
        targetSpace = Vector3.zero;

        //TODO: If the spot the player landed on is a snake or ladder, move player to it.
        SNLGameSquare targetSquare = Spaces[pos];
        if (targetSquare.state == SNLGameSquare.SNLSquareState.ladder) {
            // Ladder! Do some stuff to go there!
            targetSpace = Spaces[targetSquare.targetSpaceNumber].position;
            yield return new WaitForSeconds(3f);
        } else if (targetSquare.state == SNLGameSquare.SNLSquareState.snake) {
            // Snake! Do some stuff to go there!
            targetSpace = Spaces[targetSquare.targetSpaceNumber].position;
            yield return new WaitForSeconds(3f);
        } else if (targetSquare.state == SNLGameSquare.SNLSquareState.goal) {
            // Oh, the player won the game?
            CompleteGame(player);
        }

        EndTurn();
    }

    void CompleteGame (GameObject winningPlayer) {
        // TODO: Show a screen or something to signify winning.
        PrepareEnd();
    }
    
    void SetTurn(WhoseTurn player)
    {
        switch (player)
        {
            case WhoseTurn.p1:
                // Player manually inputs die roll.
                break;
            case WhoseTurn.p2:
                RollDie();
                break;
            case WhoseTurn.p3:
                RollDie();
                break;
            case WhoseTurn.p4:
                RollDie();
                break;
            case WhoseTurn.end:
                StartCoroutine(PrepareEnd());
                break;
        }
        currentPlayer = player;
    }

    void RollDie ()
    {
        isDieRolling = true;
        dieRoller.roll();
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
