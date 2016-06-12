using UnityEngine;
using System.Collections;

public class BossDeathBehaviour : BossAbstractBehaviour
{
	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		boss.onAnimationState (AbstractBossControl.ANIM_DEATH_END);
	}
}
