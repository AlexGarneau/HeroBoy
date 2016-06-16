using UnityEngine;
using System.Collections;

public class EnemyRangeCollider : MonoBehaviour {

    public bool inRangeLeft;
    public bool inRangeRight;
    public bool somethingInRange;
    Vector3 _pos;

    // Use this for initialization
    void Start() {
        _pos = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if(inRangeLeft == false && inRangeRight == false)
        {
            somethingInRange = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        AbstractEnemyControl enemy = other.GetComponent<AbstractEnemyControl>();
        if (enemy && !somethingInRange)
        {
            if (other.transform.position.x > _pos.x) {
                inRangeRight = true;
            }
            if (other.transform.position.x < _pos.x) {
                inRangeLeft = true;
            }
            somethingInRange = true;
            enemy.inMeleeRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        AbstractEnemyControl enemy = other.GetComponent<AbstractEnemyControl>();
        if (enemy && enemy.inMeleeRange)
        {
            if (other.transform.position.x > _pos.x)
            {
                inRangeRight = false;
            }
            if (other.transform.position.x < _pos.x)
            {
                inRangeLeft = false;
            }
            somethingInRange = false;
            enemy.inMeleeRange = false;
        }
    }
}
