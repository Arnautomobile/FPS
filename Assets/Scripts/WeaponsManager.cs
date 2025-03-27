using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Camera _camera;

    private WeaponScript weaponScript;

    void Start()
    {
        weaponScript = _weapon.GetComponent<WeaponScript>();
    }


    void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit)){
            targetPoint = hit.point;
        }
        else {
            targetPoint = ray.GetPoint(200);
        }

        weaponScript.PlayerUse(targetPoint);
    }
}
