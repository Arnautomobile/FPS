using UnityEngine;

public class Ammunition : MonoBehaviour, IPickable
{
    [SerializeField] private GameObject _visible;
    [SerializeField] private WeaponType _type;
    [SerializeField] private int _quantity;
    

    public void Pickup(GameObject player)
    {
        if (_visible != null) {
            _quantity = player.GetComponent<WeaponsManager>().AddAmmo(_type, _quantity);
            if (_quantity <= 0) {
                Destroy(_visible);
                RemoveOutline();
            }
        }
    }

    public void Drop()
    {
        Destroy(gameObject);
    }

    public void Outline()
    {
        GetComponent<Outline>().enabled = true;
    }

    public void RemoveOutline()
    {
        GetComponent<Outline>().enabled = false;
    }

    public bool IsPicked()
    {
        return _visible == null;
    }
}
