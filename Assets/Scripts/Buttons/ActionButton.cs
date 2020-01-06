using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des boutons d'actions
/// </summary>
public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable
{
    // Propriété d'accès à l'objet utilisable
    public IUseable MyUseable { get; set; }

    // Propriété d'accès au bouton d'action
    public Button MyActionButton { get; private set; }
    
    // Image du bouton
    [SerializeField]
    private Image icon;

    // Propriété d'accès à l'image du bouton
    public Image MyIcon { get => icon; set => icon = value; }

    // Propriété d'accès aux items de l'emplacement
    public Stack<IUseable> useables = new Stack<IUseable>();

    // Nombre d'éléments de la Stack de l'emplacement
    private int count;

    // Propriété d'accès au nombre d'éléments de la Stack de l'emplacement
    public int MyCount { get => count; }

    // Texte du bouton d'action
    [SerializeField]
    private Text stackSize = default;

    // Propriété d'accès au texte du bouton d'action
    public Text MyStackText { get => stackSize; }


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence sur le bouton de l'action
        MyActionButton = GetComponent<Button>();

        // Ecoute l'évènement OnClick sur le bouton et déclenche OnClick()
        MyActionButton.onClick.AddListener(OnClick);

        // Abonnement sur l'évènement de mise à jour du nombre d'élements de l'item
        InventoryScript.MyInstance.ItemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    /// <summary>
    /// Action de clic
    /// </summary>
    public void OnClick()
    {
        // Si je manipule quelque chose
        if (Hand.MyInstance.MyMoveable == null)
        {
            // S'il y a quelque chose à utiliser
            if (MyUseable != null)
            {
                // Utilisation de l'item
                MyUseable.Use();
            }

            // S'il y a un item (stack) à utiliser
            if (useables != null && useables.Count > 0)
            {
                // Utilisation de l'item (Peek retourne l'item situé en haut de la Stack sans le supprimer)
                useables.Peek().Use();
            }
        }
    }
    
    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Clic gauche
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // S'il y a un objet à déplacer et qu'il est utlisable
            if (Hand.MyInstance.MyMoveable != null && Hand.MyInstance.MyMoveable is IUseable)
            {
                // Cast l'objet à déplacer en objet à utiliser
                SetUseable(Hand.MyInstance.MyMoveable as IUseable);
            }
        }
    }

    /// <summary>
    /// Définit l'utilisation du bouton d'action
    /// </summary>
    /// <param name="useable">Objet utilisable</param>
    public void SetUseable(IUseable useable)
    {
        // Si c'est un item
        if (useable is Item)
        {
            // L'item est utilisable
            useables = InventoryScript.MyInstance.GetUseables(useable);

            // Nombre d'éléments de la Stack
            count = useables.Count;

            // Actualise la couleur de l'emplacement
            InventoryScript.MyInstance.MyFromSlot.MyIcon.color = Color.white;

            // Réinitilisatiion de l'emplacement
            InventoryScript.MyInstance.MyFromSlot = null;
        }
        else
        {
            // Utilisation du bouton
            MyUseable = useable;
        }

        // Mise à jour de l'image
        UpdateVisual();
    }
    
    /// <summary>
    /// Actualise l'image du bouton d'action
    /// </summary>
    public void UpdateVisual()
    {
        // Image du bouton
        MyIcon.sprite = Hand.MyInstance.Put().MyIcon;

        // Couleur du bouton
        MyIcon.color = Color.white;

        // S'il y a plus d'un élement
        if (count > 1)
        {
            // Mise à jour du nombre d'éléments de l'emplacement de l'item cliquable
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    /// <summary>
    /// Actualise le nombre d'éléments du bouton
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItemCount(Item item)
    {
        // Si c'est un item utilisable et qu'il a un ou plusieurs élements dans sa stack
        if (item is IUseable && useables.Count > 0)
        {
            // Si cet item est le même que celui du bouton
            if (useables.Peek().GetType() == item.GetType())
            {
                // Nombre d'éléments du même type
                useables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                // Actualise le nombre d'éléments de la Stack de l'emplacement
                count = useables.Count;

                // Mise à jour du nombre d'éléments de l'emplacement de l'item cliquable
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }
}