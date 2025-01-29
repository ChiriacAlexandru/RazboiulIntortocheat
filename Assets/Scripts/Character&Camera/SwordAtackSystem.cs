using UnityEngine;

public class SwordAtackSystem : MonoBehaviour
{
    [SerializeField]
    private float _damage = 2f;
    
    private bool _isAtacking = false;


    private void OnTriggerEnter(Collider other)
    {
        if(_isAtacking && other.CompareTag("Enemy"))
        {
            EnemyHealth _enemyHealth = other.GetComponent<EnemyHealth>();

            if (_enemyHealth != null) { 
            
                _enemyHealth.TakeDamage(_damage);
            
            }
        }
    }


}
