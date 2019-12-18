using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Classe de gestion des boutons des sacs
/// </summary>
public class BagButton : MonoBehaviour, IPointerClickHandler
{
    // Référence sur le sac
    private Bag bag;

    // Propriété d'accès au sac
    public Bag MyBag
    { 
        get => bag;
        set
        {
            GetComponent<Image>().sprite = (value != null) ? equiped : empty;
            bag = value;
        }
    }


    // Image du sac équipé
    [SerializeField]
    private Sprite equiped = default;

    // Image du sac non équipé
    [SerializeField]
    private Sprite empty = default;


    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // S'il y a un sac
        if (bag != null)
        {
            bag.MyBagScript.OpenClose();
        }
    }
}