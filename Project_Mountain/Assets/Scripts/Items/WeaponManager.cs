using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]MeleeWeaponDamageCollider meleeDamageCollider;

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
    }
}
