using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStatusEffect : StatusEffect
{

    protected override void Initialize()
    {
        this.Kind = StatusEffectKind.kStun;
        StatusPoolManager.Instance.SetStatusEffect(this.gameObject, this.transform.parent);
        this.gameObject.SetActive(false);
    }
}
