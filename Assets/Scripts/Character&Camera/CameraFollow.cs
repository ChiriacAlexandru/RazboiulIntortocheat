using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Game Objects & Data")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private void LateUpdate()
    {
        if (player == null) return;

        // Poziție actualizată
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Rotire lină către player
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
