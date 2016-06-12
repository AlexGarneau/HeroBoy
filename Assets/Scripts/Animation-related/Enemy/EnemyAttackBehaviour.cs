using UnityEngine;

public class EnemyAttackBehaviour : EnemyAbstractBehaviour
{
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.onAnimationState (AbstractEnemyControl.ANIM_ATTACK_START);
	}
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.onAnimationState (AbstractEnemyControl.ANIM_ATTACK_END);
	}
}


