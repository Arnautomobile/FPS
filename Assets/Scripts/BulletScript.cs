using UnityEngine;

public class BulletScript : Projectile
{
    void FixedUpdate()
    {
        _rigidbody.velocity = _direction * _speed;
        if (Vector3.Distance(_startingPosition, _rigidbody.position) > _range) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        IDamageable damageable = hit.GetComponent<IDamageable>();

        if (damageable != null || hit.CompareTag("Target") || hit.layer == LayerMask.NameToLayer("Ground")) {
            CreateBulletImpactEffect(collision);
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
