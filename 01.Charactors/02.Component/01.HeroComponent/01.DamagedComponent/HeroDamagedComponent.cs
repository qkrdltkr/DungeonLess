using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDamagedComponent : HeroAIComponent {
    private Transform _attackCol;
    public Transform _HeroHpBar { get; set; }
    public Transform _HeroHpBarBack { get; set; }
    private Vector3 _direction;
    private Rigidbody _monsterRigidbody;
    private DamageInfo _damageInfo = new DamageInfo();
    private bool _isDamaged;
    private bool _isDead;
    private bool _isHpbarShow = false;

    private float _hpBarShowTime = 0;
    private float monsterScale;
    private float tempScale;
    public override void Clear() { }
    public override void SetTarget(Transform target) {
        _target = target;     
    }
    public override void Initialize(Transform origin, Condition condition) {
        _hero = origin;
        _hitBox = _hero.Find("HitBox");
        _heroStats = _hitBox.GetComponent<Hero>().GetStats();

        _monsterRigidbody = _hero.GetComponent<Rigidbody>();

        // Hp 초기화
        _HeroHpBar = _hitBox.GetChild(0);
        _HeroHpBarBack = _hitBox.GetChild(1);
        _HeroHpBar.gameObject.SetActive(false);
        _HeroHpBarBack.gameObject.SetActive(false);

        _player = OperationData.Player;
       
        _condition = condition;
    }
    public override void Run(float dt) {
        if (_isDamaged) {
            CalculateHeatPoint();
            KnockBack();
            _isDamaged = false;
        }
        if (_isDead) {
            _HeroHpBar.gameObject.SetActive(false);
            _HeroHpBarBack.gameObject.SetActive(false);
            _isHpbarShow = false;
        }
        if (_isHpbarShow)
            SetHpBar(dt);
    }

    private void CalculateHeatPoint() {
        _heroStats.HeatPoint -= _damageInfo.Damage;

        if (_heroStats.HeatPoint <= 0.1f) Dead();// 피가 0이되면 사망
        _HeroHpBar.gameObject.SetActive(true);
        _HeroHpBarBack.gameObject.SetActive(true);

        if (tempScale == 0) tempScale = _HeroHpBar.transform.localScale.x;
        monsterScale = tempScale *
        _heroStats.HeatPoint / _heroStats.MaxHeatPoint;

        _isHpbarShow = true;
        _hpBarShowTime = 0f;
    }

    public override void DoWork(Transform tr) {
        _attackCol = tr;
        SetDamageInfo();
        _isDamaged = true;
    }
    private void SetDamageInfo() {
        if (_attackCol.CompareTag("Monster")) {
            _damageInfo.Damage = _attackCol.GetComponentInChildren<Character>().GetStats().Damage;
            _damageInfo.PushPower = _attackCol.GetComponentInChildren<Character>().GetStats().PushPower;
        }
        else if (_attackCol.CompareTag("MonsterBullet") || (_attackCol.CompareTag("Bullet"))){
            _damageInfo.Set(_attackCol.GetComponentInChildren<IBullet>().GetBulletInfo());
        }
        else if (_attackCol.CompareTag("Trap")) {
            _damageInfo.Damage = _attackCol.GetComponent<Trap>().TrapStats.Damage;
        }
    }

    private void Dead() {
        _isDead = true;
        if (_hero.transform.parent != null)//test용
            _hero.transform.parent.parent.GetComponent<Room>().SubtractionMonsterNumber();
        _condition.SetCondition(Condition.kDead);
        _hero.transform.GetChild(2).GetComponent<Hero>().Dead();

    }
    private void KnockBack() {
        if (_damageInfo.PushPower == 0) return;
        _direction = (_hero.transform.position - _attackCol.transform.position).normalized;
        _monsterRigidbody.AddForce(_direction * _damageInfo.PushPower);
    }
    private void SetHpBar(float dt) {
        _hpBarShowTime += dt;
        if (_hpBarShowTime > 3f) {
            _isHpbarShow = false;
            _HeroHpBar.gameObject.SetActive(false);
            _HeroHpBarBack.gameObject.SetActive(false);
            _hpBarShowTime = 0;
        }
        if (_isHpbarShow) {
            _HeroHpBar.transform.localScale = new Vector3(monsterScale, _HeroHpBar.localScale.y,
                _HeroHpBar.localScale.z);
            _HeroHpBar.gameObject.SetActive(true);
            _HeroHpBarBack.gameObject.SetActive(true);
        }
    }
}
