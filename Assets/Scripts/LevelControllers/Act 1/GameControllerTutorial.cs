using UnityEngine;
using System.Collections;

public class GameControllerTutorial: AbstractGameController
{
	float activateTimer = 0;
	float timerReset = 5f;

    protected enum TutorialState
    {
        Normal,
        TutorialDodge,
        TutorialLight,
        TutorialMedium,
        TutorialHeavy,
        TutorialRangedAttack,
        TutorialLightCombo,
        TutorialChargedAttack,
        TutorialFinale
    };

    protected TutorialState state;
    protected TutorialState nextState;
    protected float waitTimer;
    protected string targetButton = "";

    Bully[] bullies;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
        activateTimer = timerReset;

		LevelBoundary.topWidth = 7.35f - -5.4f; // Use the enemies positions during game play to get these coordinates.
		LevelBoundary.bottomWidth = 8.25f - -7.65f; // Widths are right-corner minus left corner.
		LevelBoundary.left = -7.5f; // Left is the left corner of the larger width (in this case, bottom)
		LevelBoundary.bottom = -6f; // Bottom is the lowest point in the boundary.
		LevelBoundary.height = -0.8f - -6f; // Height is top minus bottom.

        bullies = GameObject.FindObjectsOfType<Bully>();

        for (int i = bullies.Length - 1; i >= 0; i--)
        {
            bullies[i].setEnemyState(AbstractEnemyControl.EnemyStates.stand);
            bullies[i].setEnemyDamage(1);
        }

        // Bully4 is the first to move around.
        bullies[bullies.Length-1].setEnemyState(AbstractEnemyControl.EnemyStates.move);

        // Start the tutorial.
        setState(TutorialState.Normal, 2);
        nextState = TutorialState.TutorialDodge;
    }

	void Update ()
	{
		if (player.playerHealth <= 0) {
			ActivateScreen ();
		}

        if (targetButton != "" && (Input.GetButtonDown(targetButton) || Input.GetButtonDown("Dodge")))
        {
            // Player hit the button. Go normal.
            setState(TutorialState.Normal, 5);
        }

        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                waitTimer = 0;
                setState(nextState);
            }
        }
	}

    void setState (TutorialState newState, float waitTime = -1)
    {
        Time.timeScale = 0.0f;
        player.setPlayerState(PlayerControl.PlayerStates.immobile);
        targetButton = "";

        switch (newState)
        {
            case TutorialState.Normal:
                // Game plays normally.
                Time.timeScale = 1.0f;
                player.setPlayerState(PlayerControl.PlayerStates.mobile);
                player.PlayerAction();
                _anim.SetTrigger("Tutorial0");
                break;
            case TutorialState.TutorialDodge:
                _anim.SetTrigger("Tutorial1");
                player.playerHealth = 1000;
                targetButton = "Dodge";
                nextState = TutorialState.TutorialLight;
                break;
            case TutorialState.TutorialLight:
                _anim.SetTrigger("Tutorial2");
                targetButton = "InputA";
                nextState = TutorialState.TutorialMedium;
                break;
            case TutorialState.TutorialMedium:
                _anim.SetTrigger("Tutorial3");
                targetButton = "InputB";
                nextState = TutorialState.TutorialHeavy;
                break;
            case TutorialState.TutorialHeavy:
                _anim.SetTrigger("Tutorial4");
                targetButton = "InputC";
                nextState = TutorialState.TutorialRangedAttack;
                break;
            case TutorialState.TutorialRangedAttack:
                _anim.SetTrigger("Tutorial5");
                targetButton = "InputD";
                nextState = TutorialState.TutorialLightCombo;
                break;
            case TutorialState.TutorialLightCombo:
                _anim.SetTrigger("Tutorial6");
                targetButton = "InputA";
                nextState = TutorialState.TutorialChargedAttack;
                break;
            case TutorialState.TutorialChargedAttack:
                _anim.SetTrigger("Tutorial9");
                targetButton = "InputD";
                ChargeBarScript cb = GetComponentInChildren<ChargeBarScript>();
                cb.chargePercentage = 100;
                nextState = TutorialState.TutorialFinale;
                break;
            case TutorialState.TutorialFinale:
                _anim.SetTrigger("Tutorial10");
                nextState = TutorialState.Normal;
                Time.timeScale = 1.0f;
                player.setPlayerState(PlayerControl.PlayerStates.mobile);
                player.playerHealth = 100;
                for (int i = bullies.Length - 1; i >= 0; i--)
                {
                    // Last part of tutorial. Make bullies invincible.
                    bullies[i].invincible = true;
                    bullies[i].setEnemyState(AbstractEnemyControl.EnemyStates.move);
                    bullies[i].setEnemyDamage(25);
                }
                break;
        }

        if (waitTime > 0)
        {
            // Setup a wait timer to tick down and automatically call the next state.
            waitTimer = waitTime;
        }

        state = newState;
    }

	void ActivateScreen ()
	{
		if (activateTimer > 0) {
			activateTimer -= 1 * Time.deltaTime;
			if (activateTimer <= 0) {
				Application.LoadLevel (nextLevel);
				activateTimer = 0;
			}
		}
	}
}