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
    public float lightAttack_01Modifier = 1.1f;

    //WEAPON MODIFIERS

    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostModifier = 0.9f;

    //ITEM BASED ACTIONS [SKILLS]
    [Header("Actions")]
    public WeaponItemAction r1_Action;

}
