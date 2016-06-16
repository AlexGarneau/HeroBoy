using UnityEngine;

public class PlayerAttackStartBehaviour : PlayerAbstractBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Start Attack");
        player.setPlayerState(PlayerControl.PlayerStates.attacking);
    }
}


