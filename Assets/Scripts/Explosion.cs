using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _lifetime;

    void Start()
    {
        Destroy(gameObject, _lifetime);
    }


    public void Explode(int damage, float radius)
    {
        // check for nearby colliders and deal damage
    }
}
