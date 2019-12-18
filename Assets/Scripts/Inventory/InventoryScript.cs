using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion de l'inventaire
/// </summary>
public class InventoryScript : MonoBehaviour
{
    // Instance de classe (singleton)
    private static InventoryScript instance;

    // Propriété d'accès à l'instance
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type Inventory (doit être unique)
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    // Liste des sacs de l'inventaire
    private List<Bag> bags = new List<Bag>();

    // Tableau des boutons des sacs
    [SerializeField]
    private BagButton[] bagButtons = default;

    // [DEBUG] : Tableau des items de l'inventaire
    [SerializeField]
    private Item[] items = default;

    // Propriété d'ajout des sacs
    public bool CanAddBag { get => bags.Count < bagButtons.Length; }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Création d'un sac
        Bag bag = (Bag)Instantiate(items[0]);

        // Initialisation du sac
        bag.Initialize(20);

        // Ajoute un sac à l'inventaire
        bag.Use();
    }

    /// <summary>
    /// [DEBUG] : Update
    /// </summary>
    private void Update()
    {
        // Ajoute un sac avec la touche J
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Création d'un sac
            Bag bag = (Bag)Instantiate(items[0]);

            // Initialisation du sac
            bag.Initialize(20);

            // Ajoute un sac dans l'inventaire
            bag.Use();
        }

        // Ajoute un item avec la touche K
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Création d'un Item sac
            Bag bag = (Bag)Instantiate(items[0]);

            // Initialisation du sac
            bag.Initialize(2);

            // Ajoute l'item Sac dans un sac de l'inventaire
            AddItem(bag);
        }

    }

    /// <summary>
    /// Ajoute un sac à l'inventaire
    /// </summary>
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            // S'il n'y a pas de sacs sur le bouton
            if (bagButton.MyBag == null)
            {
                // Assignation du sac
                bagButton.MyBag = bag;

                // Ajoute le sac dans la liste
                bags.Add(bag);

                break;
            }
        }
    }

    /// <summary>
    /// Ouverture/Fermeture de tous les sacs de l'inventaire
    /// </summary>
    public void OpenClose()
    {
        // Y a-t-il au moins un sac fermé ?
        bool closedBag = bags.Find(bag => !bag.MyBagScript.IsOpen);

        // Ouverture de tous les sacs
        foreach (Bag bag in bags)
        {
            // Si un sac est fermé, ouverture de tous les sacs fermés 
            if (bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    /// <summary>
    /// Ajoute un item dans un sac de l'inventaire
    /// </summary>
    /// <param name="item">Item à ajouter</param>
    /// <returns></returns>
    public void AddItem(Item item)
    {
        foreach (Bag bag in bags)
        {
            // Si l'ajout dans le sac est OK
            if (bag.MyBagScript.AddItem(item))
            {
                // Pas besoin d'aller plus loin
                return;
            }
        }
    }
}