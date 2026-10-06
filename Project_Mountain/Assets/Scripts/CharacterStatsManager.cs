using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina Regeneration")]
    [SerializeField] int staminaRegenAmount = 2;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;
        
        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegerateStamina()
    {
        if (character.isSprinting)
        {
            return;
        }

        if (character.isPerformingAction)
        {
            return;
        }

        staminaRegenerationTimer += Time.deltaTime;
        if(staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if(character.currentStamina < character.maxStamina)
            {
                staminaTickTimer += Time.deltaTime;
                if(staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.currentStamina = Mathf.Min(character.currentStamina + staminaRegenAmount, character.maxStamina);
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        if(currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
