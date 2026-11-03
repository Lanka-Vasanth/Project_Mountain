using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string heavyAttack_01 = "Main_Heavy_Attack_01";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        //CHECK FOR STOPS

        if(playerPerformingAction.currentStamina <= 0)
        {
            return;
        }

        if (!playerPerformingAction.isGrounded)
        { 
            return;
        }

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction,  WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavyAttack_01, true);
    }
}
