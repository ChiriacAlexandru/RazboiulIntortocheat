using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Componente")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private Animator animator;

    [Header("Valori")]
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float rotationSpeed = 10; // Rotație mai rapidă

    private bool isRunning;

    private void Awake()
    {
        if (cameraScript == null)
        {
            cameraScript = FindAnyObjectByType<CameraScript>();
        }

        if (animator == null)
        {
            Debug.Log("Mesh animator != exist");
        }
    }

    private void FixedUpdate()
    {
        MoveController();
    }

    private void MoveController()
    {
        Vector2 moveInput = InputController.instance.moveDirection;
        bool sprintMove = InputController.instance.sprintMove;
        isRunning = sprintMove;

        // Calculează direcția de mișcare relativă la camera
        Vector3 forward = cameraScript.GetCameraForward();
        Vector3 right = cameraScript.GetCameraRight();
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // Alege viteza în funcție de starea de alergare
        float speed = isRunning ? runSpeed : walkSpeed;

        // Mișcă rigidbody-ul
        Vector3 targetPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // Rotirea caracterului
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            visualTransform.rotation = Quaternion.Slerp(
                visualTransform.rotation,
                targetRotation,
                Time.fixedDeltaTime * rotationSpeed);
         }

        // Animatii

        // Animații
        if (moveDirection.magnitude > 0.1f) // Folosește un prag mic pentru a evita erori de floating-point
        {
            if (isRunning)
            {
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false); // Asigură-te că nu stă în mers când aleargă
            }
            else
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false); // Asigură-te că nu aleargă când merge
            }
        }
        else
        {
            // Dacă jucătorul nu se mișcă, dezactivează ambele animații
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }
}