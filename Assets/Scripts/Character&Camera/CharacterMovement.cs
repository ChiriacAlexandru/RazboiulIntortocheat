using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CharacterMovement : MonoBehaviour
{
    [Header("Componente")]
    [SerializeField]
    private Rigidbody rb;
   // [SerializeField]
   // private Animator animator; 
    [SerializeField]
    private Transform visualTransform;
    [SerializeField]
    private CameraScript cameraScript;

    [Header("Valori")]
    [SerializeField]
    private float walkSpeed = 5;
    [SerializeField]
    private float runSpeed = 8;
    [SerializeField]
    private float rotationSpeed = 3;


    private bool isRunning;

    private void Awake()
    {
        if(cameraScript == null)
        {
            cameraScript = GetComponent<CameraScript>();
        }


    }



    private void FixedUpdate()
    {
        moveController();
    }


    private void moveController()
    {
        Vector2 vector2 = InputController.instance.moveDirection;

        Vector3 forward = cameraScript.GetCameraForward();
        Vector3 right = cameraScript.GetCameraRight();

        Vector3 moveDirection = forward * vector2.y + right * vector2.x;
        Vector3 targetPosition = rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);




        bool isWalking = moveDirection.magnitude != 0f && !isRunning;

        
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * rotationSpeed
            );



        }
    }


}
