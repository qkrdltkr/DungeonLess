using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterComponentKind { NONPASS = -1, PATTERN, ATTACK, DAMAGED, SKILL}
public class Monster : Character {

    private PlayerController _playerController;
    private Stats _playerStats;
    
    protected MonsterAIComponent[] _monsterAiComponents = new MonsterAIComponent[4];
    protected bool isMonsterAlive = true;
    public Transform Target { get; set; }
    public MonsterKind _Kind;

    public SpriteRenderer[] _SpriteRenderers;

    protected Stats GetTargetStats() {
        _playerController = OperationData.Player.GetComponentInChildren<PlayerController>();
        _playerStats = _playerController.GetStats();
        return _playerStats;
    }

    public MonsterAIComponent GetAiComponent(int idx) {
        if (idx >= _monsterAiComponents.Length) return null;
        return _monsterAiComponents[idx];
    }

    public void SetTarget(Transform tr) {      
        Target = tr;
        for (int i = 0; i < _monsterAiComponents.Length; i++) {
            _monsterAiComponents[i].SetTarget(Target);
        }
        SetAnimationTarget(Target);
    }
    public void SetAnimationTarget(Transform target) {
        _animationComponent.SetTarget(this.transform, target);
    }
    public void SetAnimationTarget(Vector3 targetpos) {
        _animationComponent.SetTarget(this.transform, targetpos);
    }
    public void Clear(){
        for (int i = 0; i < _monsterAiComponents.Length; i++) {
            _monsterAiComponents[i].SetTarget(null);
            _monsterAiComponents[i].Clear();
        }
        _currentCondition.SetCondition(Condition.kIdle);
    }
    public virtual void DoWork(MonsterComponentKind _kind, Transform tr = null) {
        for (int i = 0; i < _monsterAiComponents.Length; i++) {
            switch (_kind) {
                case MonsterComponentKind.PATTERN:
                    if (_monsterAiComponents[i] is PatternComponent) _monsterAiComponents[i].DoWork(tr);
                    break;
                case MonsterComponentKind.ATTACK:
                    if (_monsterAiComponents[i] is AttackComponent) _monsterAiComponents[i].DoWork(tr);
                    break;
                case MonsterComponentKind.DAMAGED:
                    if (_monsterAiComponents[i] is DamagedComponent) _monsterAiComponents[i].DoWork(tr);
                    break;
                case MonsterComponentKind.SKILL:
                    if (_monsterAiComponents[i] is MonsterSkillComponent) _monsterAiComponents[i].DoWork(tr);
                    break;
                default:
                    break;
            }
        }
    }

    public override void Dead() {
        isMonsterAlive = false;
        this.transform.tag = "Untagged";
        StartCoroutine(DeadDuration(1.0f));
    }
    IEnumerator DeadDuration(float time) {
        if(Random.Range(0,100)%100 == 0 ) //100분의 1로 마석 드랍
            DropStone();
        else if (Random.Range(0, 2) == 1) {//그게아니면 2분의 1로 골드드랍
            DropGold();
        }
          
        yield return YieldInstructionCache.WaitForSeconds(time);

        SpriteRenderer spr = this.GetComponent<SpriteRenderer>();
        for (int i = 10; i > 0; --i) {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, i * 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        Destroy(this.transform.parent.gameObject);
    }
    private void DropStone(){
        GoodsPoolManager goodsPoolManager = GoodsPoolManager.Instance;
        GameObject goods = goodsPoolManager.Pop(GoodsKind.MAGIC_STONE, 1);
        goods.GetComponent<Goods>().Set(this.transform);
    }
    private void DropGold() {
        int coin = 0;
        int tenCoin = 0, fiveCoin = 0, oneCoin = 0;
        GoodsPoolManager goodsPoolManager = GoodsPoolManager.Instance;
        coin = Random.Range(1, 11) * _characterStats.Level;
        
        if (coin >= 10) {
            tenCoin = coin / 10;
            coin = coin % 10;
        }
        else if (coin >= 5) {
            fiveCoin = coin / 5;
            coin = coin % 5;
        }
        if(coin != 0)
        oneCoin = coin / 1;

        GameObject goods;
        for (int i = 0; i < tenCoin; i++) {
            goods = goodsPoolManager.Pop(GoodsKind.COIN, 10);
            goods.GetComponent<Goods>().Set(this.transform);
        }
        for (int i = 0; i < fiveCoin; i++) {
            goods = goodsPoolManager.Pop(GoodsKind.COIN, 5);
            goods.GetComponent<Goods>().Set(this.transform);
        }
        for (int i = 0; i < oneCoin; i++) {
            goods = goodsPoolManager.Pop(GoodsKind.COIN, 1);
            goods.GetComponent<Goods>().Set(this.transform);
        }
    }

    public override void OnFixedUpdate(float dt) {
        if (isMonsterAlive) {
            if (_monsterAiComponents[0] == null) return;
            //AI 컴포넌트 RUN
            for (int i = 0; i < _monsterAiComponents.Length; i++) {
                _monsterAiComponents[i].Run(dt);
            }

            if (_animationComponent != null)
                _animationComponent.Run(dt);
        }
    }

   
    protected void OnMouseEnter() {
        if (DungeonMaster.Mode == GameMode.PLACEMENT) {
            //Outline.SetColor(_SpriteRenderers, new Color(0.0f, 0.6f, 0.0f, 1.0f));

            ((DamagedComponent)_monsterAiComponents[2])._MonsterHpBar.gameObject.SetActive(true);     // HP 표기
            ((DamagedComponent)_monsterAiComponents[2])._MonsterHpBarBack.gameObject.SetActive(true); // HP 표기
            PlacementMonsterToolTip.Instance.OnToolTip(this); // 툴팁 세팅
        } else if (DungeonMaster.Mode == GameMode.ROGUE_LIKE) {
            if (NecronomiconBar.MaxResistancePoint <= 0) MouseCursor.Instance.PlayCursor();
        }

        OperationData.TargetMonster = this.transform.parent;
    }

    protected void OnMouseExit() {
        if (DungeonMaster.Mode == GameMode.PLACEMENT) {
            //Outline.SetColor(_SpriteRenderers, Color.black);
            PlacementMonsterToolTip.Instance.OffToolTip(); // 툴팁 닫기
        } else if (DungeonMaster.Mode == GameMode.ROGUE_LIKE) {
            if(MouseCursor.Instance.Index == CursorIcon.kCapture) MouseCursor.Instance.StopCursor(4);
        }

        OperationData.TargetMonster = null;
    }
}