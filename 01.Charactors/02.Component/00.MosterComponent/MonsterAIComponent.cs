using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAIComponent : IComponent {

    //몬스터ai관련 컴포넌트들의 부모
    protected PlayerController _playerController;
    protected Character.Stats _monsterStats;
    protected Character.Stats _playerStats;
    protected Character.Stats _targetStats;
    protected Condition _condition;
    protected Animator _SwordAnimator;
    protected Transform _monster;
    protected Transform _hitBox;
    protected Transform _player;
    protected Transform _hero;
    protected Transform _target;
    public virtual void Run(float dt) { }
    public virtual void DoWork(Transform tr = null) { }
    public abstract void SetTarget(Transform target);
    public abstract void Clear();
    public abstract void Initialize(Transform origin, Condition condition);
   
}