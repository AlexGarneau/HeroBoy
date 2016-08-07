using UnityEngine;

public class PlayerDodgeBehaviour : PlayerAbstractBehaviour
{
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        player.setPlayerState(PlayerControl.PlayerStates.dodging);
	}
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		player.setPlayerState(PlayerControl.PlayerStates.mobile);
	}
}


