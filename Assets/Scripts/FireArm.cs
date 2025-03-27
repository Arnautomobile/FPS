using System.Collections;
using UnityEngine;

public class FireArm : WeaponScript
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _gunEndPoint;
    [SerializeField] private ShootingModeEnum _shootingMode;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _spread;
    [SerializeField] private float _burstSpeed;
    [SerializeField] private int _bulletsPerBurst;

    private Vector3 _target = Vector3.zero;
    private float _cooldown = 0;

    void Update()
    {
        if (_cooldown > 0) {
            _cooldown -= Time.deltaTime;
        }
    }


    public override void PlayerUse(Vector3 target)
    {
        if (!Active || _ownerType != OwnerType.Player) return;

        _target = target;
        bool shoot = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0) && _shootingMode == ShootingModeEnum.Auto;

        if (shoot && _cooldown <= 0) {
            _cooldown = _fireRate;

            if (_shootingMode == ShootingModeEnum.Burst) {
                StartCoroutine(BurstFire(_bulletsPerBurst));
            }
            else {
                Fire();
            }
        }
    }


    public override void EnemyUse(Vector3 target)
    {
        if (!Active || _ownerType != OwnerType.Enemy) return;


    }


    private Vector3 GetSpreadDirection()
    {
        float x = Random.Range(-_spread, _spread);
        float y = Random.Range(-_spread, _spread);
        return (_target - _gunEndPoint.position).normalized + new Vector3(x,y,0);
    }


    private void Fire()
    {
        if (!Active) return;

        GameObject bullet = Instantiate(_bulletPrefab, _gunEndPoint.position, transform.rotation);
        bullet.GetComponent<Projectile>().Setup(GetSpreadDirection(), _damage, _range, _bulletSpeed);
    }


    private IEnumerator BurstFire(int bullets)
    {
        while (bullets > 0) {
            bullets--;
            Fire();
            yield return new WaitForSeconds(_burstSpeed);
        }
    }
}
