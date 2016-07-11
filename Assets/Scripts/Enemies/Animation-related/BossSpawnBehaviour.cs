using UnityEngine;
using System.Collections;

public class BossSpawnBehaviour : BossAbstractBehaviour
{
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        if (boss != null) {
            boss.onAnimationState(AbstractBossControl.ANIM_SPAWN_END);
        }
	}
}
