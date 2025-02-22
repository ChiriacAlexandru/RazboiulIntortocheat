﻿using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    private string enemyTag = "Enemies"; // Tag-ul obiectelor inamice
    [SerializeField]
    private float attackRadius = 3f; // Raza de atac
    private GameObject currentEnemy; // Referință la inamicul curent
    private Animator animator; // Referință la Animator
    [SerializeField]
    private Rigidbody characterController; // Referință la CharacterController

    void Start()
    {
        // Obține referința la CharacterController de pe obiectul părinte
        characterController = GetComponentInParent<Rigidbody>();

        // Obține referința la Animator de pe obiectul curent
        animator = GetComponent<Animator>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController nu a fost găsit pe obiectul părinte!");
        }
    }

    void Update()
    {
        // Găsește cel mai apropiat inamic în raza de atac
        GameObject closestEnemy = GetClosestEnemy();

        // Verifică dacă există un inamic aproape și dacă personajul nu se mișcă
        if (closestEnemy != null && characterController.linearVelocity == Vector3.zero)
        {
            // Salvează referința la inamicul curent
            currentEnemy = closestEnemy;

            // Activează animația de atac
            animator.SetBool("IsAtacking", true);
        }
        else
        {
            // Oprește animația de atac dacă nu mai există inamici
            animator.SetBool("IsAtacking", false);
            currentEnemy = null;
        }
    }

    // Găsește cel mai apropiat inamic în raza de atac
    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Obține toate obiectele din raza de atac
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    // Opțional: vizualizare a razei de atac în editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}