using System.Collections.Generic;
using UnityEngine;



// Gestion de la mise à jour du nombre d'élements de l'item
public delegate void ItemCountChanged(Item item);


/// <summary>
/// Classe de gestion de l'inventaire
/// </summary>
public class InventoryScript : MonoBehaviour
{
    // Evènement de mise à jour du nombre d'élements de l'item
    public event ItemCountChanged ItemCountChangedEvent;

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
    private readonly List<Bag> bags = new List<Bag>();

    // Tableau des boutons des sacs
    [SerializeField]
    private BagButton[] bagButtons = default;

    // [DEBUG] : Tableau des items de l'inventaire
    [SerializeField]
    private Item[] items = default;

    // Propriété sur l'indicateur d'ajout des sacs
    public bool CanAddBag { get => bags.Count < bagButtons.Length; }

    // Référence sur un emplacement
    private SlotScript fromSlot;

    // Propriété d'accès sur la référence sur un emplacement
    public SlotScript MyFromSlot
    { 
        get => fromSlot;
        set
        {
            fromSlot = value;

            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    // Propriété d'accès au nombre d'emplacements vides
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            // Pour chaque sac
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }

            return count;
        }
    }

    // Propriété d'accès au nombre d'emplacements du sac
    public int MyTotalSlotCount
    { 
        get
        {
            int count = 0;

            // Pour chaque sac
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    // Propriété d'accès au nombre d'emplacements plein du sac
    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }


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
        // [L] : Utilise un sac (Ajoute le sac sur un emplacement)
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Création d'un sac
            Bag bag = (Bag)Instantiate(items[0]);

            // Initialisation du sac
            bag.Initialize(20);

            // Utilise le sac
            bag.Use();
        }

        // [K] : Ajoute un item Sac dans l'inventaire
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Création d'un item Sac
            Bag bag = (Bag)Instantiate(items[0]);

            // Initialisation du sac
            bag.Initialize(20);

            // Ajoute l'item Sac dans un sac de l'inventaire
            AddItem(bag);
        }

        // [J] : Ajoute un item Potion dans l'inventaire
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Création d'un item Potion
            HealthPotion healthPotion = (HealthPotion)Instantiate(items[1]);

            // Ajoute l'item HealthPotion dans un sac de l'inventaire
            AddItem(healthPotion);
        }

        // [H] : Ajoute un item Armor dans l'inventaire
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Création d'un sac
            Bag bag = (Bag)Instantiate(items[0]);

            // Initialisation du sac
            bag.Initialize(12);

            // Utilise le sac
            bag.Use();

            // Création des items Armor
            Armor helmet = (Armor)Instantiate(items[2]);
            Armor shoulders = (Armor)Instantiate(items[3]);
            Armor chest = (Armor)Instantiate(items[4]);
            Armor gloves = (Armor)Instantiate(items[5]);
            Armor pants = (Armor)Instantiate(items[6]);
            Armor boots = (Armor)Instantiate(items[7]);
            Armor sword = (Armor)Instantiate(items[8]);
            Armor shield = (Armor)Instantiate(items[9]);
            Armor rod = (Armor)Instantiate(items[10]);
            Armor orb = (Armor)Instantiate(items[11]);

            // Ajoute les items Armor dans un sac de l'inventaire
            bag.MyBagScript.AddItem(helmet);
            bag.MyBagScript.AddItem(shoulders);
            bag.MyBagScript.AddItem(chest);
            bag.MyBagScript.AddItem(gloves);
            bag.MyBagScript.AddItem(pants);
            bag.MyBagScript.AddItem(boots);
            bag.MyBagScript.AddItem(sword);
            bag.MyBagScript.AddItem(shield);
            bag.MyBagScript.AddItem(rod);
            bag.MyBagScript.AddItem(orb);

            bag.MyBagScript.OpenClose();
        }
    }

    /// <summary>
    /// Ajoute un sac à sur la barre des sacs
    /// </summary>
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            // S'il n'y a pas de sacs sur le bouton
            if (bagButton.MyBag == null)
            {
                // Assignation du sac au bouton
                bagButton.MyBag = bag;

                // Assignation du bouton au sac
                bag.MyBagButton = bagButton;

                // Ajoute le sac dans la liste
                bags.Add(bag);

                // Assignation de l'index du sac
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);

                break;
            }
        }
    }

    /// <summary>
    /// Ajoute un sac à sur un emplacement spécifique 
    /// </summary>
    public void AddBag(Bag bag, BagButton bagButton)
    {
        // Ajoute le sac sur un bouton
        bagButton.MyBag = bag;

        // Ajoute un sac à sur la barre des sacs
        bags.Add(bag);

        // Assignation de l'index du sac
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }


    /// <summary>
    /// Retire un sac à sur la barre des sacs
    /// </summary>
    /// <param name="bag">Sac à retirer</param>
    public void RemoveBag(Bag bag)
    {
        // Retire le sac
        bags.Remove(bag);

        // Détruit l'objet
        Destroy(bag.MyBagScript.gameObject);
    }

    /// <summary>
    /// Echange les sacs
    /// </summary>
    /// <param name="oldBag">Ancien sac</param>
    /// <param name="newBag">Nouveau sac</param>
    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.MySlotsCount) + newBag.MySlotsCount;

        // S'il y a assez de place entre les emplacements du nouveau sac et du sac à échanger
        if (newSlotCount - MyFullSlotCount >= 0)
        {
            // Items du sac à échanger
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            // Retire l'ancien sac
            RemoveBag(oldBag);

            // Reprend le bouton de l'ancien sac
            newBag.MyBagButton = oldBag.MyBagButton;

            // Ajoute le nouveau sac
            newBag.Use();

            // Pour tous les elemnts du sac
            foreach(Item item in bagItems)
            {
                // Si l'item est différent de mon sac
                if (item != newBag)
                {
                    // Ajoute l'item
                    AddItem(oldBag);
                }
            }
            
            // Libère l'item
            Hand.MyInstance.Drop();

            // Réinitialisation de l'emplacement de base
            MyInstance.fromSlot = null;
        }
    }

    /// <summary>
    /// Ouverture/Fermeture de tous les sacs de l'inventaire
    /// </summary>
    public void OpenClose()
    {
        // Y a-t-il au moins un sac fermé ?
        bool closedBag = bags.Find(bag => !bag.MyBagScript.IsOpen);

        // Pour tous les sacs de l'inventaire
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
    public bool AddItem(Item item)
    {
        // Si l'item est stackable
        if (item.MyStackSize > 0)
        {
            // Si l'item peut se stacker sur un emplacement
            if (PlaceInStack(item))
            {
                // Retourne que c'est OK
                return true;
            }
        }
                
        // Place l'item dans un nouvel emplacement
        return PlaceInEmpty(item);
    }

    /// <summary>
    /// Place l'item dans la Stack de l'emplacement du sac
    /// </summary>
    /// <param name="item">Item à placer dans la stack</param>
    /// <returns></returns>
    private bool PlaceInStack(Item item)
    {
        // Pour tous les sacs de l'inventaire
        foreach (Bag bag in bags)
        {
            // Pour tous les emplacements du sac
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // Si l'item peut être ajouté dans la stack de l'emplacement
                if (slot.StackItem(item))
                {
                    // Déclenche l'évènement de mise à jour du nombre d'élements de l'item
                    OnItemCountChanged(item);

                    // Retourne que c'est OK
                    return true;
                }
            }
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Place l'item dans un nouvel emplacement
    /// </summary>
    /// <param name="item">Item à placer</param>
    private bool PlaceInEmpty(Item item)
    {
        // Pour tous les sacs de l'inventaire
        foreach (Bag bag in bags)
        {
            // Si l'ajout dans le sac est OK
            if (bag.MyBagScript.AddItem(item))
            {
                // Déclenche l'évènement de mise à jour du nombre d'élements de l'item
                OnItemCountChanged(item);

                // Retourne que c'est OK
                return true;
            }
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Retourne les items "useable" d'un même type
    /// </summary>
    /// <param name="useable">Item "useable"</param>
    /// <returns></returns>
    public Stack<IUseable> GetUseables(IUseable useable)
    {
        // Stack des items utilisables
        Stack<IUseable> items = new Stack<IUseable>();

        // Pour tous les sacs
        foreach (Bag bag in bags)
        {
            // Pour tous les emplacements du sac
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // Si l'emplacement n'est pas vide et que c'est le même type
                if (!slot.IsEmpty && slot.MyItem.GetType() == useable.GetType())
                {
                    // Pour tous les items de l'emplacement
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item as IUseable);
                    }
                }
            }
        }

        // Retourne la stack des items utilisables
        return items;
    }

    /// <summary>
    /// Nombre d'items du même nom contenu dans l'inventaire
    /// </summary>
    /// <param name="name">Nom de l'item</param>
    /// <returns></returns>
    public int GetItemCount(string name)
    {
        // Nombre d'éléments
        int count = 0;

        // Pour tous les sacs
        foreach (Bag bag in bags)
        {
            // Pour tous les emplacements du sac
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // Si l'emplacement n'est pas vide et que c'est le même type
                if (!slot.IsEmpty && slot.MyItem.GetType().ToString().ToLower() == name.ToLower())
                {
                    // Ajoute le nombre d'élements de l'emplacement
                    count += slot.MyItems.Count;
                }
            }
        }

        // Retourne le nombre d'éléments
        return count;
    }

    /// <summary>
    /// Appelle l'évènement de mise à jour du nombre d'élements de l'item
    /// </summary>
    /// <param name="item">Item courant</param>
    public void OnItemCountChanged(Item item)
    {
        // S'il existe un abonnement à cet évènement
        if (ItemCountChangedEvent != null)
        {
            // Déclenchement de mise à jour du nombre d'élements de l'item
            ItemCountChangedEvent.Invoke(item);
        }
    }
}