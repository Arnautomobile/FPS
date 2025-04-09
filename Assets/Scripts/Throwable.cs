using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] protected float _lifeTime;
    private Rigidbody _rigidBody;

    
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.isKinematic = true;
    }

    public void Throw(Vector3 direction, float force)
    {
        _rigidBody.isKinematic = false;
        _rigidBody.AddForce(direction * force, ForceMode.Impulse);

        if (_lifeTime > 0) {
            Invoke(nameof(DestroyItem), _lifeTime);
        }
        GlobalReferences.RenderItem(transform);
    }


    protected abstract void DestroyItem();
}
