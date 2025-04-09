using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxPickupDistance;

    private WeaponsManager _weaponsManager;
    private GameObject _hoveredItem;


    void Start()
    {
        _weaponsManager = GetComponent<WeaponsManager>();
        _hoveredItem = null;
    }


    void Update()
    {
        // check for items that can be picked by being close to them
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        GameObject item = null;

        if (Physics.Raycast(ray, out hit, _maxPickupDistance)) {
            item = hit.transform.gameObject;

            if (item != _hoveredItem) {
                ItemUnselection();
                IPickable pickable = item.GetComponent<IPickable>();
                if (pickable != null && !pickable.IsPicked()) {
                    _hoveredItem = item;
                    pickable.Outline();
                }
            }
        }
        else {
            ItemUnselection();
        }

        if (Input.GetKeyDown(KeyCode.F) && _hoveredItem != null) {
            _hoveredItem.GetComponent<IPickable>().Pickup(gameObject);
        }
    }


    private void ItemUnselection()
    {
        if (_hoveredItem == null) return;

        _hoveredItem.GetComponent<IPickable>().RemoveOutline();
        _hoveredItem = null;
    }


    // put this function in an inventory script
    public bool AddConsummable(ConsummableType type)
    {
        if (type == ConsummableType.GRENADE || type == ConsummableType.FLASHBANG) {
            _weaponsManager.AddThrowable(type);
            return true;
        }
        return false;
    }
}
