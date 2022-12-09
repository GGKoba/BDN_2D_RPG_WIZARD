using UnityEngine;



/// <summary>
/// Classe pour utiliser des sorts
/// </summary>
public class SpellScript : MonoBehaviour
{
    // Vitesse du sort
    [SerializeField]
    private float speed = default;

    // Référence sur le rigidbody du sort
    private Rigidbody2D myRigidbody;

    // Dégâts du sort
    private float damage;

    // Débuff du sort
    private Debuff debuff;

    // Propriété d'accès à la cible du sort
    public Transform MyTarget { get; private set; }

    // Source du sort
    public Character source;



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
    public void Initialize(Transform spellTarget, float spellDamage, Character spellSource)
    {
        // Initialisation de la cible du sort
        MyTarget = spellTarget;

        // Initialisation des dégâts du sort
        damage = spellDamage;

        // Initialisation de la source du sort
        source = spellSource;
    }

    /// <summary>
    /// Initialisation des données du sort
    /// </summary>
    /// <param name="spellTarget">Cible du sort</param>
    /// <param name="spellDamage">Dégâts du sort</param>
    /// <param name="debuff">Débuff du sort</param>
    public void Initialize(Transform spellTarget, float spellDamage, Character spellSource, Debuff spellDebuff)
    {
        // Initialisation de la cible du sort
        MyTarget = spellTarget;

        // Initialisation des dégâts du sort
        damage = spellDamage;

        // Initialisation de la source du sort
        source = spellSource;

        // Initialisation du débuff du sort
        debuff = spellDebuff;
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
            // Cible de l'attaque
            Character characterAttacked = collision.GetComponentInParent<Character>();

            // Stoppe le déplacement du sort
            speed = 0;

            // Appelle la fonction de dégats sur le personnage ciblé
            characterAttacked.TakeDamage(damage, source);

            // Applique le débuff s'il existe
            if (debuff != null)
            {
                // Clone le débuff
                Debuff clone = debuff.Clone();

                // Applique le débuff
                clone.Apply(characterAttacked);
            }

            // Activation du trigger "impact"
            GetComponent<Animator>().SetTrigger("impact");

            // Arrêt du mouvement du sort
            myRigidbody.velocity = Vector2.zero;

            // Réinitialise la cible du sort
            MyTarget = null;
        }
    }
}