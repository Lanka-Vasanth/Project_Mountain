using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.currentWeaponInUse = weaponPerformingAction.itemID;

        Debug.Log("ACTION HAS BEEN FIRED !!!");
    }

}
