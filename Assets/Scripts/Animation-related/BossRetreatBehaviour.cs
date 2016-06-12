using UnityEngine;
using System.Collections;

public class BossRetreatBehaviour : BossAbstractBehaviour
{
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_RETREAT_START);
	}
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_RETREAT_END);
	}
	override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_RETREAT_UPDATE);
	}
}
