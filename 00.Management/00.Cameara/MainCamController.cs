using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : IUpdateableObject {
    // 초기화용
    private Transform _playerPosition;
    // 마우스 
    public static Vector3 MousePosition;
    public static float RotateDegree;
    private Vector3 _mPosition;
    private Vector3 _oPosition;
    // Ray
    private Ray _ray;
    private RaycastHit _hit;
    // 컴포넌트
    private InputComponent _inputGetKeyComponent;
    public MapIndex Index { get; set; }

    protected override void Initialize() {
        this.GetComponent<Camera>().cullingMask = ~LayerMask.GetMask("UI");

        _inputGetKeyComponent = new InputComponent(KeyKind.GETKEY);
        _inputGetKeyComponent.Bind(KeyCode.W, () => this.transform.Translate(Vector3.up    * 20.0f * Time.deltaTime));
        _inputGetKeyComponent.Bind(KeyCode.A, () => this.transform.Translate(Vector3.left  * 20.0f * Time.deltaTime));
        _inputGetKeyComponent.Bind(KeyCode.S, () => this.transform.Translate(Vector3.down  * 20.0f * Time.deltaTime));
        _inputGetKeyComponent.Bind(KeyCode.D, () => this.transform.Translate(Vector3.right * 20.0f * Time.deltaTime));
    }

    public override void OnUpdate(float dt) {
        UpdateMousePosition();
    }

    public override void OnFixedUpdate(float dt) {
        switch (DungeonMaster.Mode) {
            case GameMode.ROGUE_LIKE: ExplorationCamera();  break;
            case GameMode.PLACEMENT: case GameMode.DEFENSE: PlacementCameara(dt); break;
        }
    }

    public void Init(GameObject player) {
        _playerPosition = player.transform;
        PlayerController pc = player.GetComponent<PlayerController>();
        Index = new MapIndex(pc.Index.x, pc.Index.y);
    }
    
    private void UpdateMousePosition() {
        _mPosition = Input.mousePosition;
        _oPosition = transform.position;

        _mPosition.z = _oPosition.z - Camera.main.transform.position.z;
        
        //화면의 픽셀별로 변화되는 마우스의 좌표를 유니티의 좌표로 변화해 줘야 합니다.
        //그래야, 위치를 찾아갈 수 있겠습니다.
        MousePosition = Camera.main.ScreenToWorldPoint(_mPosition);
        float dy = MousePosition.y - _oPosition.y;
        float dx = MousePosition.x - _oPosition.x;

        RotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
    }

    private void ExplorationCamera() {
        this.transform.position =
            new Vector3(_playerPosition.position.x, _playerPosition.position.y, -100.0f);
    }
    private void PlacementCameara(float dt) {
        _inputGetKeyComponent.Run(dt);
    }
}
