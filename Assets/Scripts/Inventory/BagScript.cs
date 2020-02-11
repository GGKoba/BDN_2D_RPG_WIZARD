using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe de gestion des sacs de l'inventaire
/// </summary>
public class BagScript : MonoBehaviour
{
    // Liste des emplacements du sac
    private readonly List<SlotScript> slots = new List<SlotScript>();

    // Propriété d'accès aux emplacement du sac
    public List<SlotScript> MySlots { get => slots; }

    // Propriété d'accès sur l'index du sac
    public int MyBagIndex { get; set; }

    // Prefab de l'emplacement du sac
    [SerializeField]
    private GameObject slotPrefab = default;

    // Canvas du sac
    private CanvasGroup canvasGroup;

    // Propriété d'accès sur l'indicateur d'ouverture du sac
    public bool IsOpen { get => canvasGroup.alpha > 0; }

    // Propriété d'accès au nombre d'emplacements vides
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            // Pour chaque emplacement
            foreach (SlotScript slot in MySlots)
            {
                // Si l'emplacement est vide
                if (slot.IsEmpty)
                {
                    count++;
                }
            }

            return count;
        }
    }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le canvasGroup d'un sac
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Ajoute des emplacements au sac
    /// </summary>
    /// <param name="slotsCount">Nombre d'emplacements</param>
    public void AddSlots(int slotsCount)
    {
        for (int i = 0; i < slotsCount; i++)
        {
            // Instantiation d'un objet Slot
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();

            // Assigne le sac à l'emplacement
            slot.MyBag = this;

            // Assigne l'index de l'emplacement
            slot.MyIndex = i;

            // Ajoute l'emplacement dans la liste des emplacements du sac
            slots.Add(slot);
        }
    }

    /// <summary>
    /// Ouverture/Fermeture d'un sac
    /// </summary>
    public void OpenClose()
    {
        // Masque(0) /Affiche(1) le sac
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        // Bloque/débloque les interactions
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
    }

    /// <summary>
    /// Ajoute un item dans un emplacement du sac
    /// </summary>
    /// <param name="item">Item à ajouter</param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        // Pour tous les emplacements du sac
        foreach (SlotScript slot in slots)
        {
            // Si l'emplacement est vide
            if (slot.IsEmpty)
            {
                // Ajoute l'item dans l'emplacement
                slot.AddItem(item);

                // Retourne que c'est OK
                return true;
            }
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Renvoie la liste des items du sac
    /// </summary>
    /// <returns></returns>
    public List<Item> GetItems()
    {
        // Liste des items du sac
        List<Item> items = new List<Item>();

        // Pour tous les emplacements du sac
        foreach (SlotScript slot in slots)
        {
            // Si l'emplacement n'est pas vide
            if (!slot.IsEmpty)
            {
                // Pour tous les items de l'emplacement
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }

        // Retourne la liste
        return items;
    }

    // Vide le contenu du sac
    public void Clear()
    {
        // Pour tous les emplacements du sac
        foreach (SlotScript slot in slots)
        {
            // Vide l'emplacement
            slot.Clear();
        }
    }
}