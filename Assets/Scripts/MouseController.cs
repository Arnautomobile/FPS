using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private GameObject _head;

    [Header("Mouse Parameters")]
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _topClamp;
    [SerializeField] private float _bottomClamp;

    private float _xRotation = 0;
    private float _yRotation = 0;
    private bool _cursorLocked = false;


    void Start()
    {
        // We go straight into fps mode so cursor is locked
        LockCursor(true);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LockCursor(!_cursorLocked);
        }

        _yRotation += Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        _xRotation -= Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, _topClamp, _bottomClamp);

        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
        _head.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }


    private void LockCursor(bool locked)
    {
        if (locked) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
        _cursorLocked = locked;
    }
}
