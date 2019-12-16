using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des déplacements élements de l'interface
/// </summary>
public class Hand : MonoBehaviour
{
    // Instance de classe (singleton)
    private static Hand instance;

    // Propriété d'accès à l'instance
    public static Hand MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type Hand (doit être unique)
                instance = FindObjectOfType<Hand>();
            }

            return instance;
        }
    }

    // Propriété d'accès à l'ibjet déplaçable
    public IMoveable MyMoveable { get; set; }

    // Image de l'objet déplaçable
    private Image icon;

    // Décalage de l'image avec le curseur
    [SerializeField]
    private Vector3 offset = default;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence sur l'image de l'objet déplaçable
        icon = GetComponent<Image>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // L'icône a la position du curseur de la souris
        icon.transform.position = Input.mousePosition + offset;
    }


    public void TakeMoveable(IMoveable moveableItem)
    {
        // Item déplaçable
        MyMoveable = moveableItem;

        // Image de l'item
        icon.sprite = moveableItem.MyIcon;

        // Couleur de l'item;
        icon.color = Color.white;
    }
}