using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    protected Collider damageCollider;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;

    [Header("ContactPoint")]
    protected Vector3 contactPoint;

    [Header("Characters Dmaaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();
    
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //CHECK IF WE CAN DAMAGE THIS TARGET

            //CHECK IF TARGET IS BLOCKING 

            //CHECK IF TARGET IS INVULNERABILITY

            //DAMAGE TARGET
            DamageTarget(damageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
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

        damageTarget.characterEffectsManager.ProcessInstantEffects(damageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear();
    }

}
