using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTool : MonoBehaviour {
    public Sprite[] _CaptureEffectSprites;

    public void Play(Transform target) {
        StartCoroutine(Animate(target));
    }

    private IEnumerator Animate(Transform target) {
        MouseCursor.Instance.Set(CursorIcon.kBasic);

        // 초기화 및 위치 조절
        this.transform.position = new Vector3(
            target.transform.position.x, target.transform.position.y, ObjectSortLevel.kUI);
        this.transform.localScale = target.transform.localScale * 2.0f;
        Room room = target.parent.parent.GetComponent<Room>();
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        // 애니메이션 재생
        for (int i = 0; i < _CaptureEffectSprites.Length; ++i)  {
            sr.sprite = _CaptureEffectSprites[i];
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }
        // 몬스터 감추기
        SpriteRenderer[] srs = target.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 10; i >=0; --i) {
            for (int j = 0; j < srs.Length; ++j)            
                srs[j].color = new Color(srs[j].color.r, srs[j].color.g, srs[j].color.b, i * 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.025f);
        }
        target.gameObject.SetActive(false);
        for (int j = 0; j < srs.Length; ++j) srs[j].color = Color.white;
        
        for (int i = _CaptureEffectSprites.Length - 1; i >= 0; --i) {
            sr.sprite = _CaptureEffectSprites[i];
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }

        // 방에 몬스터 줄었다고 안내
        room.SubtractionMonsterNumber();
        // 인벤토리 추가
        target.GetComponentInChildren<Monster>().Index.Set(-1, -1);
        MonsterInventory.Instance.Add(target.gameObject);

        Destroy(this.gameObject);
    }
}
