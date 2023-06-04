using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Camera _camera;
    private float _camOffsetZ;
    public  static Vector2 CursorPosition { get; private set; }

    private void Awake()
    {
        _camera = Camera.main;
        _camOffsetZ = _camera.transform.position.z;
    }

    private void FixedUpdate()
    {
        CursorPosition = _camera.ScreenToWorldPoint(Input.mousePosition - Vector3.one * _camOffsetZ);
    }
}