using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contents에 Convert index to Name 함수하고 동기화 해야함
public struct SkillKind { 
    public static int maxSkill = 61;
    public static int defaultSkillCount = 11;

    public const int DeFaultSkill = 0;
    public const int FlameSkill = 1;
    public const int WaterSkill = 2;
    public const int NaturalSkill = 3;
    public const int LightSkill = 4;
    public const int DarknessSkill = 5;
    //Default
    public const int kHaste = 0;
    public const int kRapid = 1;
    public const int kSharp = 2;
    public const int kHealty = 3;
    public const int kVolant = 4;
    public const int kSonic = 5;
    public const int kSuperArmor = 6;
    public const int kGiant = 7;
    public const int kHeadHunter = 8;
    public const int kSmash = 9;
    public const int kTwiceAttack = 10;
    //Flame
    public const int kLava = 11;
    public const int kFlameColak = 12;
    public const int kFlameTmp1 = 13;
    public const int kFlameTmp2 = 14;
    public const int kFlameTmp3 = 15;
    public const int kFlameTmp4 = 16;
    public const int kFlameTmp5 = 17;
    public const int kFlameTmp6 = 18;
    public const int kFlameTmp7 = 19;
    public const int kFlameTmp8 = 20;
    //Water
    public const int kSlow = 21;
    public const int kWaterTmp1 = 22;
    public const int kWaterTmp2 = 23;
    public const int kWaterTmp3 = 24;
    public const int kWaterTmp4 = 25;
    public const int kWaterTmp5 = 26;
    public const int kWaterTmp6 = 27;
    public const int kWaterTmp7 = 28;
    public const int kWaterTmp8 = 29;
    public const int kWaterTmp9 = 30;
    //Natural
    public const int kNaturalTmp1 = 31;
    public const int kNaturalTmp2 = 32;
    public const int kNaturalTmp3 = 33;
    public const int kNaturalTmp4 = 34;
    public const int kNaturalTmp5 = 35;
    public const int kNaturalTmp6 = 36;
    public const int kNaturalTmp7 = 37;
    public const int kNaturalTmp8 = 38;
    public const int kNaturalTmp9 = 39;
    public const int kNaturalTmp10 = 40;
    //Light
    public const int kLightTmp1 = 41;
    public const int kLightTmp2 = 42;
    public const int kLightTmp3 = 43;
    public const int kLightTmp4 = 44;
    public const int kLightTmp5 = 45;
    public const int kLightTmp6 = 46;
    public const int kLightTmp7 = 47;
    public const int kLightTmp8 = 48;
    public const int kLightTmp9 = 49;
    public const int kLightTmp10 = 50;
    //DarkNess
    public const int kDarkNessTmp1 = 51;
    public const int kDarkNessTmp2 = 52;
    public const int kDarkNessTmp3 = 53;
    public const int kDarkNessTmp4 = 54;
    public const int kDarkNessTmp5 = 55;
    public const int kDarkNessTmp6 = 56;
    public const int kDarkNessTmp7 = 57;
    public const int kDarkNessTmp8 = 58;
    public const int kDarkNessTmp9 = 59;
    public const int kDarkNessTmp10 = 60;
}
public struct BossMonsterSkillKind
{
    public const int kKingSlimeJump = 0;
    public const int kKingSlimeSpit = 1;
    public const int kKingSlimeRadiationSpit = 2;
    public const int kKingSlimeSplit = 3;
}
    public class MonsterSkillComponent : MonsterAIComponent {
    //몬스터 스킬컴포넌트들으 엄마
   
    public List<int> AcquiredSkills { get; set; }
    public int[] ActiveSkills { get; set; }
    public int SkillCount { get; set; }
    public override void Clear() { }
    public override void SetTarget(Transform target) { }
    public override void Initialize(Transform origin, Condition condition) { }
    public virtual bool UseSkill() { return true; }
    public virtual bool UseSkill(Transform target) { return true; }
    //Default Skills
    public virtual void Haste(Character.Stats monsterstats) { }      //신속한
    public virtual void Rapid(Character.Stats monsterstats) { }      //날렵한
    public virtual void Sharp(Character.Stats monsterstats) { }      //날카로운
    public virtual void Healty(Character.Stats monsterstats) { }     //건강한
    public virtual void Volant(Character.Stats monsterstats) { }     //날쌘
    public virtual void Sonic(Character.Stats monsterstats) { }      //소닉
    public virtual void SuperArmor(Character.Stats monsterstats, Transform monster) { } //슈퍼아머
    public virtual void Giant(Character.Stats monsterstats, Transform monster) { }      //거대한
    public virtual void HeadHunter(Character.Stats monsterstats, PlayerController playerController) { } //헤드헌터   
    public virtual void Smash(Character.Stats monsterstats , Transform player) { }      //강격
    public virtual void TwiceAttack(Character.Stats monsterstats) { }//난한번에 두번때려
    //Flame Skills
    public virtual void LearnFlameSkill(Character.Stats monsterStats, Transform monster) { }
    public virtual void Lava(){}
    public virtual void FlameCloak() { }
    //Water Skills 
    public virtual void LearnWaterSkill(Character.Stats monsterStats, Transform monster) { }
    public virtual void Slow() { }
    //Light Skills 
    public virtual void LearnLightSkill(Character.Stats monsterStats, Transform monster) { }
    //Nature Skills 
    public virtual void LearnNatureSkill(Character.Stats monsterStats, Transform monster) { }
    //Darkness Skills 
    public virtual void LearnDarknessSkill(Character.Stats monsterStats, Transform monster) { }

    //King Slime
    public virtual void LearnKingSlimeSkill(Character.Stats monsterStats, Transform monster) { }
    public virtual void JumpAttack() { }
    public virtual void Spit() { }
    public virtual void Split() { }
    public virtual void RadialSpit() { }

    protected bool isCanUseSkill;
    protected void CheckOverlap(int minSkill, int maxSkill) {
        // 중복검사
        bool isOverlaperd = true;
        while (isOverlaperd) {
            isOverlaperd = false;
            for (int i = 0; i < SkillCount; i++) {
                int currentSkill = SkillCount - 1;
                if (currentSkill == i) continue;
                if (this.ActiveSkills[currentSkill] == this.ActiveSkills[i]) {
                    if (Random.Range(0, 2) > 0)
                    {
                        this.ActiveSkills[currentSkill] = Random.Range(0, SkillKind.defaultSkillCount);
                    }
                    else this.ActiveSkills[currentSkill] = Random.Range(minSkill, maxSkill + 1);
                    isOverlaperd = true;
                }
            }
            if (!isOverlaperd) break;
        }
    }
}

