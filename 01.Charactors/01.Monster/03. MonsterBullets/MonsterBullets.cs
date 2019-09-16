using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParabolaPrameter {
    public float _vx;
    public float _vy;
    public float _vz;
}
public class MonsterBullets : IUpdateableObject,IBullet {

    public int Kind { get; set; }
    public Transform _bullet;
    [SerializeField]
    public Character.Stats _monsterstats;
    protected Condition _condition;
    protected Vector3 _direction; protected Vector3 _startPos; protected Vector3 _destPos;
    protected GameObject _spitObj; protected GameObject[] _spitObjs;

    protected Transform _target;
    protected Transform _firePos;
    protected Transform _monster;
    protected Transform _hitBox;

    protected int _bulletNum;
    protected float _timer; protected float _range;
    protected float _delayTime = 0; protected float _distance;
    protected float _sx;  protected float _sy; protected float _sz;

    protected bool _isSpit;
    protected bool _isCalculateFinished;
    protected DamageInfo _bulletInfo = new DamageInfo();

    public DamageInfo GetBulletInfo() { return _bulletInfo; }
    protected override void Initialize() { }
    protected virtual void ExplosionBullet(int i) { }
    protected virtual void ExplosionBullet() { }
    protected virtual void InitParabola(int i, Transform target) { }
    protected virtual void InitParabola(Transform target) { }
    protected virtual void CalculateParabola(int i) { }
    protected virtual void CalculateParabola() { }
    protected virtual void CalculateRange(float dt) { }
    protected virtual void Spit(float dt) { }
    protected virtual void Init(Transform monster, Transform target, Condition condition) { }
}
