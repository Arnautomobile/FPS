using UnityEngine;

public class Consummable : MonoBehaviour, IPickable
{
    [SerializeField] private ConsummableType _type;

    public void Pickup(GameObject player)
    {
        if (player.GetComponent<PickupItem>().AddConsummable(_type)) {
            Destroy(gameObject);
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
        return false;
    }
}
