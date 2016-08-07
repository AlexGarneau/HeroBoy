using UnityEngine;
using System.Collections;

public class EnemyShootBehaviour : EnemyAbstractBehaviour
{

	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.onAnimationState (AbstractEnemyControl.ANIM_PISTOL_FIRE);
	}
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.onAnimationState (AbstractEnemyControl.ANIM_ATTACK_END);
	}

}
