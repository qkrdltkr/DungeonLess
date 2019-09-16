using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroAIComponent : IComponent {

    protected enum AnimationDirections { NONPASS = -1, D}
    //Hero AI 관련 컴포넌트들의 부모
    protected bool _isHeroCanAttack;
    protected PlayerController _playerController;
    protected Character.Stats _heroStats;
    protected Character.Stats _playerStats;
    protected Character.Stats _monsterStats;
    protected Character.Stats _targetStats;
    protected Animator _SwordAnimator;
    protected Condition _condition;
    protected Transform _hero;
    protected Transform _hitBox;
    protected Transform _player;
    protected Transform _monster;
    protected Transform _target;
    public abstract void Clear();
    public virtual void Run(float dt) { }
    public virtual void DoWork(Transform tr = null) { }
    public abstract void SetTarget(Transform target);
    public abstract void Initialize(Transform origin, Condition condition);

}
