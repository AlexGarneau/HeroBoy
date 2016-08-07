using UnityEngine;

public class PlayerAttackEndBehaviour : PlayerAbstractBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.setPlayerState(PlayerControl.PlayerStates.attacking);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.setPlayerState(PlayerControl.PlayerStates.mobile);
    }
}


