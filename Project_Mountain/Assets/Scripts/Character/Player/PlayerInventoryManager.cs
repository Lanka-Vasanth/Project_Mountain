using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentInHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponsInHandSlots = new WeaponItem[3];
    public int handWeaponIndex = 0;
}
