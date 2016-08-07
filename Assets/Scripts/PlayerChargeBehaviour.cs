using UnityEngine;
using System.Collections;

public class PlayerChargeBehaviour : PlayerAbstractBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.setPlayerState(PlayerControl.PlayerStates.mobile);
    }
}
