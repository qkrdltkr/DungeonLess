using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLocation : MonoBehaviour {
    public Sprite[] _StartSprites;
    public Sprite[] _EndSprites;
    public SpriteRenderer _SpriteRenderer;

    private bool _isTeleport;

    private GameObject _weapon;
    private GameObject _player;
    private PlayerController _playerController;
    private Rigidbody _rb;

    private Vector3 _direation;

    public void Init() {
        _weapon = OperationData.Weapons.gameObject;
        _player = OperationData.Player.gameObject;
        _playerController = _player.GetComponentInChildren<PlayerController>();
        _rb = this.GetComponent<Rigidbody>();
    }

    public void Teleportation() {
        if (_isTeleport) return; // 텔레포트 중인가?
        if (_playerController.GetStats().ManaPoint < 40) return; // 마나 부족일 경우
        _playerController.GetStats().ManaPoint -= 40;

        _isTeleport = true;
        this.gameObject.SetActive(true);

        // 텔레포트 시작
        StartCoroutine(StartTeleport());

    }

    private void OnCollisionEnter(Collision target) {
        if (!target.collider.CompareTag("Wall")) return;
        // 위치 조정
        SetPosition();
    }

    private void SetPosition() {
        // 위치 설정
        _rb.isKinematic = true; _rb.isKinematic = false;

        // 텔레포트 끝
        StopAllCoroutines();
        StartCoroutine(EndTeleport());
    }

    private IEnumerator StartTeleport() {
        // 로케이션 이동
        this.transform.position = OperationData.Player.position;
        _direation = Utility.LookAt(MainCamController.MousePosition, OperationData.Player.position);
        _direation = new Vector3(_direation.x, _direation.y, 0);
        // 위치 고정
        _player.GetComponentInChildren<PlayerController>().CanMove = false;

        // 이펙트
        for (int i = 0; i < _StartSprites.Length; ++i) {
            _SpriteRenderer.sprite = _StartSprites[i];
            yield return YieldInstructionCache.WaitForSeconds(0.01f);

            if(i == 6) {
                _player.transform.localScale = Vector3.zero;
                _weapon.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            }
        }
        _SpriteRenderer.sprite = null;

        // 발사
        _rb.isKinematic = true; _rb.isKinematic = false;
        _rb.AddForce(_direation * 50000.0f);

        yield return YieldInstructionCache.WaitForSeconds(0.025f);
        SetPosition();
    }

    private IEnumerator EndTeleport() {
        // 이펙트
        for (int i = 0; i < _EndSprites.Length; ++i) {
            _SpriteRenderer.sprite = _EndSprites[i];
            yield return YieldInstructionCache.WaitForSeconds(0.01f);

            if(i == 6) {
                _player.transform.localScale = Vector3.one;
                _weapon.transform.localScale = Vector3.one;
                // 플레이어 이동
                _player.transform.position = this.transform.position;
                _weapon.transform.position = this.transform.position;
            }
        }
        // 위치 고정 해제
        _playerController.CanMove = true;

        _SpriteRenderer.sprite = null;
 
        _isTeleport = false;
        this.gameObject.SetActive(false);
    }
}
