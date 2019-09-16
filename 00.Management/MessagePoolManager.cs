using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MessageSpriteType {
    public const int kNonpass = 0;
    public const int kRepairIcon = 1;
}

public class MessagePoolManager : Singleton<MessagePoolManager> {
    public static MessagePoolManager Instance {
        get { return (MessagePoolManager)_Instance; }
        set { _Instance = value; }
    }

    public Sprite[] _MessageSprites;
    public GameObject _MessagePrefabs;
    private List<GameObject> _messages = new List<GameObject>();

    void Start(){
        for (int i = 0; i < this.transform.childCount; ++i)
            _messages.Add(this.transform.GetChild(i).gameObject);
    }

    public void Send(string msg, Color color, Vector3 pos, int spriteType = MessageSpriteType.kNonpass) {
        GameObject message = _messages.Count == 0 ? SupplyBullet() : _messages[0];
        _messages.RemoveAt(0);
        // 기초 세팅
        message.GetComponent<UIMessage>().Send(msg, color, pos, _MessageSprites[spriteType]);
    }

    public void Reload(GameObject msg) {
        if (_messages.Contains(msg)) return;

        msg.SetActive(false);
        msg.transform.parent = this.transform;
        _messages.Add(msg);
    }

    private GameObject SupplyBullet() {
        GameObject bullet = Instantiate(_MessagePrefabs, this.transform.position, Quaternion.identity, this.transform);
        _messages.Add(bullet);
        return bullet;
    }
}
