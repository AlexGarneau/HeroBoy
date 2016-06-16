using UnityEngine;
using System.Collections;

public class EnemyDeathBehaviour : EnemyAbstractBehaviour
{
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.onAnimationState (AbstractEnemyControl.ANIM_DEATH_END);
	}
}
