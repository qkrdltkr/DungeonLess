using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo {
    public DamageAttribute Attribute { get; set; }
    public int Damage { get; set; }
    public float PushPower { get; set; }

    public void Set(DamageInfo info) {
        Attribute = info.Attribute;
        Damage = info.Damage;
        PushPower = info.PushPower;
    }
}

public interface IBullet {
    DamageInfo GetBulletInfo();
}
