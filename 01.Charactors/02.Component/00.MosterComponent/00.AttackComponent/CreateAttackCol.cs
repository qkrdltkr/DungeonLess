using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAttackCol : MonoBehaviour {

    public bool IsPlayerInAttackCol { get; set; }
    public bool IsHeroInAttackCol { get; set; }
    public bool IsMonsterInAttackCol { get; set; }
    public bool IsAttacking { get; set; }
    public Transform _Target;
    private bool _isMonster;
    private bool _isHero;
    private void Start() {
        if (_Target.CompareTag("Monster")) {
            _isMonster = true;
        }
        else if (_Target.CompareTag("Hero")){
            _isHero = true;
        }

        StartCoroutine(TurnOffCol());
    }
    private void OnTriggerEnter(Collider target) {
        if (target.CompareTag("Player")) {
            IsPlayerInAttackCol = true;
            this.gameObject.SetActive(false);
            IsAttacking = false;
        }
        if(target.CompareTag("Hero")) {
            if (!_isHero) {
                IsHeroInAttackCol = true;
                this.gameObject.SetActive(false);
                IsAttacking = false;
            }
        }
        if (target.CompareTag("Monster")) {
            if (!_isMonster) {
                IsMonsterInAttackCol = true;
                this.gameObject.SetActive(false);
                IsAttacking = false;
            }
            
        }
    }
    private IEnumerator TurnOffCol(){
        if (this.gameObject.activeInHierarchy){
            yield return new WaitForSeconds(0.5f);
            IsPlayerInAttackCol = false;
            this.gameObject.SetActive(false);
            StopCoroutine(TurnOffCol());
        }
    }
}
