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
        // Clic gauche
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // [LEFT SHIFT] : Déséquipe le sac
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Drag le sac
                Hand.MyInstance.TakeMoveable(MyBag);
            }
            // S'il y a un sac
            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    /// <summary>
    /// Déséquipe un sac
    /// </summary>
    public void RemoveBag()
    {
        // Retire un sac de la liste des boutons des sacs
        InventoryScript.MyInstance.RemoveBag(MyBag);

        // Réinitialisation du bouton du sac
        MyBag.MyBagButton = null;

        // Pour tous les items du sac
        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            // Ajoute tous les items du sac dans l'inventaire
            InventoryScript.MyInstance.AddItem(item);
        }

        // Réinitialisation du sac
        MyBag = null;
    }
}