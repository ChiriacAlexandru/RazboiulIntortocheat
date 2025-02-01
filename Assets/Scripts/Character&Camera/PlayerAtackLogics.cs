using UnityEngine;

public class PlayerAtackLogics : MonoBehaviour
{
    [SerializeField] private AnimationEvents animationEvents; // Referință la scriptul de evenimente de animație
    [SerializeField] private GameObject swordMesh; // Referință la obiectul sabiei

    private BoxCollider _swordCollider; // Componenta BoxCollider a sabiei
    private bool _isAtacking; // Starea curentă a atacului

    private void Start()
    {
        // Obține componenta BoxCollider de pe sabie
        if (swordMesh != null && swordMesh.TryGetComponent(out _swordCollider))
        {
            _swordCollider.enabled = false; // Dezactivează coliziunea la început
        }
        else
        {
            Debug.LogError("swordMesh nu are un BoxCollider atașat sau nu este setat în Inspector!");
        }
    }

    private void Update()
    {
        UpdateAtackingState(); // Actualizează starea atacului
        UpdateCollisionState(); // Actualizează starea coliziunii sabiei
    }

    private void UpdateAtackingState()
    {
        if (animationEvents != null)
        {
            _isAtacking = animationEvents.isAtacking; // Actualizează starea atacului
        }
        else
        {
            Debug.LogError("animationEvents nu este setat în Inspector!");
        }
    }

    private void UpdateCollisionState()
    {
        if (_swordCollider != null)
        {
            _swordCollider.enabled = _isAtacking; // Activează sau dezactivează coliziunea
        }
    }



}