using UnityEngine;

public class BulletExplodeBehaviour : StateMachineBehaviour
{
    public AbstractBullet bullet;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bullet.explosionStart();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bullet.explosionEnd();
    }
}


