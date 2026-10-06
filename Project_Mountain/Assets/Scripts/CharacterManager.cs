using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterController characterController;
    public Animator animator;
    
    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isSprinting = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    public event Action<float, float> OnStaminaChanged;
    [Header("Stats")]
    public int endurance = 10;
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
    

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void LateUpdate()
    {
        
    }
}
