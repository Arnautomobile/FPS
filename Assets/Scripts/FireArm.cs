using System.Collections;
using UnityEngine;

public class FireArm : WeaponScript
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Transform _gunEndPoint;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _fireSound;
    [SerializeField] private AudioClip _emptySound;
    [SerializeField] private AudioClip _reloadSound;

    [Header("Firearm Parameters")]
    [SerializeField] private ShootingMode _shootingMode;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _spread;
    [SerializeField] private float _aimAccuracy;
    [SerializeField] private int _magSize;
    [SerializeField] private float _reloadTime;

    [Header("Burst Weapon Settings")]
    [SerializeField] private float _burstSpeed;
    [SerializeField] private int _bulletsPerBurst;


    private ParticleSystem _muzzleParticles;
    private Vector3 _target = Vector3.zero;
    private float _cooldown = 0;
    private bool _isReloading = false;
    private bool _playEmptyNoise = false;

    public int BulletsLeft { get; private set; }
    public bool Aiming { get; private set; }


    private new void Start()
    {
        base.Start();

        _muzzleParticles = _muzzleFlash.GetComponent<ParticleSystem>();
        BulletsLeft = _magSize;
        Aiming = false;
    }


    private new void Update()
    {
        base.Update();

        if (_cooldown > 0) {
            _cooldown -= Time.deltaTime;
        }
    }


    public override void Use(Vector3 target)
    {
        if (_state != ItemState.Active)
            return;

        if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < _magSize && !_isReloading) {
            int amount = _weaponsManager.RemoveAmmo(_type, _magSize - BulletsLeft);
            if (amount > 0) {
                StartCoroutine(Reload(amount));
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            _playEmptyNoise = true;
        }

        _target = target;
        Aiming = Input.GetKey(KeyCode.Mouse1);
        _animator.SetBool("AIMING", Aiming);

        bool shoot = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0) &&
                    _shootingMode == ShootingMode.Auto;

        if (shoot && !_isReloading) {
            if (BulletsLeft <= 0 && _playEmptyNoise) {
                _audioSource.PlayOneShot(_emptySound);
                _playEmptyNoise = false;
            }
            else if (_cooldown <= 0 && BulletsLeft > 0) {
                _cooldown = _fireRate;
                _playEmptyNoise = true;

                if (_shootingMode == ShootingMode.Burst) {
                    StartCoroutine(BurstFire(_bulletsPerBurst));
                }
                else {
                    Fire();
                }
            }
        }
    }


    private void Fire()
    {
        float spread = _spread;
        if (!Aiming) {
            _animator.Play("RECOIL", 0, 0f);
        }
        else {
            _animator.Play("AIMRECOIL", 0, 0f);
            spread /= _aimAccuracy;
        }
        _audioSource.PlayOneShot(_fireSound);
        _muzzleParticles.Play();

        BulletsLeft--;
        GameObject bullet = Instantiate(_bulletPrefab, _gunEndPoint.position, transform.rotation);
        bullet.GetComponent<BulletScript>().Setup(GetSpreadDirection(spread), _damage, _range, _bulletSpeed);
    }

    private Vector3 GetSpreadDirection(float spread)
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        return (_target - _gunEndPoint.position).normalized + new Vector3(x,y,0);
    }


    private IEnumerator BurstFire(int bullets)
    {
        while (bullets > 0) {
            bullets--;
            Fire();
            yield return new WaitForSeconds(_burstSpeed);
        }
    }

    private IEnumerator Reload(int amount)
    {
        _animator.SetTrigger("RELOAD");
        _audioSource.PlayOneShot(_reloadSound);
        _isReloading = true;
        yield return new WaitForSeconds(_reloadTime);
        _isReloading = false;
        BulletsLeft += amount;
    }
}
