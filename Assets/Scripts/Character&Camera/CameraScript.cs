using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 GetCameraForward()
    {
        Vector3 forwardDir = transform.forward;
        forwardDir.y = 0;
        return forwardDir.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 rightDir = transform.right;
        rightDir.y = 0;
        return rightDir.normalized;
    }
}
