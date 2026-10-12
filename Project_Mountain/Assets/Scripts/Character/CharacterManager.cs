using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterController characterController;
    public CharacterEffectsManager characterEffectsManager;
    public Animator animator;

    [Header("Status")]
    public bool isDead;
    
    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isGrounded = true;
    public bool isSprinting = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnStaminaChanged;
    public event Action<int, int> OnVitalityChanged;
    public event Action<int, int> OnEnduranceChanged;
    
    [Header("Resources")]
    public int maxHealth = 0;
    private float _currentHealth;
    public float currentHealth
    {
        get =>_currentHealth;
        set
        {
            if (_currentHealth == value)
                return;

            float oldValue = _currentHealth;
            _currentHealth = value;
            OnHealthChanged?.Invoke(oldValue, _currentHealth);
        }
    }

    public int maxStamina = 0;
    private float _currentStamina;
    public float currentStamina
    {
        get =>_currentStamina;
        set
        {
            if (_currentStamina == value)
                return;

            float oldValue = _currentStamina;
            _currentStamina = value;
            OnStaminaChanged?.Invoke(oldValue, _currentStamina);
        }
    }

    [Header("Stats")]
    private int _vitality = 10;
    public int vitality
    {
        get =>_vitality;
        set
        {
            if (_vitality == value)
                return;

            int oldValue = _vitality;
            _vitality = value;
            OnVitalityChanged?.Invoke(oldValue, _vitality);
        }
    }
    private int _endurance = 10;
    public int endurance
    {
        get =>_endurance;
        set
        {
            if (_endurance == value)
                return;

            int oldValue = _endurance;
            _endurance = value;
            OnEnduranceChanged?.Invoke(oldValue, _endurance);
        }
    }

    
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    protected virtual void LateUpdate()
    {
        
    }
}
