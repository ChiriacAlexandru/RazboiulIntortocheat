using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Game Objects & Data")]

    [SerializeField]
    private Transform _playerPosition;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 0, 0);
    [SerializeField]
    private float _cameraFollowSpeed = 5f;


    //Target Location

    private Vector3 _targetPosition;

    private void Start()
    {
        _offset = transform.position - _playerPosition.position;
    }

    private void FixedUpdate()
    {
        if (_playerPosition.position != null)
        {

            _targetPosition = _playerPosition.position + _playerPosition.forward * _offset.z + _playerPosition.up * _offset.y + _playerPosition.right * _offset.x;


            transform.position = Vector3.Lerp(transform.position, _targetPosition, _cameraFollowSpeed * Time.fixedDeltaTime);

        }
        else
        {
            Debug.Log("_playerPosition = null");
        }
    }
}