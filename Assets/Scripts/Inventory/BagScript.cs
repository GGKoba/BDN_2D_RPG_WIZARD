using UnityEngine;



/// <summary>
/// Classe de gestion des sacs de l'inventaire
/// </summary>
public class BagScript : MonoBehaviour
{
    // Prefab de l'emplacement du sac
    [SerializeField]
    private GameObject slotPrefab = default;

    // Canvas du sac
    private CanvasGroup canvasGroup;

    // Le sac est-il ouvert ?
    public bool IsOpen { get => canvasGroup.alpha > 0; }


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
            Instantiate(slotPrefab, transform);
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
}