using UnityEngine;
using System.Collections;

public class GameControllerSnakesAndLadders : MonoBehaviour {

    protected static float TIME_TO_ONE_SQUARE = .5f;

    public DieLaunchTest dieRoller;
    public DisplayDieValue die;
    public Rigidbody dieRB;

    public SNLPlayer player1;
    public SNLPlayer player2;
    public SNLPlayer player3;
    public SNLPlayer player4;

    public int nextLevel = 1;
    protected bool isDieRolling = false;
    protected Vector3 targetSpace;
    protected float squareMoveTime = 0;

    SNLGameSquare[] Spaces = new SNLGameSquare[]
    {
        new SNLGameSquare(1, new Vector3(-9, 9.67f, -11)),
        new SNLGameSquare(2, new Vector3(-6.63f, 9.67f, -11), 37),
        new SNLGameSquare(3, new Vector3(-4.26f, 9.67f, -11)),
        new SNLGameSquare(4, new Vector3(-1.89f, 9.67f, -11), 14),
        new SNLGameSquare(5, new Vector3(0.48f, 9.67f, -11)),
        new SNLGameSquare(6, new Vector3(2.85f, 9.67f, -11)),
        new SNLGameSquare(7, new Vector3(5.22f, 9.67f, -11)),
        new SNLGameSquare(8, new Vector3(7.59f, 9.67f, -11)),
        new SNLGameSquare(9, new Vector3(9.96f, 9.67f, -11), 31),
        new SNLGameSquare(10, new Vector3(12.33f, 9.67f, -11)),
        new SNLGameSquare(11, new Vector3(12.33f, 9.67f, -8.63f)),
        new SNLGameSquare(12, new Vector3(9.96f, 9.67f, -8.63f)),
        new SNLGameSquare(13, new Vector3(7.59f, 9.67f, -8.63f)),
        new SNLGameSquare(14, new Vector3(5.22f, 9.67f, -8.63f)),
        new SNLGameSquare(15, new Vector3(2.85f, 9.67f, -8.63f)),
        new SNLGameSquare(16, new Vector3(0.48f, 9.67f, -8.63f)),
        new SNLGameSquare(17, new Vector3(-1.89f, 9.67f, -8.63f), 7),
        new SNLGameSquare(18, new Vector3(-4.26f, 9.67f, -8.63f)),
        new SNLGameSquare(19, new Vector3(-6.63f, 9.67f, -8.63f)),
        new SNLGameSquare(20, new Vector3(-9, 9.67f, -8.63f)),
        new SNLGameSquare(21, new Vector3(-9, 9.67f, -6.26f), 42),
        new SNLGameSquare(22, new Vector3(-6.63f, 9.67f, -6.26f)),
        new SNLGameSquare(23, new Vector3(-4.26f, 9.67f, -6.26f)),
        new SNLGameSquare(24, new Vector3(-1.89f, 9.67f, -6.26f)),
        new SNLGameSquare(25, new Vector3(0.48f, 9.67f, -6.26f)),
        new SNLGameSquare(26, new Vector3(2.85f, 9.67f, -6.26f)),
        new SNLGameSquare(27, new Vector3(5.22f, 9.67f, -6.26f)),
        new SNLGameSquare(28, new Vector3(7.59f, 9.67f, -6.26f), 84),
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
        new SNLGameSquare(51, new Vector3(12.33f, 9.67f, 0.85f), 67),
        new SNLGameSquare(52, new Vector3(9.96f, 9.67f, 0.85f)),
        new SNLGameSquare(53, new Vector3(7.59f, 9.67f, 0.85f)),
        new SNLGameSquare(54, new Vector3(5.22f, 9.67f, 0.85f), 34),
        new SNLGameSquare(55, new Vector3(2.85f, 9.67f, 0.85f)),
        new SNLGameSquare(56, new Vector3(0.48f, 9.67f, 0.85f)),
        new SNLGameSquare(57, new Vector3(-1.89f, 9.67f, 0.85f)),
        new SNLGameSquare(58, new Vector3(-4.26f, 9.67f, 0.85f)),
        new SNLGameSquare(59, new Vector3(-6.63f, 9.67f, 0.85f)),
        new SNLGameSquare(60, new Vector3(-9, 9.67f, 0.85f)),
        new SNLGameSquare(61, new Vector3(-9, 9.67f, 3.22f)),
        new SNLGameSquare(62, new Vector3(-6.63f, 9.67f, 3.22f)),
        new SNLGameSquare(63, new Vector3(-4.26f, 9.67f, 3.22f), 18),
        new SNLGameSquare(64, new Vector3(-1.89f, 9.67f, 3.22f), 60),
        new SNLGameSquare(65, new Vector3(0.48f, 9.67f, 3.22f)),
        new SNLGameSquare(66, new Vector3(2.85f, 9.67f, 3.22f)),
        new SNLGameSquare(67, new Vector3(5.22f, 9.67f, 3.22f)),
        new SNLGameSquare(68, new Vector3(7.59f, 9.67f, 3.22f)),
        new SNLGameSquare(69, new Vector3(9.96f, 9.67f, 3.22f)),
        new SNLGameSquare(70, new Vector3(12.33f, 9.67f, 3.22f)),
        new SNLGameSquare(71, new Vector3(12.33f, 9.67f, 5.59f), 91),
        new SNLGameSquare(72, new Vector3(9.96f, 9.67f, 5.59f)),
        new SNLGameSquare(73, new Vector3(7.59f, 9.67f, 5.59f)),
        new SNLGameSquare(74, new Vector3(5.22f, 9.67f, 5.59f)),
        new SNLGameSquare(75, new Vector3(2.85f, 9.67f, 5.59f)),
        new SNLGameSquare(76, new Vector3(0.48f, 9.67f, 5.59f)),
        new SNLGameSquare(77, new Vector3(-1.89f, 9.67f, 5.59f)),
        new SNLGameSquare(78, new Vector3(-4.26f, 9.67f, 5.59f)),
        new SNLGameSquare(79, new Vector3(-6.63f, 9.67f, 5.59f)),
        new SNLGameSquare(80, new Vector3(-9, 9.67f, 5.59f), 82),
        new SNLGameSquare(81, new Vector3(-9, 9.67f, 7.96f)),
        new SNLGameSquare(82, new Vector3(-6.63f, 9.67f, 7.96f)),
        new SNLGameSquare(83, new Vector3(-4.26f, 9.67f, 7.96f)),
        new SNLGameSquare(84, new Vector3(-1.89f, 9.67f, 7.96f)),
        new SNLGameSquare(85, new Vector3(0.48f, 9.67f, 7.96f)),
        new SNLGameSquare(86, new Vector3(2.85f, 9.67f, 7.96f)),
        new SNLGameSquare(87, new Vector3(5.22f, 9.67f, 7.96f), 36),
        new SNLGameSquare(88, new Vector3(7.59f, 9.67f, 7.96f)),
        new SNLGameSquare(89, new Vector3(9.96f, 9.67f, 7.96f)),
        new SNLGameSquare(90, new Vector3(12.33f, 9.67f, 7.96f)),
        new SNLGameSquare(91, new Vector3(12.33f, 9.67f, 10.33f)),
        new SNLGameSquare(92, new Vector3(9.96f, 9.67f, 10.33f), 73),
        new SNLGameSquare(93, new Vector3(7.59f, 9.67f, 10.33f)),
        new SNLGameSquare(94, new Vector3(5.22f, 9.67f, 10.33f)),
        new SNLGameSquare(95, new Vector3(2.85f, 9.67f, 10.33f)),
        new SNLGameSquare(96, new Vector3(0.48f, 9.67f, 10.33f), 75),
        new SNLGameSquare(97, new Vector3(-1.89f, 9.67f, 10.33f)),
        new SNLGameSquare(98, new Vector3(-4.26f, 9.67f, 10.33f), 79),
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
        player1.playerNumber = 1;
        player2.playerNumber = 2;
        player3.playerNumber = 3;
        player4.playerNumber = 4;
        player1.currentSpace = Spaces[0];
        player2.currentSpace = Spaces[0];
        player3.currentSpace = Spaces[0];
        player4.currentSpace = Spaces[0];
    }
	
