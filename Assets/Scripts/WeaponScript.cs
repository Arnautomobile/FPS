using UnityEngine;


public abstract class WeaponScript : MonoBehaviour, IPickable
{
    [SerializeField] protected float _damage;
    [SerializeField] protected float _range;

    protected OwnerType _ownerType = OwnerType.Player;
    public bool Active { get; set; } = true;

    public abstract void PlayerUse(Vector3 target);
    public abstract void EnemyUse(Vector3 target);


    public void Pickup(OwnerType owner) {
        _ownerType = owner;
    }

    public void Drop() {
        _ownerType = OwnerType.NoOwner;
    }

    public bool Picked() {
        return (int)_ownerType > 0;
    }
}
