using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadialSpitBullet : MonsterBullets {

    private List<Vector3> destPoss = new List<Vector3>();
    private List<ParabolaPrameter> destinations = new List<ParabolaPrameter>();
    private ParabolaPrameter parabolaPrams;
    protected override void Initialize(){
        _bulletNum = 20;
        _spitObjs = new GameObject[_bulletNum];
        this.Kind = MonsterBulletKind.kRadialMucus;
        MonsterBulletPoolManager.Instance.SetBullet(this.gameObject, this.transform.parent);
        for (int i = 0; i < _bulletNum; i++) {
            GameObject obj = Instantiate(_bullet.gameObject, this.transform);
            obj.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
    public void Fire(Transform monster, Transform target, Condition condition){
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
        _startPos = _firePos.position;
        _condition = condition;
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
        if (_isSpit)  {
            _timer += dt;
            for (int i = 0; i < _bulletNum; i++) {
                if (!_isCalculateFinished)  {
                    InitParabola(i, _target); //출발지, 도착지 계산
                }
            }
            _isCalculateFinished = true;
            for (int i = 0; i < _bulletNum; i++) {
                CalculateParabola(i); //포물선 계산
                _spitObjs[i].transform.position = new Vector3(_sx, _sy, _sz);
                ExplosionBullet(i); // 닿으면 폭발
            }
            CalculateRange(dt); //딜레이 계산
        }
    }

    protected override void ExplosionBullet(int i) {
        _distance = Vector3.Distance(_spitObjs[i].transform.position, destPoss[i]);
        if (_distance < 1f) {
            _spitObjs[i].GetComponent<MonsterBulletSensor>().ExplosionBullet();
        }
    }

    protected override void InitParabola(int i, Transform target) {
        _destPos = new Vector3(target.position.x + Random.Range(-3f, 3f), target.position.y +
            Random.Range(-3f, 3f), target.position.z);
        destPoss.Add(_destPos);
        parabolaPrams._vx = (_destPos.x - _startPos.x) / 2f;
        parabolaPrams._vy = (_destPos.y - _startPos.y + 2 * 9.8f) / 2f;
        parabolaPrams._vz = (_destPos.z - _startPos.z) / 2f;

        _spitObjs[i] = this.transform.GetChild(i).gameObject;
        _spitObjs[i].SetActive(true);

        destinations.Add(parabolaPrams);
    }
    protected override void CalculateParabola(int i) {
        _sx = _startPos.x + destinations[i]._vx * _timer;
        _sy = _startPos.y + destinations[i]._vy * _timer - 0.5f * 9.8f * _timer * _timer;
        _sz = _startPos.z + destinations[i]._vz * _timer;
    }
    protected override void CalculateRange(float dt)  {
        _range -= dt;
        if (_range <= 0)  {
            MonsterBulletPoolManager.Instance.RechargeBullet(this.gameObject, this.transform.parent);
            _isCalculateFinished = false;
            _isSpit = false;
            destinations.Clear();
            destPoss.Clear();
        }
    }
}
