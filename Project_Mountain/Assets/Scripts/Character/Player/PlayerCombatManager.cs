using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItem currentWeaponInUse;

    [Header("Flags")]
    public bool canCombo = false;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        if(currentWeaponInUse == null)
        {
            return;
        }

        float staminaDeducted = 0;

        switch (currentAttackType)
        { 
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponInUse.baseStaminaCost * currentWeaponInUse.lightAttackStaminaCostModifier;
                break;
            case AttackType.LightAttack02:
                staminaDeducted = currentWeaponInUse.baseStaminaCost * currentWeaponInUse.lightAttackStaminaCostModifier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponInUse.baseStaminaCost * currentWeaponInUse.heavyAttackStaminaCostModifier;
                break;
            default:
                break;
        }

        player.currentStamina -= Mathf.RoundToInt(staminaDeducted);
    }
}
 