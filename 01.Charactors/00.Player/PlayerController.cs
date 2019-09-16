using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character {
    public Transform Body { get; private set; }
    // Component
    private InputComponent _inputGetKeyComponent;
    private InputComponent _inputGetKeyDownComponent;
    // Attack 4 Book
    public bool _IsSpellbook { get; set; }
    private Grimoire grimoire;
    private BookIcon _spellBookIcon;
    private SpellBook _spellBook;
    private BookIcon _necronomiconIcon;
    private Necronomicon _necronomicon;
    // Item
    private ActiveItemSlot _activeItemSlot;
    // Damaged
    public GameObject _HighLight;
    private GameObject _damegedEffect;
    public SpriteRenderer _SpriteRenderer;
    private bool _isAttackPossible;
    // Teleport
    private TeleportLocation _teleport;

    private float _deltaTime;
    public bool CanMove { get; set; }

    #region Initialize
    protected override void Initialize() {
        GameObject.Find("Tools/Chair").transform.position = this.transform.position;
        _teleport = GameObject.Find("Tools").transform.Find("TeleportLocation").GetComponent<TeleportLocation>();
        Body = this.transform.parent;
        CanMove = true;
        _deltaTime = Time.deltaTime;

        InitStats();
        InitComponent();
        InitWeapon();
        InitUI();
        
        InvokeRepeating("RecoveryMana", 0.0f, 0.1f);
    }

    private void InitUI() {
        _activeItemSlot   = GameObject.Find("UI/Common/ActiveItemSlot").GetComponent<ActiveItemSlot>();
        _spellBookIcon    = GameObject.Find("UI/Common/BooksIcon/SpellBook").GetComponent<BookIcon>();
        _necronomiconIcon = GameObject.Find("UI/Common/BooksIcon/Necronomicon").GetComponent<BookIcon>();
        _damegedEffect    = GameObject.Find("UI/Common/DamegedEffect");
        _damegedEffect.SetActive(false);
    }

    private void InitStats() {
        // 스텟
        _characterStats.MoveSpeed =20.0f;

        _characterStats.BulletSpeed = 3.0f;
        _characterStats.ShootSpeed = 0.75f;
        _characterStats.Range = 3.0f;

        _characterStats.ResistancePoint = 5.0f;
        _characterStats.Damage = 50;
        _characterStats.Luck = 1.0f;

        _characterStats.MaxHeatPoint = 1000;
        _characterStats.HeatPoint = 1000;
        _characterStats.MaxManaPoint = 100.0f;
        _characterStats.ManaPoint = 100.0f;

        _characterStats.ManaRecovery = 1.0f;
        _characterStats.ManaCost = 1.0f;

        _characterStats.PushPower = 75.0f;

        _isAttackPossible = true;
    }

    private void InitWeapon() {
        // 책
        _spellBook = GameObject.Find("Weapons/SpellBook").GetComponent<SpellBook>();
        _spellBook.Init(_characterStats);
        _necronomicon = GameObject.Find("Weapons/Necronomicon").GetComponent<Necronomicon>();
        _necronomicon.Init(_characterStats);
        grimoire = _spellBook;
        _IsSpellbook = true;
        // 텔레포트
        _teleport.Init();
    }

    private void InitComponent() {
        // 애니메이션        
        _animationComponent = new PlayerAnimationComponent(this.GetComponent<Animator>(),
            new string[] { "IsIdle", "IsMove", "IsDead"});
        ((PlayerAnimationComponent)_animationComponent).Player = this.transform;
        // 인풋
        _inputGetKeyComponent = new InputComponent(KeyKind.GETKEY);
        _inputGetKeyComponent.Bind(KeyCode.W, MoveTop);
        _inputGetKeyComponent.Bind(KeyCode.A, MoveLeft);
        _inputGetKeyComponent.Bind(KeyCode.S, MoveDown);
        _inputGetKeyComponent.Bind(KeyCode.D, MoveRight);
        _inputGetKeyComponent.Bind(KeyCode.Mouse0, Attack);
        _inputGetKeyComponent.Idle = Idle;

        _inputGetKeyDownComponent = new InputComponent(KeyKind.GETKEY_DOWN);
        _inputGetKeyDownComponent.Bind(KeyCode.T, Toggle);
        _inputGetKeyDownComponent.Bind(KeyCode.Space, UseItem);
        _inputGetKeyDownComponent.Bind(KeyCode.Mouse1, Teleportation);

    }
    #endregion
    public override void OnUpdate(float dt) {
        _animationComponent.CurrentCondition = _currentCondition;
        _animationComponent.Run(dt);
    }
    public override void OnFixedUpdate(float dt) {
        if (!CanMove) return;

        _inputGetKeyComponent.Run(dt);
        _inputGetKeyDownComponent.Run(dt);
    }

    #region BindKey
    private void Attack() {
        if (UIManager.IsMaxMapOn || UIManager.IsInventory) return;
        grimoire.Attack();
    }

    private void UseItem() {
        if (UIManager.IsMaxMapOn || UIManager.IsInventory) return;
        _activeItemSlot.Use();
    }

    private void Teleportation() {
        if (UIManager.IsMaxMapOn || UIManager.IsInventory) return;
        if (DungeonMaster.Mode != GameMode.ROGUE_LIKE) return;
        _teleport.Teleportation();
    }

    private void Toggle() {
        if (UIManager.IsMaxMapOn || UIManager.IsInventory) return;
        grimoire.gameObject.SetActive(false);
        if (_IsSpellbook) {
            grimoire = _necronomicon;
            _IsSpellbook = false;

            _spellBookIcon.SwapPosition(true);
            _necronomiconIcon.SwapPosition(false);
            _necronomicon.SetMouseCursor();
        }
        else {
            grimoire = _spellBook;
            _IsSpellbook = true;

            _spellBookIcon.SwapPosition(false);
            _necronomiconIcon.SwapPosition(true);
            MouseCursor.Instance.Set(CursorIcon.kTargeting);
        }
        grimoire.gameObject.SetActive(true);

    }
    #endregion

    #region Interaction
    public void Apply(Stats stats) {
        this._characterStats.Apply(stats);
    }

    public void Placement() {
        this.enabled = false;
        Animator anim = this.GetComponent<Animator>();
        anim.SetFloat("IdlePosX", 0);
        anim.SetFloat("IdlePosY", -1);
    }

    public void Damaged(Vector3 dir, int damage, int kind) {

        if(kind != StatusEffectKind.kNoStatusEffect) {
            AddStatusEffect(kind);
        }
        if (!_isAttackPossible) return;

        Body.GetComponent<Rigidbody>().AddForce(dir * 50f);
        _characterStats.HeatPoint -= damage;

        if (_characterStats.HeatPoint <= 0) {
            _characterStats.HeatPoint = 0;
            Dead();
        }

        StartCoroutine(HitDetermination());
        StartCoroutine(DamegedEffect());
    }

    public override void Dead(){
        CanMove = false;
        OperationData.Weapons.gameObject.SetActive(false);
        _currentCondition.SetCondition(Condition.kDead);

        StartCoroutine(
            GameObject.Find("Managers/DungeonMaster").GetComponent<DungeonMaster>().EndGame());
    }

    private IEnumerator HitDetermination() {
        Color color = _SpriteRenderer.color;
        _isAttackPossible = false;

        _HighLight.SetActive(true);
        _damegedEffect.SetActive(true);
        color.a = 0.5f; _SpriteRenderer.color = color;
        yield return YieldInstructionCache.WaitForSeconds(0.25f);
        _HighLight.SetActive(false);
        color.a = 1.0f; _SpriteRenderer.color = color;

        yield return YieldInstructionCache.WaitForSeconds(0.25f);
        color.a = 0.5f; _SpriteRenderer.color = color;
        yield return YieldInstructionCache.WaitForSeconds(0.25f);
        color.a = 1.0f; _SpriteRenderer.color = color;
        yield return YieldInstructionCache.WaitForSeconds(0.25f);
        color.a = 0.5f; _SpriteRenderer.color = color;
        yield return YieldInstructionCache.WaitForSeconds(0.25f);
        color.a = 1.0f; _SpriteRenderer.color = color;

        _damegedEffect.SetActive(false);
        _isAttackPossible = true;
    }

    private IEnumerator DamegedEffect() {
        UnityEngine.UI.Image image = _damegedEffect.GetComponent<UnityEngine.UI.Image>();
        Color color = image.color;
        _damegedEffect.SetActive(true);
        for (int i = 0; i < 5; ++i) {
            color.a = i * 0.2f;
            image.color = color;
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }

        for (int i = 5; i > -1; --i) {
            color.a = i * 0.2f;
            image.color = color;
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
    }

    private void RecoveryMana() {
        _characterStats.ManaPoint += _characterStats.ManaRecovery;
        if (_characterStats.ManaPoint > _characterStats.MaxManaPoint)
            _characterStats.ManaPoint = _characterStats.MaxManaPoint;
    }

    private void MoveTop() {
        _currentCondition.SetCondition(Condition.kRun);
        Body.Translate(0.0f, _deltaTime * _characterStats.MoveSpeed, 0.0f);
    }
    private void MoveDown() {
        Body.Translate(0.0f, -_deltaTime * _characterStats.MoveSpeed, 0.0f);
        _currentCondition.SetCondition(Condition.kRun);
    }
    private void MoveLeft() {
        Body.Translate(-_deltaTime * _characterStats.MoveSpeed, 0.0f, 0.0f);
        _currentCondition.SetCondition(Condition.kRun);
    }
    private void MoveRight() {
        Body.Translate(_deltaTime * _characterStats.MoveSpeed, 0.0f, 0.0f);
        _currentCondition.SetCondition(Condition.kRun);
    }
    private void Idle(){
        _currentCondition.SetCondition(Condition.kIdle);
    }
    #endregion
}
