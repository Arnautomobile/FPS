using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

    protected Rigidbody _rigidbody;
    protected Vector3 _direction;
    protected Vector3 _startpos;
    protected int _damage;
    protected float _range;
    protected float _speed;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startpos = transform.position;
        Destroy(gameObject, _lifeTime);
    }

    public void Setup(Vector3 direction, int damage, float range, float speed)
    {
        _direction = direction;
        _damage = damage;
        _range = range;
        _speed = speed;
    }



    protected void FixedUpdate()
    {
        _rigidbody.velocity = _direction * _speed;
        if (Vector3.Distance(_startpos, _rigidbody.position) > _range) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        IDamageable damageable = hit.GetComponent<IDamageable>();

        if (hit.CompareTag("Target") || hit.layer == LayerMask.NameToLayer("Ground")) {
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        if (damageable != null) {
            damageable.Damage(_damage);
            Destroy(gameObject);
        }
    }

    private void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.stoneBulletImpact,
                           contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(collision.gameObject.transform);
    }
}
