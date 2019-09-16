using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxMapCamController : MapCamController {
    private Vector3 MouseStart;

    public void EnableCam() {
        this.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _camera.orthographicSize = 15.0f;
        _camera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    }
    
    public void DisableCam() {
        _camera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public override void OnUpdate(float dt) {
        if (UIManager.IsMaxMapOn) {
            if (Input.GetMouseButtonDown(0)) {
                MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                MouseStart = Camera.main.ScreenToWorldPoint(MouseStart);
                MouseStart.z = transform.position.z;

            } else if (Input.GetMouseButton(0)) {
                var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                MouseMove = Camera.main.ScreenToWorldPoint(MouseMove);
                MouseMove.z = transform.position.z;

                transform.position = transform.position - (MouseMove - MouseStart) * Time.deltaTime * 3.0f;
            } else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                _camera.orthographicSize += 0.5f;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                if (_camera.orthographicSize <= 0.5) {
                    _camera.orthographicSize = 0.5f;
                    return;
                }
                    _camera.orthographicSize -= 0.5f;
            }
        }
    }
}
