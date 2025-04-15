using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _head;

    [Header("Main Parameters")]
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private float _patrolCooldown;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _loopingSource;
    [SerializeField] private AudioClip _screamClip;
    [SerializeField] private AudioClip _chasingClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _deadClip;
    [SerializeField] private AudioClip _hissClip;
    [SerializeField] private AudioClip _growlClip;

    [Header("Pausing Movement")]
    [SerializeField] private float _screamPause;
    [SerializeField] private float _hitPause;
    [SerializeField] private float _dyingTime;

    private Animator _animator;
    private NavMeshAgent _navAgent;
    private Vector3 _lastPlayerPosition;
    private bool _playerDetected = false;
    private float _patrolTimer = 0;
    private float _pauseTime = 0;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
        _lastPlayerPosition = transform.position;
        _loopingSource.clip = _chasingClip;
        _loopingSource.loop = true;
    }

    void Update()
    {
        GameObject player = GlobalReferences.Instance.player;
        Vector3 direction = player.transform.position - _head.position;
        float distance = direction.magnitude;
        direction.Normalize();
        Vector3 start = _head.position + direction; // offset so that zombie head doesnt block
        RaycastHit[] hits = Physics.RaycastAll(start, direction, _detectionRange);

        _playerDetected = false;
        if (hits.Length > 0 && hits[0].collider.CompareTag("Player") || distance < _attackRange) {
            _playerDetected = true;
            _lastPlayerPosition = player.transform.position;
            _navAgent.speed = _runningSpeed;
            _navAgent.angularSpeed = 180;
        }

        if (_playerDetected && !_animator.GetBool("PlayerDetected")) {
            _animator.SetBool("PlayerDetected", true);
            transform.LookAt(player.transform.position);
            _audioSource.PlayOneShot(_screamClip);
            _pauseTime = _screamPause;
        }

        if (Vector3.Distance(transform.position, _lastPlayerPosition) < _navAgent.stoppingDistance && !_playerDetected) {
            Patrol();
            _navAgent.speed = _walkingSpeed;
            _navAgent.angularSpeed = 60;

            if (_animator.GetBool("PlayerDetected")) {
                _animator.SetBool("PlayerDetected", false);
                StopLoop();
            }
        }

        if (_pauseTime > 0) {
            _pauseTime -= Time.deltaTime;
            _navAgent.destination = transform.position;
        }
        else {
            _navAgent.destination = _lastPlayerPosition;
        }

        if (_playerDetected && distance <= _attackRange) {
            _animator.SetBool("PlayerClose", true);
        } else {
            _animator.SetBool("PlayerClose", false);
        }
        if (_playerDetected && _pauseTime <= 0) {
            PlayLoop();
        }
    }


    private void Patrol()
    {
        if (_patrolTimer == 0) {
            _audioSource.PlayOneShot(_hissClip);
        }
        _patrolTimer += Time.deltaTime;
        _animator.SetBool("Walking", false);
        
        if (_patrolTimer >= _patrolCooldown) {
            _patrolTimer = 0;
            _lastPlayerPosition = GlobalReferences.GetRandomNavMeshPosition(transform.position, _patrolRadius);
            _animator.SetBool("Walking", true);
            _audioSource.PlayOneShot(_growlClip);
        }
    }


    public void Hit()
    {
        StopLoop();
        _pauseTime = _hitPause;
        _animator.SetTrigger("Hit");
        _audioSource.PlayOneShot(_hitClip);
    }

    public void Die()
    {
        StopLoop();
        _animator.SetTrigger("Die");
        _audioSource.PlayOneShot(_deadClip);
        Destroy(gameObject, _dyingTime);
        this.enabled = false;
    }


    public void PlayLoop()
    {
        if (!_loopingSource.isPlaying)
            _loopingSource.Play();
    }

    public void StopLoop()
    {
        if (_loopingSource.isPlaying)
            _loopingSource.Stop();
    }
}
