using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimePatternComponent : MonsterAIComponent {
    // ~~~~ Pattern Component들의 부모
    public override void SetTarget(Transform target) { }
    public override void Initialize(Transform origin, Condition condition) { }

    protected Vector3 _direction;
    protected Quaternion _location;

    protected bool _isTracing;
    protected bool _runawayStart;
    protected bool _isArrived = true;

    protected float _dis;
    protected Transform _attackSensor; 
    protected void MonsterFlip(Transform monster, Transform target, bool direction) {
        if (direction ? monster.position.x - target.position.x > 0 :
                   monster.position.x - target.position.x < 0) {
            monster.GetComponent<SpriteRenderer>().flipX = false;
            if (_attackSensor.transform.localScale.x < 0)
                _attackSensor.transform.localScale = new Vector3(
                _attackSensor.transform.localScale.x * -1f,
                _attackSensor.transform.localScale.y,
                _attackSensor.transform.localScale.z);
        }
        else {
            monster.GetComponent<SpriteRenderer>().flipX = true;
            if (_attackSensor.transform.localScale.x > 0)
                _attackSensor.transform.localScale = new Vector3(
                _attackSensor.transform.localScale.x * -1f,
                _attackSensor.transform.localScale.y,
                _attackSensor.transform.localScale.z);
        }
    }
    public override void Clear() {
        _condition.SetCondition(Condition.kIdle);
        _target = null;
    }
    protected void Trace(float dt, bool direction) {
        Debug.Log("따라간다");
        //추적
        //_dis = Vector3.Distance(_hitBox.position, _target.position);
        //if (_dis < 0.3f) {
        //    //대상과의 거리가 좁혀지면 idle상태로 변경
        //    if (_condition.currentCondition.Equals(Condition.kIdle) ||
        //        _condition.currentCondition.Equals(Condition.kAttack)) return;
        //    else {
        //        _condition.SetCondition(Condition.kIdle);
        //        return;
        //    }
        //}
        if (_condition.currentCondition.Equals(Condition.kAttack)) return;
        _condition.SetCondition(Condition.kIdle);

        //몬스터 플립
        MonsterFlip(_hitBox, _target, direction);

        _direction = Utility.LookAt(_target, _hitBox);

        _monster.transform.position += new Vector3(
            _direction.x * _monsterStats.MoveSpeed * dt * 3.0f,
            _direction.y * _monsterStats.MoveSpeed * dt * 3.0f,
            0.0f
            );
    }
}
