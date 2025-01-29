using UnityEngine;

public class AtackLogics : MonoBehaviour
{
    public bool isAtacking;

    private void OnAnimationEvent(string atacking)
    {
        isAtacking = true;
    }

}
