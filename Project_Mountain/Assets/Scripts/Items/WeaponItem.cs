using UnityEngine;

public class WeaponItem : Item
{
    //ANIMATOR CONTROLLER OVERRIDE
    [Header("Weapon Model")]
    public GameObject weaponModel;
    public bool isTwoHanded = false;

    //COMES LATER
    [Header("Weapon Requirements")]

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int fireDamage = 0;
    public int lightningDamage = 0;
    public int iceDamage = 0;
    public int poisonDamage = 0;

    [Header("Weapon Poise")]
    public float poiseDamage = 10;

    //WEAPON MODIFIERS

    [Header("Damage Modifiers")]
    public float lightAttack_01Modifier = 1.0f;
    public float lightAttack_02Modifier = 1.2f;
    public float heavyAttack_01Modifier = 1.4f;
    public float chargedHeavyAttack_01Modifier = 1.8f;

    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostModifier = 0.9f;
    public float heavyAttackStaminaCostModifier = 1.4f;

    //ITEM BASED ACTIONS [SKILLS]
    [Header("Actions")]
    public WeaponItemAction r1_Action;
    public WeaponItemAction r2_Action;


}
