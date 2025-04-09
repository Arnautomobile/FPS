using UnityEngine;

public class GrenadeScript : Throwable
{
    [SerializeField] private GameObject _explosion;

    protected override void DestroyItem()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
