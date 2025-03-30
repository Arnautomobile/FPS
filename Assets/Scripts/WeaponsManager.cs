using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private Camera _camera;

    public Dictionary<AmmoType, int> Ammunitions { get; private set; }
    public WeaponScript[] Weapons { get; private set; }
    public int Selected { get; private set; }


    private void Start()
    {
        Weapons = new WeaponScript[3];
        Weapons[0] = null;
        Weapons[1] = null;
        Weapons[2] = null;
        Selected = 0;

        Ammunitions = new Dictionary<AmmoType, int>
        {
            { AmmoType.AR, 0 },
            { AmmoType.PISTOL, 0 },
            { AmmoType.PRECISION, 0 },
            { AmmoType.ROCKET, 0 }
        };
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            DropWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            Selected = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Selected = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Selected = 2;
        }

        for (int i = 0; i < 3; i++) {
            if (Weapons[i] == null) continue;

            if (i == Selected) {
                Weapons[i].Activate();
            }
            else {
                Weapons[i].Disable();
            }
        }

        if (Weapons[Selected] != null) {
            UseWeapon();
        }
    }


    private void UseWeapon()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit)) {
            targetPoint = hit.point;
        }
        else {
            targetPoint = ray.GetPoint(200);
        }
        Weapons[Selected].Use(targetPoint);
    }


    public void AddWeapon(WeaponScript weaponScript)
    {
        bool added = false;
        if (Weapons[Selected] == null) {
            Weapons[Selected] = weaponScript;
            added = true;
        }
        else {
            for (int i = 0; i < 3; i++) {
                if (Weapons[i] == null) {
                    Weapons[i] = weaponScript;
                    added = true;
                    break;
                }
            }
        }

        if (added) {
            weaponScript.transform.SetParent(_weaponPosition, false);    
            weaponScript.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);        
        }
    }

    public void DropWeapon()
    {
        if (Weapons[Selected] == null) return;
        
        Weapons[Selected].transform.SetParent(null);
        Weapons[Selected].Drop();
        Weapons[Selected] = null;
    }


    public void AddAmmo(AmmoType type, int amount)
    {
        Ammunitions[type] += amount;
        if (Ammunitions[type] > 999) {
            Ammunitions[type] = 999;
        }
    }

    public int RemoveAmmo(AmmoType type, int amount)
    {
        Ammunitions[type] -= amount;
        if (Ammunitions[type] < 0) {
            amount += Ammunitions[type];
            Ammunitions[type] = 0;
        }
        return amount;
    }
}
