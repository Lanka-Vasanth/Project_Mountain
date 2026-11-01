using UnityEngine;

public class AIDummyCombatManager : AICharacterCombatManager
{
    [Header("DamageCollider")]
    [SerializeField] DummyDamageCollider rightHandDamageCollider;
    [SerializeField] DummyDamageCollider leftHandDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.5f;

    public void SetAttack01Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage*attack01DamageModifier;
        leftHandDamageCollider.physicalDamage = baseDamage*attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
        leftHandDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
    }

    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
}
