using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponScript : MonoBehaviour, IPickable
{
    // use scriptable object for constant fields;
    [SerializeField] protected WeaponType _type;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _range;
    [SerializeField] private Sprite _sprite;

    protected ItemState _state = ItemState.NotPicked;
    protected WeaponsManager _weaponsManager;
    protected AudioSource _audioSource;
    protected Animator _animator;
    private Rigidbody _rigidbody;

    public WeaponType Type { get => _type; }
    public Sprite Sprite { get => _sprite; }


    protected void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public abstract void Use(Vector3 target);


    public void Pickup(GameObject player)
    {
        if (_state != ItemState.NotPicked) return;

        _weaponsManager = player.GetComponent<WeaponsManager>();
        if (!_weaponsManager.AddWeapon(this)) {
            _weaponsManager = null;
            return;
        }

        _rigidbody.isKinematic = true;
        _animator.enabled = true;
        _state = ItemState.PickedUp;
        RemoveOutline();
        GlobalReferences.RenderOver(transform);
    }

    public void Drop()
    {
        if (_state == ItemState.NotPicked) return;

        _weaponsManager = null;
        _rigidbody.isKinematic = false;
        _animator.enabled = false;
        _state = ItemState.NotPicked;
        transform.GetChild(0).localPosition = Vector3.zero;
        GlobalReferences.RenderItem(transform);
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
