using UnityEngine;



/// <summary>
/// Classe pour la gestion des sorts
/// </summary>
public class SpellManager : MonoBehaviour
{
    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Référence sur le rigidbody du sort
    private Rigidbody2D myRigidbody;

    // Cible du sort
    public Transform MyTarget { get; set; }


    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // Référence sur le rigidbody du sort
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// FixedUpdate : Update utilisé pour le Rigibody
    /// </summary>
    private void FixedUpdate()
    {
        // S'il y a une cible, le sort est en mouvement
        if (MyTarget != null)
        {
            // Calcule la direction du sort
            Vector2 direction = MyTarget.position - transform.position;

            // Déplace le sort en utilisant le rigidboby
            myRigidbody.velocity = direction.normalized * speed;

            // Calcule l'angle de rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Fait pivoter le sort vers la cible
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Detection de l'impact
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la collision de la cible a le tag Hitbox
        if (collision.CompareTag("HitBox") && collision.transform == MyTarget)
        {
            // Activation du trigger "impact"
            GetComponent<Animator>().SetTrigger("impact");

            // Arrêt du mouvement du spell
            myRigidbody.velocity = Vector2.zero;

            // Reset de la cible du spell
            MyTarget = null;
        }
    }
}