	void Update () {
	    if (currentPlayer == WhoseTurn.p1 && !isDieRolling && Input.GetButtonDown("InputA")) {
            StartCoroutine(RollDie());
        } else if (isDieRolling && dieRB.IsSleeping()) {
            // Die finished rolling.
            RollDiceComplete();
        }

        if (targetSpace != Vector3.zero) {
            squareMoveTime += Time.deltaTime;
            switch (currentPlayer) {
                case WhoseTurn.p1: player1.transform.localPosition = Vector3.Lerp(player1.transform.localPosition, targetSpace, squareMoveTime / TIME_TO_ONE_SQUARE); break;
                case WhoseTurn.p2: player2.transform.localPosition = Vector3.Lerp(player2.transform.localPosition, targetSpace, squareMoveTime / TIME_TO_ONE_SQUARE); break;
                case WhoseTurn.p3: player3.transform.localPosition = Vector3.Lerp(player3.transform.localPosition, targetSpace, squareMoveTime / TIME_TO_ONE_SQUARE); break;
                case WhoseTurn.p4: player4.transform.localPosition = Vector3.Lerp(player4.transform.localPosition, targetSpace, squareMoveTime / TIME_TO_ONE_SQUARE); break;
            }
        }
    }

    void RollDiceComplete()
    {
        isDieRolling = false;
        int dieValue = die.currentValue;

        Debug.Log("Dice Roll Complete: " + dieValue);

        // Move active player here.
        SNLPlayer player;
        switch (currentPlayer) {
            case WhoseTurn.p1: player = player1; break;
            case WhoseTurn.p2: player = player2; break;
            case WhoseTurn.p3: player = player3; break;
            case WhoseTurn.p4: player = player4; break;
            default: player = player1; break;
        }
        StartCoroutine(MovePlayer(player, dieValue));
    }

