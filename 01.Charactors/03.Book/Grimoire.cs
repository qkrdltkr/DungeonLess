using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grimoire : MonoBehaviour {
    protected Character.Stats _playerStats;
    protected BookAnimationComponent _bookAnimationComponent;

    public void Init(Character.Stats stats) {
        _playerStats = stats;
        _bookAnimationComponent =
            new BookAnimationComponent(this.transform.GetComponent<Animator>(), this.transform,
               new string[] { "Bottom", "Top", "Side"});
    }

    protected void Update() {
        _bookAnimationComponent.Run(0.0f);
    }

    public abstract void Attack();
}
