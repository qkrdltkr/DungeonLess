using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageAttribute { NONPASS = -1,
    NORMAL, BURN, FREEZING, POISON, BIND, CURSE, ELECTRIC, }

public class BulletAttributeMgr {
    private DamageAttribute[] _damageAttribute = new DamageAttribute[100];
    
    public int Burn     { get; set; }
    public int Freezing { get; set; }
    public int Poison   { get; set; }
    public int Bind     { get; set; }
    public int Curse    { get; set; }
    public int Electric { get; set; }

    public void Shuffle() {
        for (int i = 0; i < _damageAttribute.Length; ++i)
            _damageAttribute[i] = DamageAttribute.NORMAL;

        for (int i = 0; i < _damageAttribute.Length; ++i) {
            if (i < Burn) {
                _damageAttribute[i] = DamageAttribute.BURN;
                continue;
            } else if (i < Burn + Freezing) {
                _damageAttribute[i] = DamageAttribute.FREEZING;
                continue;
            } else if (i < Burn + Freezing + Poison) {
                _damageAttribute[i] = DamageAttribute.POISON;
                continue;
            } else if (i < Burn + Freezing + Poison + Bind) {
                _damageAttribute[i] = DamageAttribute.BIND;
                continue;
            } else if (i < Burn + Freezing + Poison + Bind + Curse) {
                _damageAttribute[i] = DamageAttribute.CURSE;
                continue;
            } else if (i < Burn + Freezing + Poison + Bind + Curse + Electric) {
                _damageAttribute[i] = DamageAttribute.ELECTRIC;
                continue;
            }
        }

        Utility.ShuffleArray<DamageAttribute>(_damageAttribute);
    }

    public DamageAttribute GetAttribute() {
        return _damageAttribute[Random.Range(0, _damageAttribute.Length)];
    }
}

public class BulletPoolManager : Singleton<BulletPoolManager> {
    public static BulletPoolManager Instance {
        get { return (BulletPoolManager)_Instance; }
        set { _Instance = value; }
    }

    // Pool
    public GameObject _BulletPrefab;
    private List<GameObject> _bullets = new List<GameObject>();
    // 특성
    private List<string> _collisionTags = new List<string>() {
        "Hero", "Monster", "Wall" };
    public BulletUpdateEvent BulletUpdateEvent       { get; private set; }
    public BulletCollisionEvent BulletCollisionEvent { get; private set; }
    private BulletAttributeMgr _bulletAttributeMgr = new BulletAttributeMgr();
    public Color[] _AtributeColors = new Color[] { 
        Color.white, Color.red, Color.blue, Color.magenta, Color.green,
        Color.black, Color.yellow
        };

    #region Specificity
    public BulletAttributeMgr GetAttributeMgr()  { return _bulletAttributeMgr; }
    public List<string> GetCollisionTags()       { return _collisionTags; }
    public void AddEvent(BulletUpdateEvent e)    { BulletUpdateEvent += e; }
    public void AddEvent(BulletCollisionEvent e) { BulletCollisionEvent += e; }
    #endregion

    #region Pool
    void Start() {
        for (int i = 0; i < this.transform.childCount; ++i)
            _bullets.Add(this.transform.GetChild(i).gameObject);
    }

    public void Reload(GameObject bullet) {
        if (_bullets.Contains(bullet)) return;

        bullet.SetActive(false);
        bullet.transform.parent = this.transform;
        _bullets.Add(bullet);
    }
    public GameObject Pop(Character.Stats stats) {
        GameObject bullet = _bullets.Count == 0 ? SupplyBullet() : _bullets[0];
        _bullets.RemoveAt(0);

        // 기초 세팅
        // 마테리얼 임시 적용
        DamageAttribute ba = _bulletAttributeMgr.GetAttribute();
        bullet.GetComponent<Bullet>().Set(stats, ba);
        bullet.GetComponent<SpriteRenderer>().color = _AtributeColors[(int)ba];
        return bullet;
    }
    private GameObject SupplyBullet() {
        GameObject bullet = Instantiate(_BulletPrefab, this.transform.position, Quaternion.identity, this.transform);
        _bullets.Add(bullet);
        return bullet;
    }
    #endregion
}
