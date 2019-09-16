using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionEffect : MonoBehaviour {
    public Transform _Bullet;
    public Animator _Animator;

    public void StartEffect() {
        StopAllCoroutines();
        this.gameObject.SetActive(true);
        this.transform.parent = null;
        _Animator.SetFloat("Collision", 1.0f);
        _Animator.SetFloat("Idle", 0.0f);
        StartCoroutine(DisalbeEffect());
    }

    private IEnumerator DisalbeEffect() {
        yield return YieldInstructionCache.WaitForSeconds(
            _Animator.GetCurrentAnimatorStateInfo(0).length);

        this.transform.parent = _Bullet;
        this.transform.position = _Bullet.position;
        this.gameObject.SetActive(false);
    }
}
