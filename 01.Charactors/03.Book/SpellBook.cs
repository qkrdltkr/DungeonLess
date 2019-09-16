using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : Grimoire {
    public MagicalMuzzle _MagicalMuzzle;
    private bool _isAttack;

    private void Start(){
        _isAttack = true;
    }

    void OnEnable() {
        StartCoroutine(Cooldown());
    }
    void OnDisable() {
        StopAllCoroutines();
    }

    public override void Attack() {
        if (OperationData.IsAttackBlock) return;

        if (!_isAttack) return;
        if (_playerStats.ManaPoint < _playerStats.ManaCost) return;
        
        _playerStats.ManaPoint -= _playerStats.ManaCost;

        _MagicalMuzzle.UseSpell(_playerStats);
        _isAttack = false;
    }

    private IEnumerator Cooldown() {
        float coolDown = 0.0f;
        while(true) {
            coolDown = _playerStats == null ? 1.0f : _playerStats.ShootSpeed;
            yield return YieldInstructionCache.WaitForSeconds(coolDown);
            _isAttack = true;
        }
    }
}