using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyKind { NONPASS = -1, GETKEY, GETKEY_DOWN }

public class InputComponent :IComponent {
    public InputComponent(KeyKind keyKind) {
        _inputStyle = keyKind;
    }

    private Dictionary<KeyCode, Action> _keyDic = new Dictionary<KeyCode, Action>();
    public Action Idle { get; set; }

    private bool _isHolding;
    private readonly KeyKind _inputStyle;

    public void Bind(KeyCode key, Action action) {
            _keyDic.Add(key, action);
    }

    public void Run(float dt) {
        if (OperationData.IsConsoleOn) return;
        if (_inputStyle == KeyKind.GETKEY) { 
            if (Input.anyKey) {
                _isHolding = true;
                foreach (var dic in _keyDic) {
                    if (Input.GetKey(dic.Key)) dic.Value();
                }
            } else if (!Input.anyKey && _isHolding) {
                _isHolding = false;
                if(Idle != null) Idle();
            }
        } else if (_inputStyle == KeyKind.GETKEY_DOWN) {
            if (Input.anyKeyDown) {
                foreach (var dic in _keyDic) {
                    if (Input.GetKey(dic.Key)) { dic.Value(); return; }
                }
            }
        }
    }
}
