using UnityEngine;
using System.Collections;

public class EnemyHitBehaviour : EnemyAbstractBehaviour
{

	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.onAnimationState (AbstractEnemyControl.ANIM_INJURED_END);
	}
}
