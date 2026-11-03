using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage Effect")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    //BUILDUPS
    // public float poisonDamage = 0;
    // public float bleedDamage = 0;
    // public float iceDamage = 0;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalSFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (character.isDead)
        {
            return;
        }

        //CHECK INVULNERABILITY

        //CALCUKATE DAMAGE
        CalculateDamage(character);

        //CHECL DAMAGE DIRECTION

        //PLAY DAMAGE ANIMATION
        PlayDirectionalDamageAnimation(character);

        //CHECK FOR BUILDUP (POISON, BLEED, ICE ETC)

        //PLAY DAMAGE VFX, SFX
        PlayDamageVFX(character);
        // PlayDamageSFX(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if(characterCausingDamage != null)
        {
            //CHECK FOR DAMAGE MODIFIERS, TO MODIFY BASE DAMAGE
        }

        //CHECK FOR ARMOR ABSORPTIONS< SUBTRACT FROM BASE DAMAGE

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + fireDamage + lightningDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }
        
        character.currentHealth -= finalDamageDealt;

        //CALCULATE POISE DAMAGE, TO DETERMINE IF CHARACTER IS STUNNED 
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatter(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
    }

    private void PlayDirectionalDamageAnimation(CharacterManager character)
    {
        //CALCULATE IF POISE IS BROKEN
        poiseIsBroken = true;

        if (character.isDead)
        {
            return;
        }

        if (angleHitFrom >= 145 && angleHitFrom <= 180){
            //PLAY FRONTAL DAMAGE ANIMATION
            // damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Damage);
            damageAnimation = character.characterAnimatorManager.hitForward01;
        }else if(angleHitFrom <= -145 && angleHitFrom >= -180){
            //PLAY FRONTAL DAMAGE ANIM
            // damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Damage)
            damageAnimation = character.characterAnimatorManager.hitForward01;
        }else if (angleHitFrom >= -45 && angleHitFrom <= 45){
            //PLAY DAMAGE FROM BACK ANIM
            // damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Damage)
            damageAnimation = character.characterAnimatorManager.hitBackward01;
        }else if(angleHitFrom >= -145 && angleHitFrom <= -45){
            // PLAY DAMAGE FROM LEFT ANIM
            // damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Damage)
            damageAnimation = character.characterAnimatorManager.hitLeftward01;
        }else if(angleHitFrom >= 45 && angleHitFrom <= 144){
            //PLAY DAMAGE FROM RIGHT ANIM.
            // damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Damage)
            damageAnimation = character.characterAnimatorManager.hitRightward01;
        }

        //IF POISE IS BROKEN, PLAY STAGGERRED ANIMATION
        if (poiseIsBroken)
        {
            character.characterAnimatorManager.lastAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }
}
