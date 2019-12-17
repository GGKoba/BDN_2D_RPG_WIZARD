using UnityEngine;



/// <summary>
/// Classe de gestion de l'inventaire de base
/// </summary>
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Pocket : Item, IUseable
{
    // Nombre d'emplacements
    private int slotsCount;

    // Propriété d'accès au nombre d'emplacements du sac
    public int MySlotsCount { get => slotsCount; }
    
    // Prefab du sac
    [SerializeField]
    private GameObject bagPrefab;

    // Propriété d'accès au script Bag
    public Bag MyBagScript { get; set; }


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
        // Instantiation d'un objet Bag
        MyBagScript = Instantiate(bagPrefab, Inventory.MyInstance.transform).GetComponent<Bag>();

        // Ajoute le nombre d'emplacements au sac
        MyBagScript.AddSlots(slotsCount);
    }
}