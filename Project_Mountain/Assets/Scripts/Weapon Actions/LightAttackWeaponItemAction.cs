using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string lightAttack_01 = "Main_Light_Attack_01";

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
        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, lightAttack_01, true);
    }
}
