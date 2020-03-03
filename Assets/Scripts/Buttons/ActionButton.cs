using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des boutons d'actions
/// </summary>
public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
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

    // Stack des items de l'emplacement
    private Stack<IUseable> useables = new Stack<IUseable>();

    // Propriété d'accès à la Stack des items de l'emplacement
    public Stack<IUseable> MyUseables
    { 
        get => useables;
        set
        {
            // Si l'item à au moins un élément
            if (value.Count > 0)
            {
                // Récupère le 1er élément de la stack de l'item
                MyUseable = value.Peek();
            }
            else
            {
                // Réinitialise l'item du bouton si l'item n'a plus d'élément
                MyUseable = null;
            }

            useables = value;
        }
    }

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
        // Si un item est manipulé
        if (Hand.MyInstance.MyMoveable == null)
        {
            // S'il y a quelque chose à utiliser
            if (MyUseable != null)
            {
                // Utilisation de l'item
                MyUseable.Use();
            }
            // S'il y a un item (stack) à utiliser
            else if (MyUseables != null && MyUseables.Count > 0)
            {
                // Utilisation de l'item (Peek retourne l'item situé en haut de la Stack sans le supprimer)
                MyUseables.Peek().Use();
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

            // Masque le tooltip
            UIManager.MyInstance.HideTooltip();
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
            MyUseables = InventoryScript.MyInstance.GetUseables(useable);

            if (InventoryScript.MyInstance.MyFromSlot != null)
            {
                // Actualise la couleur de l'emplacement
                InventoryScript.MyInstance.MyFromSlot.MyIcon.color = Color.white;

                // Réinitilisatiion de l'emplacement
                InventoryScript.MyInstance.MyFromSlot = null;
            }
        }
        else
        {
            // Réinitialise les items de l'emplacement
            MyUseables.Clear();

            // Utilisation du bouton
            MyUseable = useable;
        }

        // Actualise le nombre d'éléments de la Stack
        count = MyUseables.Count;

        // Mise à jour de l'image
        UpdateVisual(useable as IMoveable);

        // Actualise le tooltip
        UIManager.MyInstance.RefreshTooltip(MyUseable as IDescribable);
    }

    /// <summary>
    /// Actualise l'image du bouton d'action
    /// </summary>
    /// <param name="moveable">Item déplacé</param>
    public void UpdateVisual(IMoveable moveable)
    {
        // Si un item est en train d'être déplacé
        if (Hand.MyInstance.MyMoveable != null)
        {
            // Libère l'item
            Hand.MyInstance.Drop();
        }

        // Image du bouton
        MyIcon.sprite = moveable.MyIcon;

        // Couleur du bouton
        MyIcon.color = Color.white;

        // S'il y a plus d'un élement
        if (count > 1)
        {
            // Mise à jour du nombre d'éléments de l'emplacement de l'item cliquable
            UIManager.MyInstance.UpdateStackSize(this);
        }
        // Si l'item est un sort
        else if (MyUseable is Spell)
        {
            // Réinitialise le texte du nombre d'éléments de l'item
            UIManager.MyInstance.ClearStackCount(this);
        }
    }

    /// <summary>
    /// Actualise le nombre d'éléments du bouton
    /// </summary>
    /// <param name="item">Item du bouton</param>
    public void UpdateItemCount(Item item)
    {
        // Si c'est un item utilisable et qu'il a un ou plusieurs élements dans sa stack
        if (item is IUseable && MyUseables.Count > 0)
        {
            // Si cet item est le même que celui du bouton
            if (MyUseables.Peek().GetType() == item.GetType())
            {
                // Nombre d'éléments du même type
                MyUseables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                // Actualise le nombre d'éléments de la Stack de l'emplacement
                count = MyUseables.Count;

                // Mise à jour du nombre d'éléments de l'emplacement de l'item cliquable
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable describable = null;

        // S'il y a un objet utilisable (sort) et que cet objet est "descriptible"
        if (MyUseable != null && MyUseable is IDescribable)
        {
            // L'objet utilisable devient "descriptible"
            describable = MyUseable as IDescribable;
        }
        // S'il y a un item qui a plusieurs élements (objet stackable)
        else if (MyUseables.Count > 0)
        {
            // L'item devient "descriptible"
            describable = MyUseables.Peek() as IDescribable;
        }

        // S'il y a un objet "descriptible"
        if (describable != null)
        {
            // Affiche le tooltip
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, describable);
        }
    }

    /// <summary>
    /// Sortie du curseur
    /// </summary>
    /// <param name="eventData">Evenement de sortie</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Masque le tooltip
        UIManager.MyInstance.HideTooltip();
    }
}