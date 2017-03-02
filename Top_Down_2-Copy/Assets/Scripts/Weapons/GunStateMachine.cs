using UnityEngine;
using System.Collections;

public class GunStateMachine : MonoBehaviour {

    Gun equippedGun;
    public Gun startingGun;

    public Transform weaponHold;

	void Start () {
	
        if(startingGun != null)
        {
            EquipGun(startingGun);
        }

	}
	

	void Update () {
	
	}

    public void EquipGun(Gun gunToEquip)
    {
      
        if(equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position,weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void Shoot()
    {
        //If equipped gun is available
        if(equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }
}
