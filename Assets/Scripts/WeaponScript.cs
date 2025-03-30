using UnityEngine;

public abstract class WeaponScript : MonoBehaviour, IPickable
{
    [SerializeField] protected float _damage;
    [SerializeField] protected float _range;
    [SerializeField] protected AmmoType _ammoType;

    protected ItemState _state = ItemState.NotPicked;
    protected WeaponsManager _weaponsManager;
    protected AudioSource _audioSource;
    protected Animator _animator;

    public AmmoType AmmoType { get => _ammoType; }


    protected void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    protected void Update()
    {
        
    }

    public abstract void Use(Vector3 target);


    public void Pickup(GameObject player)
    {
        if (_state != ItemState.NotPicked) return;

        _animator.enabled = true;
        _state = ItemState.PickedUp;
        _weaponsManager = player.GetComponent<WeaponsManager>();
        _weaponsManager.AddWeapon(this);
        
        RemoveOutline();
        Disable();
    }

    public void Drop()
    {
        if (_state == ItemState.NotPicked) return;

        _animator.enabled = false;
        _weaponsManager = null;
        _state = ItemState.NotPicked;
    }

    public void Activate()
    {
        if (_state == ItemState.PickedUp) {
            transform.localPosition = Vector3.zero;
            _state = ItemState.Active;
        }
    }

    public void Disable()
    {
        if (_state == ItemState.Active) {
            transform.localPosition = new Vector3(0,0,-2);
            _state = ItemState.PickedUp;
        }
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
        return (int)_state > 0;
    }
}
