using UnityEngine;



/// <summary>
/// Classe de gestion de l'inventaire
/// </summary>
public class Inventory : MonoBehaviour
{
    // Instance de classe (singleton)
    private static Inventory instance;

    // Propriété d'accès à l'instance
    public static Inventory MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type Inventory (doit être unique)
                instance = FindObjectOfType<Inventory>();
            }

            return instance;
        }
    }

    // [DEBUG] : Tableau des items de l'inventaire
    [SerializeField]
    private Item[] items;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Création d'un sac
        Pocket bag = (Pocket)Instantiate(items[0]);

        // Initialisation du sac
        bag.Initialize(16);

        // Ajoute un sac à l'inventaire
        bag.Use();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {

    }
}