using UnityEngine;
using System.Collections;

public class EnemySpawnBehaviour : EnemyAbstractBehaviour
{
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        if (enemy != null)
        {
            enemy.onAnimationState(AbstractEnemyControl.ANIM_SPAWN_END);
        }
	}
}
