using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IUpdateableObject, IBullet {
    // 특성
    private BulletUpdateEvent _bulletUpdateEvent;
    private BulletCollisionEvent _bulletCollisionEvent;
    // 불렛
    private Rigidbody _rigidbody;
    private Vector3 _target;
    private Vector3 _direation;
    private Vector3 _lastFrameVelocity;
    // 스텟
    DamageInfo _bulletInfo = new DamageInfo();
    private float _speed;
    private float _range;
    // Effect
    public BulletCollisionEffect _CollisionEffect;

    protected override void Initialize() {
        // 총알 초기화
        BulletPoolManager bulletPoolManager = BulletPoolManager.Instance;
        _rigidbody = this.GetComponent<Rigidbody>();

        _bulletUpdateEvent    = bulletPoolManager.BulletUpdateEvent;
        _bulletCollisionEvent = bulletPoolManager.BulletCollisionEvent;
    }

    public override void OnFixedUpdate(float dt) {
        if(_bulletUpdateEvent != null)
            _bulletUpdateEvent(this.transform, _direation, _rigidbody, _speed);
    }

    public override void OnUpdate(float dt) {
        // 총알 상태 갱신
        if (!_rigidbody) return;
        _lastFrameVelocity = _rigidbody.velocity;
        // 총알 수명
        _range -= dt;
        if (_range <= 0) {
           BulletPoolManager.Instance.Reload(this.gameObject);
        }
    }

    public DamageInfo GetBulletInfo() { return _bulletInfo; }

    public void Set(Character.Stats stats, DamageAttribute attribute) {
        _bulletInfo.Damage = stats.Damage;
        _range = stats.Range;
        _bulletInfo.PushPower = stats.PushPower;
        _bulletInfo.Attribute = attribute;

        this.transform.localScale = Vector3.one * 0.1f * _bulletInfo.Damage;
    }

    public void Fire(Vector3 pos, float speed, Vector3 dir) {
        // 위치 및 목표 설정
        this.transform.position = new Vector3(pos.x, pos.y, ObjectSortLevel.kBullet);
        _speed = speed;
        // 반동 설정
        Rebound(dir);
        // Fire !!!
        this.gameObject.SetActive(true);
        this.transform.parent = null;

        if(!_rigidbody) _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true; _rigidbody.isKinematic = false;

        dir = (new Vector3(_direation.x, _direation.y, 0.0f).normalized);
        this.transform.up = dir;
        _rigidbody.AddForce(dir * 100.0f * _speed);
    }

    private void Rebound(Vector3 dir) {
        dir = dir.normalized ;
        dir.x += Random.Range(-dir.y * 0.15f, dir.y * 0.15f);
        dir.y += Random.Range(-dir.x * 0.15f, dir.x * 0.15f);
        _direation = dir;
    }

    private void OnCollisionEnter(Collision target) {
        if(_bulletCollisionEvent != null)
            _bulletCollisionEvent(this.transform, target, _rigidbody,_lastFrameVelocity);
    }

    private void OnTriggerEnter(Collider target) {
        // 충돌처리
        if (!target.CompareTag("Hero") && !target.CompareTag("Monster") && !target.CompareTag("Wall")) return;
        Monster _monster = target.transform.GetComponent<Monster>();
        Hero _hero = target.transform.GetComponent<Hero>();
        if (_monster) {
            _monster.DoWork(MonsterComponentKind.DAMAGED, this.transform);
        }
        else if(_hero) {
            _hero.DoWork(HeroComponentKind.DAMAGED, this.transform);
        }
        // 리로드
        Reload(target);
    }
    private void Reload(Collider target) {
        for (int i = 0; i < BulletPoolManager.Instance.GetCollisionTags().Count; ++i) {
            if (target.CompareTag(BulletPoolManager.Instance.GetCollisionTags()[i]) || !target) {
                _CollisionEffect.StartEffect();
                BulletPoolManager.Instance.Reload(this.gameObject);
                return;
            }
        }
    }
}
