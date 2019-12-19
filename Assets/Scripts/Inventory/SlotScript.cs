using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des emplacements des sacs
/// </summary>
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    // Liste (Stack) des items de l'emplacement
    private readonly ObservableStack<Item> items = new ObservableStack<Item>();

    // Image de l'emplacement
    [SerializeField]
    private Image icon;

    // Propriété d'accès à l'image de l'emplacement
    public Image MyIcon { get => icon; set => icon = value; }

    // Texte du nombre d'éléments de l'emplacement
    [SerializeField]
    private Text stackSize;

    // Propriété d'accès au texte du nombre d'éléments de l'emplacement
    public Text MyStackText { get => stackSize; }

    // Propriété d'accès à la Stack de l'emplacement
    public int MyCount { get => items.Count; }

    // Propriété d'accès sur l'indicateur du contenu de l'emplacement
    public bool IsEmpty { get => items.Count == 0; }

    // Propriété d'accès à l'item de l'emplacement (Peek retourne l'item situé en haut de la Stack sans le supprimer)
    public Item MyItem { get => !IsEmpty ? items.Peek() : null; }


    /// <summary>
    /// Awake
    /// </summary>
    public void Awake()
    {
        // Abonnement sur l'évènement d'ajout dans la stack d'un emplacement
        items.OnPush += new StackUpdated(UpdateSlot);

        // Abonnement sur l'évènement de retrait dans la stack d'un emplacement
        items.OnPop += new StackUpdated(UpdateSlot);

        // Abonnement sur l'évènement de nettoyage de la stack d'un emplacement
        items.OnClear += new StackUpdated(UpdateSlot);
    }

    /// <summary>
    /// Ajoute un item sur l'emplacement
    /// </summary>
    /// <param name="item">Item à ajouter</param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        // Ajoute l'item dans la Stack des items de l'emplacement
        items.Push(item);

        // Actualise l'image de l'emplacement
        icon.sprite = item.MyIcon;

        // Actualise la couleur de l'emplacement
        icon.color = Color.white;

        // Assigne l'emplacement à l'item
        item.MySlot = this;

        // Retourne que c'est OK
        return true;
    }

    /// <summary>
    /// Retire un item de l'emplacement
    /// </summary>
    /// <param name="item">Item à supprimer</param>
    public void RemoveItem(Item item)
    {
        // S'il y a un item sur l'emplacement
        if (!IsEmpty)
        {
            // Enleve l'item situé en haut de la Stack
            items.Pop();
        }
    }

    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Clic droit
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Utilisation de l'item
            UseItem();
        }
    }

    /// <summary>
    /// Utilisation de l'item de l'emplacement
    /// </summary>
    public void UseItem()
    {
        // Si l'item est utilisable
        if (MyItem is IUseable)
        {
            // Utilisation de l'item
            (MyItem as IUseable).Use();
        }
    }

    /// <summary>
    /// Stacke l'item sur son emplacement
    /// </summary>
    /// <param name="item">Item à stacker</param>
    /// <returns></returns>
    public bool StackItem(Item item)
    {
        // S'il y a un item sur l'emplacement et qu'il a le même nom que l'item à stacker et que la taille de la Stack de l'emplacement est inférieure au nombre de stack de l'item
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            // Ajoute l'item dans la Stack des items de l'emplacement
            items.Push(item);

            // Assigne l'emplacement à l'item
            item.MySlot = this;

            // Retourne que c'est OK
            return true;
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Actualise l'emplacement
    /// </summary>
    private void UpdateSlot()
    {
        // Mise à jour de la Stack de l'emplacement de l'item
        UIManager.MyInstance.UpdateStackSize(this);
    }
}