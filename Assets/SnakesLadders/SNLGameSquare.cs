using UnityEngine;
using System.Collections;

public class SNLGameSquare {

    public int spaceNumber = 0;
    public int targetSpaceNumber = 0;
    public Vector3 position;

    public enum SNLSquareState {
        normal,
        ladder,
        snake,
        goal
    }
    public SNLSquareState state = 0;

	// Use this for initialization
	public SNLGameSquare (int index, Vector3 pos, int target = -1) {
        spaceNumber = index;
        targetSpaceNumber = target;
        position = pos;

        if (target == -1) {
            // It's a normal, boring square.
            state = SNLSquareState.normal;
        } else if (target > index) {
            // It's a ladder.
            state = SNLSquareState.ladder;
        } else if (target < index) {
            // It's a snake.
            state = SNLSquareState.snake;
        } else if (index == 100) {
            // It's the end!
            state = SNLSquareState.goal;
        }
    }
}
