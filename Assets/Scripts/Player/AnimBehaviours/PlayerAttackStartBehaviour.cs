using UnityEngine;

public class PlayerAttackStartBehaviour : PlayerAbstractBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.setPlayerState(PlayerControl.PlayerStates.attacking);
    }
}


