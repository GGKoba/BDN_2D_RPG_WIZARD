using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe Spell pour la gestion des sorts
/// </summary>
public class Spell : MonoBehaviour
{
    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Référence sur le rigidbody du sort
    private Rigidbody2D myRigidbody;

    // Cible du sort
    private Transform target;


    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // Référence sur le rigidbody du sort
        myRigidbody = GetComponent<Rigidbody2D>();

        // [DEBUG] : Retrouve la cible
        target = GameObject.Find("Target").transform;

    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// FixedUpdate : Update utilisé pour le Rigibody
    /// </summary>
    private void FixedUpdate()
    {
        // Calcule la direction du sort
        Vector2 direction = target.position - transform.position;

        // Déplace le sort en utilisant le rigidboby
        myRigidbody.velocity = direction.normalized * speed;

        // Calcule l'angle de rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Fait pivoter le sort vers la cible
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
