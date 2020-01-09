using UnityEngine;



/// <summary>
/// Classe de gestion des sacs
/// </summary>
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    [Header("Bag")]

    // Prefab du sac
    [SerializeField]
    private GameObject bagPrefab = default;

    // Nombre d'emplacements
    private int slotsCount;

    // Propriété d'accès au nombre d'emplacements du sac
    public int MySlotsCount { get => slotsCount; }

    // Propriété d'accès au script Bag
    public BagScript MyBagScript { get; set; }

    // Propriété d'accès au bouton du sac
    public BagButton MyBagButton { get; set; }


    /// <summary>
    /// Start
    /// </summary>
    /// <param name="slots">Nombre d'emplacements</param>
    public void Initialize(int slots)
    {
        // Initialisation du nombre d'emplacements
        slotsCount = slots;
    }

    /// <summary>
    /// Utilisation du sac
    /// </summary>
    public void Use()
    {
        // Si l'on peut ajouter un sac
        if (InventoryScript.MyInstance.CanAddBag)
        {
            // Supprime l'item de l'emplacement
            Remove();

            // Instantiation d'un objet Bag
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();

            // Ajoute le nombre d'emplacements au sac
            MyBagScript.AddSlots(slotsCount);

            // s'il n'y a pas d'emplacement
            if( MyBagButton == null)
            {
                // Ajoute le sac sur un emplacement (BagButton)
                InventoryScript.MyInstance.AddBag(this);
            }
            else
            {
                // Ajoute le sac sur un emplacement (BagButton) spécifique
                InventoryScript.MyInstance.AddBag(this, MyBagButton);
            }
        }
    }

    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        // Appelle GetDescription sur la classe mère et ajoute la description de l'item
        return base.GetDescription() + string.Format("\n\n<color=#ECECEC>Ajoute <color=cyan>{0}</color> emplacements</color>", MySlotsCount);
    }
}