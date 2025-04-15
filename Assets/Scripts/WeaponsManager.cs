using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    // move this in the throwable script
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private GameObject _flashbangPrefab;
    [SerializeField] private Transform _throwablePostion;

    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private Vector3 _throwableChargedOffset;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _dropForce;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _chargeTime;

    private GameObject _activeThrowable = null;
    // put the consummable type in the inventory as public
    private ConsummableType _currentType = ConsummableType.GRENADE;
    private float _chargeTimer = 0;

    // move Selected in the inventory script, and make a separate script to controll throwables
    // Selected will be accessed by both this and the throwable script
    public Dictionary<WeaponType, int> Ammunitions { get; private set; }
    public Dictionary<ConsummableType, int> Throwables { get; private set; }
    public WeaponScript[] Weapons { get; private set; }
    public int Selected { get; private set; } = 0;


    private void Start()
    {
        Weapons = new WeaponScript[2];
        Weapons[0] = null;
        Weapons[1] = null;

        Ammunitions = new Dictionary<WeaponType, int>
        {
            { WeaponType.AR, 0 },
            { WeaponType.PISTOL, 0 },
            { WeaponType.PRECISION, 0 },
            { WeaponType.ROCKET, 0 }
        };
        Throwables = new Dictionary<ConsummableType, int>
        {
            { ConsummableType.GRENADE, 0 },
            { ConsummableType.FLASHBANG, 0 }
        };
    }

    void Update()
    {
        GetInputs();

        for (int i = 0; i < 2; i++) {
            if (Weapons[i] == null) continue;

            if (Selected == i) {
                Weapons[i].Activate();
                UseWeapon();
            }
            else {
                Weapons[i].Disable();
            }
        }

        
        if (Selected < 2 && _activeThrowable != null) {
            Destroy(_activeThrowable);
        }
        else if (Throwables[_currentType] > 0) {
            if (_activeThrowable == null) {
                SetThrowable();
            }
            else {
                UseThrowable();
            }
        }
    }


    private void GetInputs()
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
            // call this method from the inventory script with the item type as argument
            SetThrowable();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            // tactical -> if item is not throwable dont call the SetThrowable() method
            Selected = 3;
            SetThrowable();
        }
    }



    private void SetThrowable()
    {
        // add argument consummabletype to this method to then update the currentType

        if (_activeThrowable != null) {
            Destroy(_activeThrowable);
        }

        if (Selected == 2 && Throwables[ConsummableType.GRENADE] > 0) {
            _currentType = ConsummableType.GRENADE;
            _activeThrowable = Instantiate(_grenadePrefab);
        }
        if (Selected == 3 && Throwables[ConsummableType.FLASHBANG] > 0) {
            _currentType = ConsummableType.FLASHBANG;
            _activeThrowable = Instantiate(_flashbangPrefab);
        }

        if (_activeThrowable != null) {
            _activeThrowable.transform.SetParent(_throwablePostion, false);
            _activeThrowable.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            GlobalReferences.RenderOver(transform);
        }
        _chargeTimer = 0;
    }


    private void UseThrowable()
    {
        _activeThrowable.transform.localPosition = _throwableChargedOffset * (_chargeTimer/_chargeTime);
        _activeThrowable.transform.localRotation = Quaternion.identity;

        if (Input.GetKey(KeyCode.Mouse1) && _chargeTimer < _chargeTime) {
            _chargeTimer += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1)) {
            Throwables[_currentType]--;
            _activeThrowable.transform.SetParent(null);
            _activeThrowable.GetComponent<Throwable>().Throw(_camera.transform.forward, _throwForce * (_chargeTimer/_chargeTime));
            _activeThrowable = null;
        }
    }

    public void AddThrowable(ConsummableType type)
    {
        Throwables[type]++;
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


    public bool AddWeapon(WeaponScript weaponScript)
    {
        bool added = false;
        if (Selected < 2 && Weapons[Selected] == null) {
            Weapons[Selected] = weaponScript;
            added = true;
        }
        else if (Selected != 0 && Weapons[0] == null) {
            Weapons[0] = weaponScript;
            added = true;
        }
        else if (Selected != 1 && Weapons[1] == null) {
            Weapons[1] = weaponScript;
            added = true;
        }

        if (added) {
            weaponScript.transform.SetParent(_weaponPosition, false);    
            weaponScript.transform.SetLocalPositionAndRotation(new Vector3(0,0,-2), Quaternion.identity);
            return true;        
        }
        return false;
    }

    public void DropWeapon()
    {
        WeaponScript weapon = Weapons[Selected];
        if (weapon == null) return;
        
        weapon.transform.SetParent(null);
        weapon.Drop();
        weapon.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * _dropForce, ForceMode.Impulse);

        Weapons[Selected] = null;
    }


    public int AddAmmo(WeaponType type, int amount)
    {
        Ammunitions[type] += amount;
        if (Ammunitions[type] > 999) {
            amount = Ammunitions[type] - 999;
            Ammunitions[type] = 999;
            return amount;
        }
        return 0;
    }

    public int RemoveAmmo(WeaponType type, int amount)
    {
        Ammunitions[type] -= amount;
        if (Ammunitions[type] < 0) {
            amount += Ammunitions[type];
            Ammunitions[type] = 0;
        }
        return amount;
    }
}
