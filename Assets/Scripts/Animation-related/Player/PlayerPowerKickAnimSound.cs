using UnityEngine;
using System.Collections;

public class PlayerPowerKickAnimSound : StateMachineBehaviour
{

	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<AbstractClass> ().playSound (AbstractClass.sfx.powerkick, false);
	}	
}
