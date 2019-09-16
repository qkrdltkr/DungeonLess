using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBulletSensor : MonoBehaviour {

    public enum MonsterBulletKind { NONPASS = -1, RADIATE, MUCUS, RADIATEMUCUS}
    public MonsterBulletKind _MonsterBulletKind;

    private Character.Stats _monsterstats;
    private DamageInfo _bulletInfo;
    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player") || col.CompareTag("Hero")) {
            switch (_MonsterBulletKind) {
                case MonsterBulletKind.RADIATE:
                    _monsterstats = this.transform.parent.GetComponent<RadialBullet>()._monsterstats;
                    break;
                case MonsterBulletKind.MUCUS:
                    _monsterstats = this.transform.parent.GetComponent<SpitBullet>()._monsterstats;
                    break;
                case MonsterBulletKind.RADIATEMUCUS:
                    _monsterstats = this.transform.parent.GetComponent<RadialSpitBullet>()._monsterstats;
                    break;
                    
            }
            if (col.CompareTag("Player"))
                col.GetComponent<PlayerController>().Damaged(Vector3.zero, _monsterstats.Damage,
                    StatusEffectKind.kNoStatusEffect);
            else if (col.CompareTag("Hero"))
                col.GetComponentInChildren<Hero>().DoWork(HeroComponentKind.DAMAGED, this.transform.parent);

            ExplosionBullet();
        }
    }
    public void ExplosionBullet() {
        if(!this.gameObject.activeInHierarchy) return;

        this.gameObject.SetActive(false);
        if (this.transform.childCount < 1) return;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(0).GetComponent<BulletCollisionEffect>().StartEffect();
    }
}
