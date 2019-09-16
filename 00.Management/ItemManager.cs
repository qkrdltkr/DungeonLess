using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemGrade { NONPASS = -1, D, C, B, A, S, SS }
public enum ItemKind {
    CHAIR = -2,
    NONPASS = -1,
    // 0 ~ 20 스텟 증가
    STEAK,
    // 20 ~ 40 패시브
    // 40 ~ 60 액티브
    TestActive = 40,
    // 60 ~ 70 공격방식 변경
    DOUBLE_SHOT = 60,
    TRIPLE_SHOT,
    // 70 ~ 100 탄환 속성 변경
    BURN = 70,
    FREEZING,
    POISON,
    BIND,
    CURSE,
    ELECTRIC,
    // 100 ~ 120 탄환 특징 변경
    PIERCING = 100,
    GUIDED, // 유도
    BOUNCING,
}

public delegate void MuzzleEvent (Transform[] axes,  Character.Stats stats, Transform pos);
public delegate void BulletUpdateEvent(Transform origin, Vector3 dir, Rigidbody rb, float speed);
public delegate void BulletCollisionEvent(Transform origin,Collision target, Rigidbody rb, Vector3 velocity);

public class ItemManager : Singleton<ItemManager> {
    public static ItemManager Instance {
        get { return (ItemManager)_Instance; }
        set { _Instance = value; }
    }

    // Stat 템, Muzzle
    private PlayerController _playerController;
    private MagicalMuzzle _magicalMuzzle;

    private BulletPoolManager _bulletPoolManager;
    private BulletAttributeMgr _bulletAttributeMgr;

    public void Init(PlayerController playerController) {
        _playerController = playerController;
        _bulletPoolManager = BulletPoolManager.Instance;
        _bulletAttributeMgr = _bulletPoolManager.GetAttributeMgr();
        _magicalMuzzle = GameObject.Find("Weapons").GetComponent<MagicalMuzzle>();

        _magicalMuzzle.AddEvent(Single);
           // 유도
//         _bulletPoolManager.AddEvent(GuidedBullet);
//         // 관통
//         _bulletPoolManager.GetCollisionTags().Remove("Monster");
//         // 바운싱
//         _bulletPoolManager.GetCollisionTags().Remove("Wall");
//         _bulletPoolManager.AddEvent(BouncingBullet);
    }

    public void GetItem(Item item) {
        int index = (int)item._Kind;
        if      (index >= 0   && index < 20) { }
        else if (index >= 20  && index < 40) { }
        else if (index >= 40  && index < 60) { }
        else if (index >= 60  && index < 70)  SetMuzzle(item._Kind);
        else if (index >= 70  && index < 100) SetBulletAttribute(item._Kind);
        else if (index >= 100 && index < 120) SetBulletSpecificity(item._Kind);

        _playerController.Apply(item._Stats);
    }

    private void SetMuzzle(ItemKind item) {
        switch (item) {
            case ItemKind.DOUBLE_SHOT:
                _magicalMuzzle.DeleteEvent(Single);
                _magicalMuzzle.AddEvent(Dual);
                break;
            case ItemKind.TRIPLE_SHOT:
                _magicalMuzzle.DeleteEvent(Single);
                _magicalMuzzle.AddEvent(Triple);
                break;
        }
    }

    private void SetBulletAttribute(ItemKind item) {
        switch (item) {
            case ItemKind.BURN:     _bulletAttributeMgr.Burn     += 15;   break;
            case ItemKind.FREEZING: _bulletAttributeMgr.Freezing += 15;   break;
            case ItemKind.POISON:   _bulletAttributeMgr.Poison   += 15;   break;
            case ItemKind.BIND:     _bulletAttributeMgr.Bind     += 15;   break;
            case ItemKind.CURSE:    _bulletAttributeMgr.Curse    += 15;   break;
            case ItemKind.ELECTRIC: _bulletAttributeMgr.Electric += 15;   break;
        }

        _bulletAttributeMgr.Shuffle();
    }

    private void SetBulletSpecificity(ItemKind item) {
        switch (item)  {
            case ItemKind.PIERCING: _bulletPoolManager.GetCollisionTags().Remove("Monster"); break;
            case ItemKind.GUIDED:   _bulletPoolManager.AddEvent(GuidedBullet);               break;
            case ItemKind.BOUNCING:
                _bulletPoolManager.GetCollisionTags().Remove("Wall");
                _bulletPoolManager.AddEvent(BouncingBullet);
                break;
        }
    }

    //=======================================================================================
    // Muzzle Event
    //=======================================================================================
    #region
    private void Single(Transform[] axes, Character.Stats stats, Transform pos) {
        Bullet bullet = BulletPoolManager.Instance.Pop(stats).GetComponent<Bullet>();
        bullet.Fire(axes[2].position, stats.BulletSpeed,
            Utility.LookAt(MainCamController.MousePosition, pos.position));
    }

    private void Dual(Transform[] axes, Character.Stats stats, Transform pos) {
        Bullet bullet = BulletPoolManager.Instance.Pop(stats).GetComponent<Bullet>();
        bullet.Fire(axes[1].position, stats.BulletSpeed,
            Utility.LookAt(MainCamController.MousePosition, pos.position));

        bullet = BulletPoolManager.Instance.Pop(stats).GetComponent<Bullet>();
        bullet.Fire(axes[3].position, stats.BulletSpeed,
            Utility.LookAt(MainCamController.MousePosition, pos.position));
    }

    private void Triple(Transform[] axes, Character.Stats stats, Transform pos) {
        Bullet bullet = BulletPoolManager.Instance.Pop(stats).GetComponent<Bullet>();
        bullet.Fire(axes[0].position, stats.BulletSpeed,
            Utility.LookAt(MainCamController.MousePosition, pos.position));

        bullet = BulletPoolManager.Instance.Pop(stats).GetComponent<Bullet>();
        bullet.Fire(axes[2].position, stats.BulletSpeed,
            Utility.LookAt(MainCamController.MousePosition, pos.position));

        bullet = BulletPoolManager.Instance.Pop(stats).GetComponent<Bullet>();
        bullet.Fire(axes[4].position, stats.BulletSpeed,
            Utility.LookAt(MainCamController.MousePosition, pos.position));
    }
    #endregion
    //=======================================================================================
    // Bullet Update Event
    //=======================================================================================
    #region
    private void GuidedBullet(Transform origin,  Vector3 dir, Rigidbody rb, float speed) {
        Transform target = origin.GetChild(0).GetComponent<BulletSensor>().Target;
        if (!target) return;
        dir = Utility.LookAt(target, origin);
        rb.isKinematic = true;  rb.isKinematic = false;
        origin.up = dir;
        rb.AddForce((new Vector3(dir.x, dir.y, 0.0f)).normalized * speed * 100.0f);

        target = null;
    }
    #endregion
    //=======================================================================================
    // Bullet Collision Event
    //=======================================================================================
    #region
    private void BouncingBullet(Transform origin, Collision target, Rigidbody rb,Vector3 velocity) {
        var speed = velocity.magnitude;
        var direction = Vector3.Reflect(velocity.normalized, target.contacts[0].normal).normalized;

        rb.velocity = direction * speed;
    }
    #endregion
}
