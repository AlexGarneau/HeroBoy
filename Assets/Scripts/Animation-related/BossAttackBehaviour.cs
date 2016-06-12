using UnityEngine;

public class BossAttackBehaviour : BossAbstractBehaviour
{
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_ATTACK_START);
	}
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_ATTACK_END);
	}
	override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_ATTACK_UPDATE);
	}
}


