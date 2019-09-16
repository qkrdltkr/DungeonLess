using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkillComponent : MonsterSkillComponent {

    private bool isSmashed = false;
    private float _stunTime = 0;
    public override void Run(float dt) {
        if (isSmashed) {
            Debug.Log("스턴시작");
            _stunTime += dt;
            if (_stunTime > 2f) {
                isSmashed = false;
                _stunTime = 0.0f;
                Debug.Log("스턴끝");
            }
        }
    }
    protected void LearnDefaultSkill(Character.Stats monsterStats, Transform monster) {
        switch (Random.Range(0, SkillKind.defaultSkillCount)) {
            //DefaultSkills - Passive
            case SkillKind.kHaste:
                Haste(monsterStats);
                AcquiredSkills.Add(SkillKind.kHaste);
                Debug.Log("신속함을 배웠습니다"); break;
            case SkillKind.kRapid:
                Rapid(monsterStats);
                AcquiredSkills.Add(SkillKind.kRapid);
                Debug.Log("날렵한을 배웠습니다"); break;
            case SkillKind.kSharp:
                Sharp(monsterStats);
                AcquiredSkills.Add(SkillKind.kSharp);
                Debug.Log("날카로움를 배웠습니다"); break;
            case SkillKind.kHealty:
                Healty(monsterStats);
                AcquiredSkills.Add(SkillKind.kHealty);
                Debug.Log("건강함을 배웠습니다"); break;
            case SkillKind.kVolant:
                Volant(monsterStats);
                AcquiredSkills.Add(SkillKind.kVolant);
                Debug.Log("날쌘을 배웠습니다"); break;
            case SkillKind.kSonic:
                Sonic(monsterStats);
                AcquiredSkills.Add(SkillKind.kSonic);
                Debug.Log("소닉을 배웠습니다"); break;
            case SkillKind.kSuperArmor:
                SuperArmor(monsterStats, monster);
                AcquiredSkills.Add(SkillKind.kSuperArmor);
                Debug.Log("슈퍼아머를 배웠습니다"); break;
            case SkillKind.kGiant:
                Giant(monsterStats, monster);
                AcquiredSkills.Add(SkillKind.kGiant);
                Debug.Log("거대한을 배웠습니다"); break;

            //DefaultSkills - Active
            case SkillKind.kHeadHunter:
                this.ActiveSkills[SkillCount] = SkillKind.kHeadHunter; this.SkillCount++;
                AcquiredSkills.Add(SkillKind.kHeadHunter);
                Debug.Log("머리노리기를 배웠습니다"); break;
            case SkillKind.kSmash:
                this.ActiveSkills[SkillCount] = SkillKind.kSmash; this.SkillCount++;
                AcquiredSkills.Add(SkillKind.kSmash);
                Debug.Log("강격을 배웠습니다"); break;
            case SkillKind.kTwiceAttack:
                this.ActiveSkills[SkillCount] = SkillKind.kTwiceAttack; this.SkillCount++;
                AcquiredSkills.Add(SkillKind.kTwiceAttack);
                Debug.Log("난 한번에 두번 때려를 배웠습니다"); break;
        }
    }
    public override void Haste(Character.Stats monsterstats) {     
        //신속한 : 이속+  
        monsterstats.MoveSpeed += 0.5f;
        monsterstats.CheckNegative();
    }
    public override void Rapid(Character.Stats monsterstats) {      
        //날렵한 : 공속, 탄속+
        monsterstats.ShootSpeed += 0.5f;
        monsterstats.BulletSpeed += 0.5f;
        monsterstats.CheckNegative();
    }
    public override void Sharp(Character.Stats monsterstats) {       
        //날카로운 : 데미지+
        monsterstats.Damage += 5;
        monsterstats.CheckNegative();
    }
    public override void Healty(Character.Stats monsterstats) {       
        //건강한 : 채력+
        monsterstats.HeatPoint += 5;
        monsterstats.CheckNegative();
    }
    public override void Volant(Character.Stats monsterstats) {       
        //날쌘 : 이속, 공속+
        monsterstats.MoveSpeed += 0.5f;
        monsterstats.ShootSpeed += 1f;
        monsterstats.CheckNegative();
    }
    public override void Sonic(Character.Stats monsterstats) {        
        //소닉 : 이속, 공속+ 공격력 감소
        monsterstats.MoveSpeed += 0.5f;
        monsterstats.ShootSpeed += 2f;
        monsterstats.Damage -= 10;
        monsterstats.CheckNegative();
    }
    public override void SuperArmor(Character.Stats monsterstats, Transform monster) { 
        //슈퍼아머
        monsterstats.HeatPoint += 2;
        monsterstats.MaxHeatPoint += 2;
        monster.GetComponent<Rigidbody>().mass += 1000;
    }
    public override void Giant(Character.Stats monsterstats, Transform monster) { 
        //거대한 : 이속,공속 - 몸크기+ 체력+
        monsterstats.MoveSpeed -= 0.5f;
        monsterstats.ShootSpeed = monsterstats.ShootSpeed * 0.5f;
        monsterstats.HeatPoint += 50;
        monsterstats.MaxHeatPoint += 50;
        monster.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        monsterstats.CheckNegative();
    }
    public override void HeadHunter(Character.Stats monsterstats, PlayerController _playerController) {   //헤드헌터   
        //나중에 플레이어 or 히어로로 바꿔야함
        //_playerController.Damaged(Vector3.zero, monsterstats.Damage * 2, StatusEffectKind.kNoStatusEffect);
        monsterstats.CheckNegative();
    }
    public override void Smash(Character.Stats monsterstats, Transform player) {  
        //강격
        //isSmashed = true;
        //_player = player;
        //targetpos = player.transform.position;
        monsterstats.CheckNegative();
    }
    public override void TwiceAttack(Character.Stats monsterstats) {
        //난한번에 두번때려
        Debug.Log(monsterstats.Damage);
       // _playerController.Damaged(Vector3.zero, monsterstats.Damage, StatusEffectKind.kNoStatusEffect);
        Debug.Log("원펀치");
       // _playerController.Damaged(Vector3.zero, monsterstats.Damage, StatusEffectKind.kNoStatusEffect);
        Debug.Log("투펀치");
        monsterstats.CheckNegative();
    }
}