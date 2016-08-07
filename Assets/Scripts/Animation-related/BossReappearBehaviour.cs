using UnityEngine;
using System.Collections;

public class BossReappearBehaviour : BossAbstractBehaviour
{
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_REAPPEAR_START);
	}
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_REAPPEAR_END);
	}
	override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_REAPPEAR_UPDATE);
	}
}
