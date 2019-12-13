using UnityEngine;



/// <summary>
/// Classe pour utiliser des sorts
/// </summary>
public class Spell : MonoBehaviour
{
    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Référence sur le rigidbody du sort
    private Rigidbody2D myRigidbody;

    // Dégâts du sort
    private int damage;

    // Propriété d'accès à la cible du sort
    public Transform MyTarget { get; private set; }

    // Source du sort
    public Transform source;



    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence sur le rigidbody du sort
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Initialisation des données du sort
    /// </summary>
    /// <param name="spellTarget">Cible du sort</param>
    /// <param name="spellDamage">Dégâts du sort</param>
    public void Initialize(Transform spellTarget, int spellDamage, Transform spellSource)
    {
        // Initialisation de la cible du sort
        MyTarget = spellTarget;

        // Initialisation des dégâts du sort
        damage = spellDamage;

        // Initialisation de la source du sort
        source = spellSource;
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
    /// Détection de l'impact
    /// </summary>
    /// <param name="collision">L'objet de collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la collision de la cible a le tag Hitbox
        if (collision.CompareTag("HitBox") && collision.transform == MyTarget)
        {
            // Source de l'attaque
            Character character = collision.GetComponentInParent<Character>();

            // Stoppe le déplacement du sort
            speed = 0;

            // Appelle la fonction de dégats sur le personnage
            character.TakeDamage(damage, source);

            // Activation du trigger "impact"
            GetComponent<Animator>().SetTrigger("impact");

            // Arrêt du mouvement du sort
            myRigidbody.velocity = Vector2.zero;

            // Réinitialise la cible du sort
            MyTarget = null;
        }
    }
}