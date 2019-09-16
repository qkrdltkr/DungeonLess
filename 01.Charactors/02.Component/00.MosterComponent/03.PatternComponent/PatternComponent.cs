using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RayCastDirection { NONPASS = -1, RIGHT ,RIGHT_TOP ,RIGHT_DOWN,
                                LEFT,LEFT_TOP,LEFT_DOWN ,UP, DOWN }

public class PatternComponent : MonsterAIComponent {

    // ~~~~ Pattern Component들의 부모
    public override void Clear() {
        Debug.Log("클리어!");
        _isTracing = false;
        _isRunaway = false;
        _target = null;
        _isArrived = true;
        _condition.SetCondition(Condition.kIdle);
    }
    public override void SetTarget(Transform target) { }
    public override void Initialize(Transform origin, Condition condition) { }

    protected Vector3 _direction;
    protected Vector3 _randomdirection;
    protected Vector3 _originPos;

    protected GameObject _randomLocation;
    protected GameObject _randomPos;

    protected Transform _attackSensor;

    protected Quaternion _location;
    protected RaycastHit _hit;


    protected bool _isRunaway;
    protected bool _isTracing;
    protected bool _runawayStart;
    protected bool _isArrived = true;
    protected bool isArrivedPatrolPos;

    protected float _randomDistance;
    protected float _runawayTime;
    protected float _accelaration;
    protected float _dis;
    protected float _velocity;
    protected float CorrectionValue = 2.0f;
    protected float PatrolDistance = 5.0f;
    protected float CorrectionRandomValue = 2.0f;
    protected int _nextIdx;
    protected int laymask;

    #region 도망
    protected void Runaway(float dt) {
        MonsterFlip(_hitBox, _target.transform.position, false);
        _runawayTime += dt;
        if (_runawayTime < 3f) {
            _direction = Utility.LookAt(_target, _hitBox);

            _monster.transform.position += new Vector3(
                -_direction.x * _monsterStats.MoveSpeed * dt * CorrectionValue,
                -_direction.y * _monsterStats.MoveSpeed * dt * CorrectionValue,
                0.0f
                );

        }
        else { _runawayTime = 0; _runawayStart = false; }
    }
    #endregion
    #region 플립
    protected void MonsterFlip(Transform monster, Vector3 target, bool direction) {
        if (direction ? monster.position.x - target.x > 0 :
                   monster.position.x - target.x < 0 ) {
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
    #endregion
    #region 랜덤순회
    protected void PatrolRandomPlace(float dt) {
        if (_monster == null) return;
        if (!_isArrived) {
            _isArrived = true;
            SetRandomPos();
        }
        MonsterFlip(_hitBox, _randomLocation.transform.position, true);
        _direction = Utility.LookAt(_randomLocation.transform, _hitBox);
        _monster.transform.position += new Vector3(
            _direction.x * _monsterStats.MoveSpeed * dt * CorrectionValue,
            _direction.y * _monsterStats.MoveSpeed * dt * CorrectionValue,
            0.0f
            );
        _randomDistance = Vector3.Distance(_hitBox.position, _randomLocation.transform.position);
        if (_randomDistance < 0.1f) _isArrived = false;
    }

    protected void SetRandomPos() {
        _randomLocation.transform.position = _hitBox.transform.position;
        _randomLocation.transform.position += new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
    }

    protected void InitRandomLocation() {
        _randomLocation = new GameObject();
        _randomLocation.transform.position = _hitBox.transform.position;
        _randomLocation.transform.position += new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
        _randomLocation.name = "RandomLocation";
        _randomLocation.transform.parent = _monster;
    }
    #endregion
    #region 추적
    private float GetDistance(MonsterKind kind) {
        float distance = 0;
        switch (kind) {
            case MonsterKind.SLIME: distance = 0.3f; break;
            case MonsterKind.SKELETONWARRIOR: distance = 1f; break;
            case MonsterKind.KINGSLIME: distance = 1f; break;
        }
        return distance;
    }
    protected virtual void Trace(float dt, bool direction) {
        //추적
        Debug.Log(_target);
        _dis = Vector3.Distance(_monster.position, _target.position);
        if (_dis < GetDistance(_hitBox.GetComponent<Monster>()._Kind))
            {
            //대상과의 거리가 좁혀지면 idle상태로 변경
            if (_condition.currentCondition.Equals(Condition.kIdle) ||
                _condition.currentCondition.Equals(Condition.kAttack)) return;
            else {
                _condition.SetCondition(Condition.kIdle);
                return;
            }
        } else {
            if (_condition.currentCondition.Equals(Condition.kAttack)) return;
            _condition.SetCondition(Condition.kRun);

            //몬스터 플립
            MonsterFlip(_hitBox, _target.position, direction);

            _direction = Utility.LookAt(_target, _monster);

            _monster.transform.position += new Vector3(
                _direction.x * _monsterStats.MoveSpeed * dt * CorrectionValue,
                _direction.y * _monsterStats.MoveSpeed * dt * CorrectionValue,
                0.0f
                );
        }
      
    }
    #endregion
    #region 순찰
    protected void SetPatrolInit() {
        laymask = (-1) - (1 << LayerMask.NameToLayer("Wall"));
        laymask = ~laymask;
    }
    protected void Patrol(float dt ,int conditionKind) {
        _condition.SetCondition(conditionKind);
        if (_isArrived) {
            CorrectionRandomValue = PatrolDistance + Random.Range(0, CorrectionRandomValue);
            _randomdirection = Random.insideUnitCircle.normalized;
            if (!Physics.Raycast(_monster.transform.position, _randomdirection, out _hit, CorrectionRandomValue, laymask)) {
                _originPos = _monster.transform.position;
                _isArrived = false;
            }
        }
        if (!_isArrived) {
            float dis = Vector3.Distance(_originPos, _monster.transform.position);

            if (dis < CorrectionRandomValue - 1f && !isArrivedPatrolPos) { // 순찰거리까지 이동
                _monster.transform.Translate(_randomdirection * _monsterStats.MoveSpeed * dt * CorrectionValue);
                MonsterFlip(_hitBox, _originPos, false);
            }

            if (dis > CorrectionRandomValue - 1f) isArrivedPatrolPos = true; //순찰 거리까지 도달

            if (isArrivedPatrolPos) {             // 기존위치로 이동
                _randomdirection = Utility.LookAt(_originPos, _monster.position);
                _monster.transform.Translate(_randomdirection * _monsterStats.MoveSpeed * dt * CorrectionValue);
                MonsterFlip(_hitBox, _originPos, true);
                if (dis < 0.5f) {
                    _isArrived = true;
                    isArrivedPatrolPos = false;
                }
            }
        }
    }
    #endregion
}
