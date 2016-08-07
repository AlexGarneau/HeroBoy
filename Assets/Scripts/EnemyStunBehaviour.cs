using UnityEngine;
using System.Collections;

public class EnemyStunBehaviour : EnemyAbstractBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.onAnimationState(AbstractEnemyControl.ANIM_STUN_START);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.onAnimationState(AbstractEnemyControl.ANIM_STUN_END);
    }
}
