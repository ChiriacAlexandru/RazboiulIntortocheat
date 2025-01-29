using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public bool isAtacking;

    private void ActivateAtackEvent(string atacking)
    {
        isAtacking = true;
    }
    private void DeactivateAtackEvent(string notAtacking)
    {
        isAtacking = false;
    }

}