    IEnumerator MovePlayer(SNLPlayer player, int spaces) {
        // Move selected player the specified number of spaces.
        int pos = player.spaceNumber - 1 + spaces;
        int nextSpace;
        for (int i = spaces - 1; i >= 0; i--) {
            if (pos - i > 100) {
                nextSpace = 100 - (pos - i - 100);
            } else {
                nextSpace = pos - i;
            }
            targetSpace = Spaces[nextSpace].position;
            squareMoveTime = 0;
            yield return new WaitForSeconds(TIME_TO_ONE_SQUARE);
        }

        //TODO: If the spot the player landed on is a snake or ladder, move player to it.
        SNLGameSquare targetSquare = Spaces[pos];
        if (targetSquare.state == SNLGameSquare.SNLSquareState.ladder) {
            // Ladder! Do some stuff to go there!
            targetSquare = Spaces[targetSquare.targetSpaceNumber - 1];
            targetSpace = targetSquare.position;
            yield return new WaitForSeconds(TIME_TO_ONE_SQUARE * 2);
        } else if (targetSquare.state == SNLGameSquare.SNLSquareState.snake) {
            // Snake! Do some stuff to go there!
            targetSquare = Spaces[targetSquare.targetSpaceNumber - 1];
            targetSpace = targetSquare.position;
            yield return new WaitForSeconds(TIME_TO_ONE_SQUARE * 2);
        } else if (targetSquare.state == SNLGameSquare.SNLSquareState.goal) {
            // Oh, the player won the game?
            CompleteGame(player);
        }

        // Set the player's current space now.
        player.currentSpace = targetSquare;

        // Reset the target space so the next player doesn't try taking it.
        targetSpace = Vector3.zero;
        EndTurn();
    }

    void CompleteGame (SNLPlayer winningPlayer) {
        // TODO: Show a screen or something to signify winning.
        PrepareEnd();
    }
    
    void SetTurn(WhoseTurn player)
    {
        currentPlayer = player;
        switch (player)
        {
            case WhoseTurn.p1:
                // Player manually inputs die roll.
                break;
            case WhoseTurn.p2:
                StartCoroutine(RollDie());
                break;
            case WhoseTurn.p3:
                StartCoroutine(RollDie());
                break;
            case WhoseTurn.p4:
                StartCoroutine(RollDie());
                break;
            case WhoseTurn.end:
                StartCoroutine(PrepareEnd());
                break;
        }
    }

    IEnumerator RollDie ()
    {
        dieRoller.roll();
        yield return new WaitForSeconds(1f);
        isDieRolling = true;
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
