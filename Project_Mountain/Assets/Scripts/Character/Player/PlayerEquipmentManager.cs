using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        //GET OUR SLOTS
        InitializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();

        LoadWeapon();
    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach(var weaponSlot in weaponSlots)
        {
            if(weaponSlot.weaponModelSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }else if(weaponSlot.weaponModelSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeapon()
    {
        if(player.playerInventoryManager.currentInHandWeapon != null)
        {
            rightHandSlot.UnloadWeapon();
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentInHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);

            leftHandSlot.UnloadWeapon();

            if (player.playerInventoryManager.currentInHandWeapon.isTwoHanded)
            {
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentInHandWeapon.weaponModel);
            }
            else
            {
                leftHandWeaponModel = Instantiate(WorldItemDatabase.instance.unarmedWeapon.weaponModel);
            }
            
            leftHandSlot.LoadWeapon(leftHandWeaponModel);

            //ASSIGN WEEAPON DAMAGE TO COLLIDER
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentInHandWeapon);

            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentInHandWeapon);
        }
    }

    public void SwitchWeapon()
    {
        //ANIMATION WHEN SWAPPING WEAPONS
        player.playerAnimatorManager.PlayTargetActionAnimation("Equip", false, true, true, true);

        WeaponItem selectedWeapon = null;
        player.playerInventoryManager.handWeaponIndex+=1;
        
        if(player.playerInventoryManager.handWeaponIndex < 0 || player.playerInventoryManager.handWeaponIndex > 2)
        {
            player.playerInventoryManager.handWeaponIndex = 0;

             //CHECK IF WE ARE HOLDSING MORE THAN ONE WEAPON
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for(int i = 0; i < player.playerInventoryManager.weaponsInHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount+=1;
                    if(firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if(weaponCount <= 1)
            {
                player.playerInventoryManager.handWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.currentInHandWeaponID = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.handWeaponIndex = firstWeaponPosition;
                player.currentInHandWeaponID = firstWeapon.itemID;
            }

            return;
        }

        foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInHandSlots)
        {
            //CHECK TO SEE IF THIS IS 'UNARMED'
            if(player.playerInventoryManager.weaponsInHandSlots[player.playerInventoryManager.handWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInHandSlots[player.playerInventoryManager.handWeaponIndex];

                player.currentInHandWeaponID = player.playerInventoryManager.weaponsInHandSlots[player.playerInventoryManager.handWeaponIndex].itemID;
                return;
            } 
        }

        if(selectedWeapon == null && player.playerInventoryManager.handWeaponIndex <= 2)
        {
            SwitchWeapon();
        }
    }

    public void OpenDamageCollider()
    {
        rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
        //PLAY WHOOSH SFX
    }

    public void CloseDamageCollider()
    {
        rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        leftWeaponManager.meleeDamageCollider.DisableDamageCollider();

    }
}
