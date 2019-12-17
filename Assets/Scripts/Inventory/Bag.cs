using UnityEngine;



/// <summary>
/// Classe de gestion des sacs de l'inventaire
/// </summary>
public class Bag : MonoBehaviour
{
    // Prefab de l'emplacement du sac
    [SerializeField]
    private GameObject slotPrefab;


    /// <summary>
    /// Ajoute des emplacements au sac
    /// </summary>
    /// <param name="slotsCount">Nombre d'emplacements</param>
    public void AddSlots(int slotsCount)
    {
        for (int i = 0; i < slotsCount; i++)
        {
            // Instantiation d'un objet Slot
            Instantiate(slotPrefab, transform);
        }
    }
}