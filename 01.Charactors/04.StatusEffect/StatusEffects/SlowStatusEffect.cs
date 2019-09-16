using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowStatusEffect : StatusEffect
{

    protected override void Initialize()
    {
        this.Kind = StatusEffectKind.kSlow;
        StatusPoolManager.Instance.SetStatusEffect(this.gameObject, this.transform.parent);
        this.gameObject.SetActive(false);
    }
}
