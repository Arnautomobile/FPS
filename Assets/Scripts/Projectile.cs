using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

    protected Rigidbody _rigidbody;
    protected Vector3 _direction;
    protected Vector3 _startingPosition;
    protected float _damage;
    protected float _range;
    protected float _speed;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startingPosition = transform.position;
        Destroy(gameObject, _lifeTime);
    }

    public void Setup(Vector3 direction, float damage, float range, float speed)
    {
        _direction = direction;
        _damage = damage;
        _range = range;
        _speed = speed;
    }
}
