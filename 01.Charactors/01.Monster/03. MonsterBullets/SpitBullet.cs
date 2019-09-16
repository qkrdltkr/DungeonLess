using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpitBullet : MonsterBullets {

    private ParabolaPrameter _parabolaPrams;

    protected override void Initialize() {
        this.Kind = MonsterBulletKind.kMucus;
        MonsterBulletPoolManager.Instance.SetBullet(this.gameObject, this.transform.parent);
        GameObject obj = Instantiate(_bullet.gameObject, this.transform);
        obj.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void Fire(Transform monster, Transform target, Condition condition) {
        if (condition.currentCondition.Equals(Condition.kAttack)) return;
        Init(monster, target, condition);
        _condition.SetCondition(Condition.kAttack, (int)KingSlimeSkillKInds.SPIT);

        _bulletInfo.Damage = _monsterstats.Damage;
        _bulletInfo.PushPower = _monsterstats.PushPower;
        _bulletInfo.Attribute = DamageAttribute.POISON;

        StartCoroutine(DelaySpit());
    }
    private IEnumerator DelaySpit() {
        yield return YieldInstructionCache.WaitForSeconds(1.0f);       
        _isSpit = true;
        yield return YieldInstructionCache.WaitForSeconds(0.9f);
        _condition.SetCondition(Condition.kIdle);

    }
    protected override void Init(Transform monster, Transform target, Condition condition) {
        _monster = monster;
        _firePos = _monster.Find("SkillSensor").GetChild(0);
        _target = target;
        _condition = condition;
        _startPos = _firePos.position;
        _timer = 0;
        this.transform.position = _monster.transform.position;
        this.gameObject.SetActive(true);

        _monsterstats = _monster.GetChild(2).GetComponent<Monster>().GetStats();
        _range = _monsterstats.Range;
    }

    public override void OnFixedUpdate(float dt) {
        Spit(dt); //침뱉기
    }

    protected override void Spit(float dt) {
        if (_isSpit) {
            _timer += dt;
   
            if (!_isCalculateFinished) {
                InitParabola(_target); //출발지, 도착지 계싼
            }
            _isCalculateFinished = true;
            CalculateParabola(); //포물선 계산
            _spitObj.transform.position = new Vector3(_sx, _sy, _sz);
            ExplosionBullet(); // 닿으면 폭발
            CalculateRange(dt); //딜레이 계산
        }
    }

    protected override void ExplosionBullet() {
        _distance = Vector3.Distance(_spitObj.transform.position, _destPos);
        if (_distance < 0.3f)
            _spitObj.GetComponent<MonsterBulletSensor>().ExplosionBullet();
    }

    protected override void InitParabola(Transform target) {
        _destPos = new Vector3(target.position.x ,target.position.y, target.position.z);
   
        _parabolaPrams._vx = (_destPos.x - _startPos.x) / 2f;
        _parabolaPrams._vy = (_destPos.y - _startPos.y + 2 * 9.8f) / 2f;
        _parabolaPrams._vz = (_destPos.z - _startPos.z) / 2f;
     
        _spitObj = this.transform.GetChild(0).gameObject;
        _spitObj.SetActive(true);
    }
    protected override void CalculateParabola() { 
        _sx = _startPos.x + _parabolaPrams._vx * _timer;
        _sy = _startPos.y + _parabolaPrams._vy * _timer - 0.5f * 9.8f * _timer * _timer;
        _sz = _startPos.z + _parabolaPrams._vz * _timer;
    }
    protected override void CalculateRange(float dt) {
        _range -= dt;
        if (_range <= 0) {
            MonsterBulletPoolManager.Instance.RechargeBullet(this.gameObject, this.transform.parent);
            _isSpit = false;
            _isCalculateFinished = false;
        }
    }
}
