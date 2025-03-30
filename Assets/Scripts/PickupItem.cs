using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private WeaponsManager _weaponsManager;
    private GameObject _hoveredItem;


    void Start()
    {
        _weaponsManager = GetComponent<WeaponsManager>();
        _hoveredItem = null;
    }


    void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        GameObject item = null;

        if (Physics.Raycast(ray, out hit)) {
            item = hit.transform.gameObject;

            if (item != _hoveredItem) {
                IPickable pickable;
                if ((pickable = item.GetComponent<IPickable>()) != null) {
                    _hoveredItem = item;
                    pickable.Outline();
                }
            }
        }

        if (item != _hoveredItem && _hoveredItem != null) {
            _hoveredItem.GetComponent<IPickable>().RemoveOutline();
            _hoveredItem = null;
        }


        if (Input.GetKeyDown(KeyCode.F) && _hoveredItem != null) {
            WeaponScript weaponScript;
            if ((weaponScript = _hoveredItem.GetComponent<WeaponScript>()) != null) {
                _weaponsManager.AddWeapon(weaponScript);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            _weaponsManager.DropWeapon();
        }
    }
}
