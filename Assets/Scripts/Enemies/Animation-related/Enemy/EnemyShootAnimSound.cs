﻿using UnityEngine;
using System.Collections;

public class EnemyShootAnimSound : StateMachineBehaviour
{

	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        AbstractClass c = animator.GetComponent<AbstractClass>();
        if (c == null)
        {
            c = animator.transform.parent.GetComponent<AbstractClass>();
        }

        c.playSound (AbstractClass.sfx.kick, false);
	}
}
