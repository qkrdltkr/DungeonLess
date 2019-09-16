using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroComponentKind {  NONPASS = -1, PATTERN, ATTACK, DAMAGED}
public class Hero : Character {

    private PlayerController _playerController;
    private Stats _playerStats;
    public HeroKind _Kind;
    public HeroRank _Rank;

    protected HeroAIComponent[] _heroAiComponents = new HeroAIComponent[3];
    protected bool isHeroAlive = true;
    public Transform Target { get; set; }

    protected Stats GetTargetStats() {
        _playerController =  OperationData.Player.GetComponentInChildren<PlayerController>();
        _playerStats = _playerController.GetStats();

        return _playerStats;
    }

    public HeroAIComponent GetAiComponent(int idx) {
        if (idx >= _heroAiComponents.Length) return null;
        return _heroAiComponents[idx];
    }
    public void SetTarget(Transform tr) {
        Target = tr;
        for (int i = 0; i < _heroAiComponents.Length; i++) {
            _heroAiComponents[i].SetTarget(Target);
        }
        SetAnimationTarget(Target);
    }
    public void SetAnimationTarget(Transform target) {
        _animationComponent.SetTarget(this.transform, target);
    }
    public void SetAnimationTarget(Vector3 targetpos) {
        _animationComponent.SetTarget(this.transform, targetpos);
    }
    public virtual void DoWork(HeroComponentKind _kind, Transform tr = null) {
        for (int i = 0; i < _heroAiComponents.Length; i++) {
            switch (_kind) {
                case HeroComponentKind.PATTERN:
                    if (_heroAiComponents[i] is HeroTracePatternComponent) _heroAiComponents[i].DoWork(tr);
                    break;
                case HeroComponentKind.ATTACK:
                    if (_heroAiComponents[i] is HeroAttackComponent) _heroAiComponents[i].DoWork(tr);
                    break;
                case HeroComponentKind.DAMAGED:
                    if (_heroAiComponents[i] is HeroDamagedComponent) _heroAiComponents[i].DoWork(tr);
                    break;
                default:
                    break;
            }
        }
    }
    public void Damaged(Transform Tr) {
        _heroAiComponents[2].DoWork(Tr);
    }
    public override void Dead() {
        isHeroAlive = false;
        this.transform.tag = "Untagged";
        StartCoroutine(DeadDuration(1.0f));
    }
    IEnumerator DeadDuration(float time)  {
    
        yield return YieldInstructionCache.WaitForSeconds(time);

        SpriteRenderer spr = this.GetComponent<SpriteRenderer>();
        for (int i = 10; i > 0; --i) {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, i * 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        Destroy(this.transform.parent.gameObject);
    }

    public override void OnFixedUpdate(float dt) {
        if (isHeroAlive) {
            if (_heroAiComponents[0] == null) return;
            //AI 컴포넌트 RUN
            for (int i = 0; i < _heroAiComponents.Length; i++) {
                _heroAiComponents[i].Run(dt);
            }
            //애니메이션 컴포넌트 RUN
            if (_animationComponent != null)
                _animationComponent.Run(dt);
        }
    }
//     protected void OnMouseEnter() {
//         if (DungeonMaster.GameMode != GameMode.PLACEMENT) return;
//         ((HeroDamagedComponent)_heroAiComponents[2])._MonsterHpBar.gameObject.SetActive(true);     // HP 표기
//         ((HeroDamagedComponent)_heroAiComponents[2])._MonsterHpBarBack.gameObject.SetActive(true); // HP 표기
//         PlacementMonsterToolTip.Instance.OnToolTip(this); // 툴팁 세팅
//     }
// 
//     protected void OnMouseExit() {
//         if (DungeonMaster.GameMode != GameMode.PLACEMENT) return;
//         // HP 닫기
//         PlacementMonsterToolTip.Instance.OffToolTip(); // 툴팁 닫기
//     }
}
