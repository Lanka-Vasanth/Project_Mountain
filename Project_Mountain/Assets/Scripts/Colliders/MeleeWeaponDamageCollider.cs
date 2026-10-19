using System;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    [Header("Weapon Attack Modifiers")]
    public float lightAttack_01Modifier;

    protected override void Awake()
    {
        base.Awake();

        if(damageCollider == null){
           damageCollider = GetComponent<Collider>();
        }

        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            if(damageTarget == characterCausingDamage)
            {
                return;
            }

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //CHECK IF WE CAN DAMAGE THIS TARGET

            //CHECK IF TARGET IS BLOCKING 

            //CHECK IF TARGET IS INVULNERABILITY

            //DAMAGE TARGET
            DamageTarget(damageTarget);
        }
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
        {
            return;
        }

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.contactPoint = contactPoint;

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(lightAttack_01Modifier, damageEffect);
                break;
            default:
                break;
        }


        // damageTarget.characterEffectsManager.ProcessInstantEffects(damageEffect);
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
    {
        damage.physicalDamage *= modifier;
        damage.fireDamage *= modifier;
        damage.lightningDamage *= modifier;
        damage.poiseDamage *= modifier;

    }
}
