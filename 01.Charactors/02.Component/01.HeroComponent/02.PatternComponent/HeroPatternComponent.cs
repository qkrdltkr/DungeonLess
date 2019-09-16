using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroPatternComponent : HeroAIComponent {
    protected Vector3 _direction;
    protected bool _isTracing;
    protected float _dis;
    protected List<Transform> _monsters = new List<Transform>();
    public override void Clear() {
        _isHeroCanAttack = false;
        _target = null;
    }
    protected void Trace(float dt, bool direction) {
        //추적
        _dis = Vector3.Distance(_hero.GetChild(2).position, _target.position);
        if (_dis <1f) {
            //대상과의 거리가 좁혀지면 idle상태로 변경
            if (_condition.currentCondition.Equals(Condition.kIdle) ||
                _condition.currentCondition.Equals(Condition.kAttack)) return;
            else {
                _condition.SetCondition(Condition.kIdle);
                return;
            }
        }else {
            if (_condition.currentCondition.Equals(Condition.kAttack)) return;
            _condition.SetCondition(Condition.kRun);

            _direction = Utility.LookAt(_target, _hero);

            _hero.transform.position += new Vector3(
                _direction.x * _heroStats.MoveSpeed * dt * 3.0f,
                _direction.y * _heroStats.MoveSpeed * dt * 3.0f,
                0.0f
                );
        }
      
    }
    public abstract void Init();
}
