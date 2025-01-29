using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Componente")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private CameraScript cameraScript;

    [Header("Valori")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    private bool isRunning;
    private Vector3 moveDirection;

    private void Start()
    {
        if (cameraScript == null)
        {
            cameraScript = FindAnyObjectByType<CameraScript>();
        }
    }

    private void FixedUpdate()
    {
        MoveController();
    }

    private void MoveController()
    {
        Vector2 inputVector = InputController.instance.moveDirection;
        if (inputVector == Vector2.zero) return;

        Vector3 forward = cameraScript.GetCameraForward();
        Vector3 right = cameraScript.GetCameraRight();

        moveDirection = (forward * inputVector.y + right * inputVector.x).normalized;
        rb.MovePosition(rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime);

        // Rotire doar dacă există mișcare
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}
