using UnityEngine;
using System.Collections;

public class PlayerHitstunAnimBehaviour : PlayerAbstractBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("BallinBro");
        player.setPlayerState(PlayerControl.PlayerStates.mobile);
    }
}
