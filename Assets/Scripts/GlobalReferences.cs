using UnityEngine;
using UnityEngine.AI;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; private set; }
    public GameObject player;
    public GameObject stoneBulletImpact;
    public GameObject bloodSpray;


    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }


    public static void RenderOver(Transform parent)
    {
        foreach (Transform child in parent) {
            child.gameObject.layer = LayerMask.NameToLayer("RenderOver");
            if (child.childCount > 0) {
                RenderOver(child);
            }
        }
    }


    public static void RenderItem(Transform parent)
    {
        foreach (Transform child in parent) {
            child.gameObject.layer = LayerMask.NameToLayer("Items");
            if (child.childCount > 0) {
                RenderItem(child);
            }
        }
    }

    public static Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++) { // Try up to 30 times
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas)) {
                return hit.position;
            }
        }
        return center; // fallback if no valid position found
    }
}
