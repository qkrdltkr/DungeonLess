using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessage : MonoBehaviour {
    public void Send(string msg, Color color, Vector3 pos, Sprite sprite = null) {
        this.gameObject.SetActive(true);
        this.transform.parent = null;

        this.transform.position = new Vector3(pos.x, pos.y, ObjectSortLevel.kUI);
        TextMesh tm = this.GetComponent<TextMesh>();
        tm.text = msg;   tm.color = color;
        if(sprite) this.GetComponentInChildren<SpriteRenderer>().sprite = sprite;        

        StartCoroutine(Send(tm));
    }
    
    private IEnumerator Send(TextMesh tm) {
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * 10.0f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        for (int i = 10; i >= 0; --i) {
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, i * 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }

        MessagePoolManager.Instance.Reload(this.gameObject);
    }
}
