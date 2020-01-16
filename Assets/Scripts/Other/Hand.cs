using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des déplacements éléments de l'interface
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
        // L'image a la position du curseur de la souris
        icon.transform.position = Input.mousePosition + offset;

        // Clic gauche et que l'on ne pointe pas sur un élément de l'interface (par exemple un bouton d'action) et qu'on déplace un item
        if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            // [DEBUG] : Supprime un item
            DeleteItem();
        }
    }

    /// <summary>
    /// Actualise les informations de l'objet déplaçable
    /// </summary>
    /// <param name="moveableItem"></param>
    public void TakeMoveable(IMoveable moveableItem)
    {
        // Objet déplaçable
        MyMoveable = moveableItem;

        // Image de l'objet
        icon.sprite = moveableItem.MyIcon;

        // Couleur de l'objet;
        icon.color = Color.white;
    }

    /// <summary>
    /// Assigne un objet déplaçable
    /// </summary>
    /// <returns></returns>
    public IMoveable Put()
    {
        // Objet déplaçable
        IMoveable item = MyMoveable;

        // Réinitialisation de l'objet
        MyMoveable = null;

        // Redéfinit une couleur noire transparente à l'objet
        icon.color = new Color(0, 0, 0, 0);

        // Retourne l'objet
        return item;
    }

    /// <summary>
    /// Libère l'objet déplaçable
    /// </summary>
    public void Drop()
    {
        // Réinitialisation de l'objet
        MyMoveable = null;

        // Redéfinit une couleur noire transparente à l'objet
        icon.color = new Color(0, 0, 0, 0);

        // Réinitialisation de l'emplacement
        InventoryScript.MyInstance.MyFromSlot = null;
    }

    /// <summary>
    /// Supprime un item
    /// </summary>
    public void DeleteItem()
    {
        // Si c'est un item
        if (MyMoveable is Item)
        {
            // L'objet déplacé devient un item
            Item item = MyMoveable as Item;

            // Si l'item a un emplacement dans l'inventaire
            if (item.MySlot != null)
            {
                // Vide l'emplacement
                item.MySlot.Clear();
            }
            // Si l'item a un emplacement sur la feuille du personnage
            else if (item.MyCharacterButton != null)
            {
                /// Déséquipe l'item
                item.MyCharacterButton.UnequipArmor();
            }
        }

        // Libère l'item
        Drop();

        // Réinitialisation de l'emplacement
        InventoryScript.MyInstance.MyFromSlot = null;
    }
}