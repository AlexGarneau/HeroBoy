using UnityEngine;
using System.Collections;

public class PlayerClownDrillBehaviour : PlayerAbstractBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.setPlayerState(PlayerControl.PlayerStates.clownDrill);
    }
}
