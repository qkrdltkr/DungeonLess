using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBullet : MonsterBullets {
    private bool _isFired = false;
    protected override void Initialize() {
        _bulletNum = 16; //16개씩 발사
        this.Kind = MonsterBulletKind.kRadiate;
        MonsterBulletPoolManager.Instance.SetBullet(this.gameObject, this.transform.parent);
        for (int i = 0; i < _bulletNum; i++) {
            GameObject obj = Instantiate(_bullet.gameObject, this.transform);
            obj.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }

    public override void OnUpdate(float dt) {
        if (!_isFired) return;
        // 총알 수명
        _range -= dt;
        if (_range <= 0) {
            MonsterBulletPoolManager.Instance.RechargeBullet(this.gameObject, this.transform.parent);
            _isFired = false;
        }
    }
    public void Fire(Character.Stats monsterstats, Transform monster) {
        this.gameObject.SetActive(true);
        _monsterstats = monsterstats;
        //총알세팅
        _range = monsterstats.Range;
        _bulletInfo.Damage = _monsterstats.Damage;
        _bulletInfo.PushPower = _monsterstats.PushPower;
        _bulletInfo.Attribute = DamageAttribute.FREEZING;

        _isFired = true;
        StartCoroutine(FireBullet(_bulletNum, monster));
     
    }
    private IEnumerator FireBullet(int idx, Transform monster) {
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        for (int i = 0; i < idx; i++)  {

            GameObject obj = this.transform.GetChild(i).gameObject;
            obj.transform.position = monster.GetChild(2).transform.position;
            obj.SetActive(true);

            obj.GetComponent<Rigidbody>().AddForce(new Vector3(100f * _monsterstats.BulletSpeed * Mathf.Cos(Mathf.PI * 2 * i / _bulletNum),
                100f * _monsterstats.BulletSpeed * Mathf.Sin(Mathf.PI * i * 2 / _bulletNum)), 0);

            obj.transform.Rotate(new Vector3(0f, 0f, 360 * i / _bulletNum - 90));
        }
    }
}
