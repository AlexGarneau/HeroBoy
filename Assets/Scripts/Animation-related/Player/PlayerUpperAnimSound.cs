using UnityEngine;
using System.Collections;

public class PlayerUpperAnimSound : StateMachineBehaviour
{
	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<AbstractClass> ().playSound (AbstractClass.sfx.upper, false);
	}	
}
