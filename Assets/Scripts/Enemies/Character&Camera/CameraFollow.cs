using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Setări")]
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Vector3 _offset = new Vector3(0, 10, -10); // Offset pentru vedere top-down
    [SerializeField] private float _cameraFollowSpeed = 5f;

    private void LateUpdate()
    {
        if (_playerPosition == null)
        {
            Debug.LogWarning("Player position is not set!");
            return;
        }

        // Calculează poziția țintă a camerei
        Vector3 targetPosition = _playerPosition.position + _offset;

        // Interpolează poziția camerei pentru a urmări jucătorul
        transform.position = Vector3.Lerp(transform.position, targetPosition, _cameraFollowSpeed * Time.deltaTime);

        // Menține camera orientată spre jucător
        transform.LookAt(_playerPosition);
    }
}