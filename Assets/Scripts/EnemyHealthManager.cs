using UnityEngine;

public class EnemyHealthManager : MonoBehaviour, IDamageable
{
    [SerializeField] private int _startHealth;

    private EnemyMovement _enemyMovement;
    private int _currentHealth;

    void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _currentHealth = _startHealth;
    }

    
    public void Damage(int amount)
    {
        if (_currentHealth <= 0) return;
        _currentHealth -= amount;

        if (_currentHealth < 0) {
            _enemyMovement.Die();
        }
        else {
            _enemyMovement.Hit();
        }
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
    }
}
