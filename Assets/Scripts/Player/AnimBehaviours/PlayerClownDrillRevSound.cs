using UnityEngine;
using System.Collections;

public class PlayerClownDrillRevSound : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<AbstractClass>().playSound(AbstractClass.sfx.clowndrill, false);
    }
}
