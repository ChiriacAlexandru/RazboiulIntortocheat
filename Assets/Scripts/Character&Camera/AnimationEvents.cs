using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public bool isAtacking {  get;private set; }
private void OnAnimationEventActivateSwordCollision()
    {
        isAtacking = true;
    }
    private void OnAnimationEventDeactivateSwordCollision()
    {
        isAtacking = false;

    }
}
