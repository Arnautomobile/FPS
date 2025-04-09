using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; private set; }

    public GameObject stoneBulletImpact;


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
}
