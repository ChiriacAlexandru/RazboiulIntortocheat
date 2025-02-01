using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private bool isAtacking;
private void OnAnimationEventActivateSwordCollision()
    {
        isAtacking = true;
    }
    private void OnAnimationEventDeactivateSwordCollision()
    {
        isAtacking = false;

    }
}
