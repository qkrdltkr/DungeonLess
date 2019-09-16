using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimationComponent : AnimationComponent {
    public BookAnimationComponent(Animator anim, Transform origin, string[] conditions) : base(anim) {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _originObj = origin;
        _conditions = conditions;
    }

    private enum BookDireation {
        NONPASS = -1,
        LEFT, TOP, RIGHT, BOTTOM
    }
    private Transform _player;
    private Transform _originObj;
    private BookDireation _bookDireation;

    public override void Run(float dt) {
        SetDir();
        switch (_bookDireation) {
            case BookDireation.TOP: ConvertCondition("Top"); break;
            case BookDireation.LEFT: ConvertCondition("Side"); break;
            case BookDireation.RIGHT: ConvertCondition("Side"); break;
            case BookDireation.BOTTOM: ConvertCondition("Bottom"); break;
        }
    }

    private void SetDir() {
        float degree = Utility.GetAngle(_player.position, MainCamController.MousePosition);
        if (degree > 0.0f && degree <= 45.0f || degree > 305.0f && degree <= 360.0f)
            _bookDireation = BookDireation.TOP;
        else if (degree > 45.0f && degree <= 135.0f)
            _bookDireation = BookDireation.LEFT;
        else if (degree > 135.0f && degree <= 215.0f)
            _bookDireation = BookDireation.BOTTOM;
        else if (degree > 215.0f && degree <= 305.0f)
            _bookDireation = BookDireation.RIGHT;

        if (_bookDireation == BookDireation.LEFT) {
            _originObj.localScale = new Vector3(1.0f, -1.0f, 1.0f) * 1.5f;
            _originObj.position = new Vector3(
                _originObj.position.x,
                _originObj.position.y,
                ObjectSortLevel.kWeapon);
        } else if (_bookDireation == BookDireation.RIGHT || _bookDireation == BookDireation.BOTTOM) {
            _originObj.localScale = Vector3.one * 1.5f;
            _originObj.position = new Vector3(
                _originObj.position.x,
                _originObj.position.y,
                ObjectSortLevel.kWeapon);
        }
        else if (_bookDireation == BookDireation.TOP) {
            _originObj.localScale = Vector3.one * 1.5f;
            _originObj.position = new Vector3(
                _originObj.position.x,
                _originObj.position.y,
                ObjectSortLevel.kCharacter + 1);
        }
    }
}
