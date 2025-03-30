using UnityEngine;

public class AmmoBox : MonoBehaviour, IPickable
{
    [SerializeField] private GameObject _bullets;
    [SerializeField] private WeaponType _type;
    [SerializeField] private int _quantity;
    

    public void Pickup(GameObject player)
    {
        if (_bullets != null) {
            player.GetComponent<WeaponsManager>().AddAmmo(_type, _quantity);
            _quantity = 0;
            Destroy(_bullets);
        }
        RemoveOutline();
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
        return _bullets == null;
    }


    public void Activate() {}

    public void Disable() {}
}
