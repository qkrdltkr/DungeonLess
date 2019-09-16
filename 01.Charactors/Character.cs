using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition {
    public int currentCondition { private set; get; }
    public int conditionKind { private set; get; }
    public const int kNonpass      = -1;
    public const int kIdle         = 0;
    public const int kRun          = 1;
    public const int kAttack       = 2;
    public const int kDead         = 3;
    public void SetCondition(int con) { currentCondition = con; }
    public void SetCondition(int con, int kind) { currentCondition = con; conditionKind = kind;}
    public static bool operator == (Condition lp, Condition rp) {
        return lp.currentCondition == rp.currentCondition;
    }
    public static bool operator !=(Condition lp, Condition rp) {
        return lp.currentCondition != rp.currentCondition;
    }

    public bool IsSame(int con) {
        return currentCondition == con;
    }
    public override bool Equals(object obj) {
        return base.Equals(obj);
    }
    public override int GetHashCode() {
        return base.GetHashCode();
    }
}

public abstract class Character : IUpdateableObject {
    [System.Serializable]
    public class Stats {
        public Stats() { }
        public float MoveSpeed;           // 이동속도

        public float BulletSpeed;         // 투사체 속도
        public float ShootSpeed;          // 연사 속도
        public float Range;               // 사거리

        public float ResistancePoint;     // 저항력(몬스터 포획 저항치 or 몬스터 포획력)
        public int Damage;                // 공격력

        public float PushPower;     // 넉백
        public int MaxHeatPoint;    // 최대 체력
        public float MaxManaPoint ; // 최대 마나
        public int HeatPoint;       // 현재 체력
        public float ManaPoint;     // 현재 마나

        public float ManaRecovery; // 마나 회복량
        public float ManaCost;     // 마나 소모량

        public float Luck;           // 운으로 변경
        public string Name;          // 이름
        public string Description;   // 설명
        public float CurrentExp;     // 경험치
        public float MaxExp;         // 다음 레벨까지 경험치
        public int Level;            // 레벨

        public int Price;            // 가격
        public int Population;       // 인구

        public MonsterTribe Tribe;

        public void Apply(Stats stats) {
            this.MoveSpeed       += stats.MoveSpeed;       CheckValue(ref MoveSpeed);
            this.BulletSpeed     += stats.BulletSpeed;     CheckValue(ref BulletSpeed);
            this.ShootSpeed      += stats.ShootSpeed;      CheckValue(ref ShootSpeed);
            this.ResistancePoint += stats.ResistancePoint; CheckValue(ref ResistancePoint);
            this.Damage          += stats.Damage;          CheckValue(ref Damage);
            this.PushPower       += stats.PushPower;       CheckValue(ref PushPower);

            this.MaxHeatPoint += stats.MaxHeatPoint; CheckValue(ref MaxHeatPoint);
            this.MaxManaPoint += stats.MaxManaPoint; CheckValue(ref MaxManaPoint);

            if (this.HeatPoint + stats.HeatPoint > this.MaxHeatPoint) this.HeatPoint = this.MaxHeatPoint;
            else this.HeatPoint += stats.HeatPoint;
            if (this.ManaPoint + stats.ManaPoint > this.MaxManaPoint) this.ManaPoint = this.MaxManaPoint;
            else this.ManaPoint += stats.ManaPoint;
             
            this.ManaRecovery += stats.ManaRecovery; CheckValue(ref ManaRecovery);

            this.ManaCost += stats.ManaCost;    CheckValue(ref ManaCost);
            this.Luck     += stats.Luck;        CheckValue(ref Luck);
        }


        private void CheckValue(ref float value) { if (value < 0.0f) value = 0.0f; }
        private void CheckValue(ref int value)   { if (value < 0)    value = 0;    }

        public void CheckNegative() {
            CheckValue(ref MoveSpeed);
            CheckValue(ref BulletSpeed);
            CheckValue(ref ShootSpeed);
            CheckValue(ref ResistancePoint);
            CheckValue(ref Damage);
            CheckValue(ref PushPower);

            CheckValue(ref MaxHeatPoint);
            CheckValue(ref MaxManaPoint);

            CheckValue(ref ManaRecovery);

            CheckValue(ref ManaCost);
            CheckValue(ref Luck);
        }
    }
    // Character
    protected Condition _currentCondition = new Condition();
    protected Stats _characterStats = new Stats();
    public ElementalAttributes _Attributes;

    // Index
    private MapIndex _index = new MapIndex();
    public MapIndex Index { get { return _index; } set { _index = value; } }

    protected AnimationComponent _animationComponent = null;
    private StatusEffectController _statusEffectController = new StatusEffectController();
    protected override void Initialize() {
        _currentCondition.SetCondition(Condition.kIdle);
    }
    public virtual void AddStatusEffect(int kind) {
        _statusEffectController.AddStatusEffect(kind, this.transform);
    }
    
    public virtual void RemoveStatusEffect(int kind ) {
        _statusEffectController.RemoveStatusEffect(kind);
    }
    public virtual List<StatusEffect> GetStatusEffect(){
        return _statusEffectController.GetStatusEffect();
    }
    public Stats GetStats() { return _characterStats; }
    public Condition GetCurrentCondition() { return _currentCondition; }
    public abstract void Dead();
}
