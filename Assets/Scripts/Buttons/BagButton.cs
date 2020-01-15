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

    // Index du sac équipé
    [SerializeField]
    private int bagIndex = default;

    // Propriété d'accès à l'index du sac
    public int MyBagIndex { get => bagIndex; set => bagIndex = value; }

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
            // Si c'est un sac que l'on veut glisser
            if (InventoryScript.MyInstance.MyFromSlot != null && Hand.MyInstance.MyMoveable != null && Hand.MyInstance.MyMoveable is Bag)
            {
                if (MyBag != null)
                {
                    // Echange les sacs
                    InventoryScript.MyInstance.SwapBags(MyBag, Hand.MyInstance.MyMoveable as Bag);
                }
                else
                {
                    // Cast l'objet en "Sac"
                    Bag aBag =(Bag)Hand.MyInstance.MyMoveable;
                    
                    // Assigne le bouton au sac
                    aBag.MyBagButton = this;

                    // Equipe le sac
                    aBag.Use();

                    // Assigne le sac
                    MyBag = aBag;

                    // Libère l'item
                    Hand.MyInstance.Drop();

                    // Réinitialisation de l'emplacement
                    InventoryScript.MyInstance.MyFromSlot = null;
                }
            }
            // [LEFT SHIFT] : Déséquipe le sac
            else if (Input.GetKey(KeyCode.LeftShift))
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