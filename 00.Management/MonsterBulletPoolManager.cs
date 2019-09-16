using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MonsterBulletKind{
    public static int maxBulletCount = 3;
    public const int kRadiate = 0;
    public const int kMucus = 1;
    public const int kRadialMucus = 2;

}

    public class MonsterBulletPoolManager : Singleton<MonsterBulletPoolManager> {
    public static MonsterBulletPoolManager Instance {
    get { return (MonsterBulletPoolManager)_Instance; }
    set { _Instance = value; }
    }
    public GameObject[] _bulletPrefabs;
    private MonsterBulletKind _bulletKind;

    private List<GameObject> bullets = new List<GameObject>();
    private GameObject _child;
    private Transform _obj;
   
    private GameObject _poolChild;
    private void Start() {
        Transform Parent = this.transform;
        for (int i = 0; i < Parent.childCount; i++) {
            FullPool(i, Parent.GetChild(i));
        }
    }
    private void FullPool(int idx, Transform child) {
        for (int i = 0; i < 10; i++){
            _poolChild = Instantiate(_bulletPrefabs[idx]);
            _poolChild.SetActive(true);
            _poolChild.transform.parent = child;
            _poolChild.transform.position = child.position;
        }
    }
    public void RechargeBullet(GameObject bullet, Transform kind) {
        for (int i = 0; i < bullet.transform.childCount; i++){    
            Rigidbody rd = bullet.transform.GetChild(i).GetComponent<Rigidbody>();
            rd.isKinematic = bullet; rd.isKinematic = false;
            _obj = bullet.transform.GetChild(i);
            _obj.transform.position = bullet.transform.position;
            _obj.transform.rotation = Quaternion.identity;
            _obj.gameObject.SetActive(false);
        }

        bullet.transform.parent = kind;
        bullets.Add(bullet);
        bullet.SetActive(false);
    }

    public void SetBullet(GameObject bullet, Transform kind) {
        bullet.transform.parent = kind;
        bullets.Add(bullet);
    }
    public GameObject PopBullet(int kind) {
        int bulletIdx = 0;

        for (int j = 0; j < bullets.Count; j++) {
            if (bullets[j].GetComponent<MonsterBullets>().Kind != kind) bulletIdx++;
            else j = bullets.Count;
        }
        GameObject bullet;

        if (bulletIdx == bullets.Count) bullet = SupplyBullet(kind);
        else bullet = bullets[bulletIdx];

        bullets.RemoveAt(bulletIdx);
        return bullet;
    }
    private GameObject SupplyBullet(int kind) {
        GameObject bullet = Instantiate(_bulletPrefabs[kind], this.transform.position, Quaternion.identity, this.transform);
        bullets.Add(bullet);
        return bullet;
    }
}
