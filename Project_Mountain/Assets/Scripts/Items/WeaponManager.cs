using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
    {
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.fireDamage = weapon.fireDamage;
        meleeDamageCollider.lightningDamage = weapon.lightningDamage;

        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;

        meleeDamageCollider.lightAttack_01Modifier = weapon.lightAttack_01Modifier;
        meleeDamageCollider.lightAttack_02Modifier = weapon.lightAttack_02Modifier;
        meleeDamageCollider.heavyAttack_01Modifier = weapon.heavyAttack_01Modifier;
        meleeDamageCollider.chargedHeavyAttack_01Modifier = weapon.chargedHeavyAttack_01Modifier;

    }
}
