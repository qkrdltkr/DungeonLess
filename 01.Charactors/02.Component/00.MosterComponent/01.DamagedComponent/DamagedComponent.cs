using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedComponent : MonsterAIComponent{

    private Transform _attackCol;
  
    private Vector3 _direction;
    private Rigidbody _monsterRigidbody;
    private DamageInfo _damageInfo = new DamageInfo();
    private bool _isDamaged;
    private bool _isDead;
    private bool _isHpbarShow = false;

    private float _hpBarShowTime = 0;
    private float monsterScale;
    private float tempScale;

    public Transform _MonsterHpBar { get; set; }
    public Transform _MonsterHpBarBack { get; set; }
    public override void Clear(){}
    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();

        _monsterRigidbody = _monster.GetComponent<Rigidbody>();

        // Hp 초기화
        _MonsterHpBar = _monster.Find("HitBox").GetChild(0);
        _MonsterHpBarBack = _monster.Find("HitBox").GetChild(1);
        _MonsterHpBar.gameObject.SetActive(false);
        _MonsterHpBarBack.gameObject.SetActive(false);

        _player = OperationData.Player;

        _condition = condition;
    }
    public override void Run(float dt) {
        if (_isDamaged) {
            CalculateHeatPoint();
            KnockBack();
            _isDamaged = false;
        }
        if (_isDead){
            _MonsterHpBar.gameObject.SetActive(false);
            _MonsterHpBarBack.gameObject.SetActive(false);
            _isHpbarShow = false;
        }
        if (_isHpbarShow)
        SetHpBar(dt);
    }

    private void CalculateHeatPoint() {
         _monsterStats.HeatPoint -= _damageInfo.Damage;

        if (_monsterStats.HeatPoint <= 0.1f) Dead(); // 피가 0이되면 사망
        _MonsterHpBar.gameObject.SetActive(true);
        _MonsterHpBarBack.gameObject.SetActive(true);

        if (tempScale == 0) tempScale = _MonsterHpBar.transform.localScale.x;
        monsterScale = tempScale *
       _monsterStats.HeatPoint / _monsterStats.MaxHeatPoint;

        _isHpbarShow = true;
        _hpBarShowTime = 0f;
    }
    public override void DoWork(Transform tr) {
        _isDamaged = true;
        _attackCol = tr;
        SetDamageInfo();
    }

    private void SetDamageInfo() {
        if (_attackCol.CompareTag("Hero")) {
            _damageInfo.Damage = _attackCol.GetComponentInChildren<Character>().GetStats().Damage;
            _damageInfo.PushPower = _attackCol.GetComponentInChildren<Character>().GetStats().PushPower;
        }
        else if (_attackCol.CompareTag("Bullet")) {
            _damageInfo.Set(_attackCol.GetComponentInChildren<IBullet>().GetBulletInfo());
        }
        else if (_attackCol.CompareTag("Trap")) {
            _damageInfo.Damage = _attackCol.GetComponent<Trap>().TrapStats.Damage;
        }
    }

    private void Dead() {
        _isDead = true;
        if (_monster.transform.parent != null)//test용
        _monster.transform.parent.parent.GetComponent<Room>().SubtractionMonsterNumber();
        _condition.SetCondition(Condition.kDead);
        _monster.Find("HitBox").GetComponent<Monster>().Dead();
    }
    private void KnockBack() {
        if (_damageInfo.PushPower == 0) return;
        _direction = (_monster.transform.position - _attackCol.transform.position).normalized;
        _monsterRigidbody.AddForce(_direction * _damageInfo.PushPower);
    }
    private void SetHpBar(float dt) {       
        _hpBarShowTime += dt;
        if (_hpBarShowTime > 3f){
            _isHpbarShow = false;
            _MonsterHpBar.gameObject.SetActive(false);
            _MonsterHpBarBack.gameObject.SetActive(false);
            _hpBarShowTime = 0;
        }
        if (_isHpbarShow) {
            _MonsterHpBar.transform.localScale = new Vector3(monsterScale, _MonsterHpBar.localScale.y,
                _MonsterHpBar.localScale.z);
            _MonsterHpBar.gameObject.SetActive(true);
            _MonsterHpBarBack.gameObject.SetActive(true);
        }
    }
}