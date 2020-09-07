using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des emplacements des sacs
/// </summary>
public class SlotScript : MonoBehaviour, IClickable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Liste (Stack) des items de l'emplacement
    private readonly ObservableStack<Item> items = new ObservableStack<Item>();

    // Propriété d'accès à la stack des items de l'emplacement
    public ObservableStack<Item> MyItems { get => items; }
    
    // Image de l'emplacement
    [SerializeField]
    private Image icon;

    // Image de superposition
    [SerializeField]
    private Image cover;

    // Propriété d'accès à l'image de l'emplacement
    public Image MyIcon { get => icon; set => icon = value; }

    // Propriété d'accès à l'image de superposition
    public Image MyCover{ get => cover; }

    // Texte du nombre d'éléments de l'emplacement
    [SerializeField]
    private Text stackSize = default;

    // Propriété d'accès au texte du nombre d'éléments de l'emplacement
    public Text MyStackText { get => stackSize; }

    // Propriété d'accès à l'item de l'emplacement (Peek retourne l'item situé en haut de la Stack sans le supprimer)
    public Item MyItem { get => !IsEmpty ? items.Peek() : null; }

    // Propriété d'accès à la Stack de l'emplacement
    public int MyCount { get => items.Count; }

    // Propriété d'accès sur l'indicateur d'un emplacement vide
    public bool IsEmpty { get => items.Count == 0; }

    // Propriété d'accès sur l'indicateur d'un emplacement plein
    public bool IsFull { get => (IsEmpty || MyCount < MyItem.MyStackSize) ? false : true; }

    // Propriété d'accès sur le sac qui contient l'emplacement
    public BagScript MyBag { get; set; }

    // Propriété d'accès sur l'index de l'emplacement
    public int MyIndex { get; set; }


    /// <summary>
    /// Awake
    /// </summary>
    public void Awake()
    {
        // Abonnement sur l'évènement d'ajout dans la stack d'un emplacement
        items.PushEvent += new StackUpdated(UpdateSlot);

        // Abonnement sur l'évènement de retrait dans la stack d'un emplacement
        items.PopEvent += new StackUpdated(UpdateSlot);

        // Abonnement sur l'évènement de nettoyage de la stack d'un emplacement
        items.ClearEvent += new StackUpdated(UpdateSlot);
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

        // Masque l'image de superposition
        MyCover.enabled = false;

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
            // Appelle l'évènement de mise à jour du nombre d'élements de l'item en enlevant l'item situé en haut de la Stack
            InventoryScript.MyInstance.OnItemCountChanged(items.Pop());
        }
    }

    /// <summary>
    /// Supprime la stack de l'item de l'emplacement
    /// </summary>
    public void Clear()
    {
        // Nombre d'éléments dans la stack des items de l'emplacement
        int initCount = items.Count;

        // Masque l'image de superposition
        MyCover.enabled = false;

        // S'il y a des éléments dans la stack des items de l'emplacement
        if (initCount > 0)
        {
            // Pour tous les éléments de la stack des items de l'emplacement
            for (int i = 0; i < initCount; i++)
            {
                // Appelle l'évènement de mise à jour du nombre d'élements de l'item en enlevant l'item situé en haut de la Stack
                InventoryScript.MyInstance.OnItemCountChanged(items.Pop());
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
            // Si rien n'est en train d'être déplacé et que l'emplacement n'est pas vide
            if (InventoryScript.MyInstance.MyFromSlot == null && !IsEmpty)
            {
                // S'il y a un objet manipulé
                if (Hand.MyInstance.MyMoveable != null)
                {
                    // Si l'objet manipulé est un item Bag
                    if (Hand.MyInstance.MyMoveable is Bag)
                    {
                        // S'il on clique sur un item Bag
                        if (MyItem is Bag)
                        {
                            // Echange des items Bag
                            InventoryScript.MyInstance.SwapBags(Hand.MyInstance.MyMoveable as Bag, MyItem as Bag);
                        }
                    }
                    // Si l'objet manipulé est un item Armor
                    else if (Hand.MyInstance.MyMoveable is Armor)
                    {
                        // S'il on clique sur un item Armor et que les items ont le même type d'équipement
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (Hand.MyInstance.MyMoveable as Armor).MyArmorType)
                        {
                            // Equipe l'item cliqué
                            (MyItem as Armor).Equip();

                            // Libère l'item
                            Hand.MyInstance.Drop();
                        }
                    }
                }
                else
                {
                    // Drag l'item
                    Hand.MyInstance.TakeMoveable(MyItem as IMoveable);

                    // Définit le slot sur lequel se trouve l'item
                    InventoryScript.MyInstance.MyFromSlot = this;
                }
            }
            // Si rien n'est en train d'être déplacé et que l'emplacement est vide
            else if (InventoryScript.MyInstance.MyFromSlot == null && IsEmpty)
            {
                //  Si l'objet manipulé est un item Bag
                if (Hand.MyInstance.MyMoveable is Bag)
                {
                    // Cast l'item en type Bag
                    Bag bag = Hand.MyInstance.MyMoveable as Bag;

                    // Si le sac n'est pas a déposer dans un de ses emplacements et qu'il y a assez de place pour contenir ce que contient le sac
                    if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.MySlotsCount > 0)
                    {
                        // Ajoute l'item dans l'inventaire
                        AddItem(bag);

                        // Déséquipe le sac de la liste des sacs
                        bag.MyBagButton.RemoveBag();

                        // Libère l'item
                        Hand.MyInstance.Drop();
                    }
                }
                //  Si l'objet manipulé est un item Armor
                else if (Hand.MyInstance.MyMoveable is Armor)
                {
                    // Cast l'item en type Armor
                    Armor armor = Hand.MyInstance.MyMoveable as Armor;

                    // Déséquipe l'item
                    CharacterPanel.MyInstance.MySelectedButton.UnequipArmor();

                    // Ajoute l'item dans l'inventaire
                    AddItem(armor);

                    // Libère l'item
                    Hand.MyInstance.Drop();
                }
            }
            // Si un item est en train d'être déplacé
            else if(InventoryScript.MyInstance.MyFromSlot != null)
            {
                // Si l'item est sur le même emplacement ou qu'il change d'emplacement
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.MyFromSlot) || SwapItems(InventoryScript.MyInstance.MyFromSlot) || MoveStackItems(InventoryScript.MyInstance.MyFromSlot.items))
                {
                    // Libère l'item
                    Hand.MyInstance.Drop();

                    // Réinitialisation de l'emplacement
                    InventoryScript.MyInstance.MyFromSlot = null;
                }
            }
        }

        // Clic droit et rien n'est en train d'être déplacé
        if (eventData.button == PointerEventData.InputButton.Right && Hand.MyInstance.MyMoveable == null)
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
        // Si l'item est un équipement
        else if (MyItem is Armor)
        {
            // Equipement de l'item
            (MyItem as Armor).Equip();
        }
    }

    /// <summary>
    /// Stacke l'item sur son emplacement
    /// </summary>
    /// <param name="item">Item à stacker</param>
    /// <returns></returns>
    public bool StackItem(Item item)
    {
        // S'il y a un item sur l'emplacement et qu'il a le même type que l'item à stacker et que la taille de la Stack de l'emplacement est inférieure au nombre de stack de l'item
        if (!IsEmpty && item.GetType() == MyItem.GetType() && items.Count < MyItem.MyStackSize)
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
        // Mise à jour du nombre d'éléments de l'emplacement de l'item cliquable
        UIManager.MyInstance.UpdateStackSize(this);
    }

    /// <summary>
    /// L'item doit-il rester à son emplacement ?
    /// </summary>
    /// <returns></returns>
    private bool PutItemBack()
    {
        // Masque l'image de superposition
        MyCover.enabled = false;

        // Si c'est le même emplacement
        if (InventoryScript.MyInstance.MyFromSlot == this)
        {
            // Actualise l'image
            InventoryScript.MyInstance.MyFromSlot.MyIcon.enabled = true;

            // Retourne que c'est OK
            return true;
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// L'item doit-il changer d'emplacement ?
    /// </summary>
    /// <param name="newItems">Stack de l'item à déplacer</param>
    /// <returns></returns>
    public bool MoveStackItems(ObservableStack<Item> stackItems)
    {
        // Si l'emplacement est vide ou qu'il est occupé par un item du même type
        if (IsEmpty || stackItems.Peek().GetType() == MyItem.GetType())
        {
            // Nombre d'éléments de la stack
            int count = stackItems.Count;

            // Pour chaque élement de la stack
            for (int i = 0; i < count; i++)
            {
                // Si l'emplacement est plein
                if (IsFull)
                {
                    // Retourne que c'est KO
                    return false;
                }

                // Ajoute l'item sur l'emplacement
                AddItem(stackItems.Pop());
            }

            // Retourne que c'est OK
            return true;
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Echange les emplacements
    /// </summary>
    public bool SwapItems(SlotScript from)
    {
        // Masque l'image de superposition
        from.MyCover.enabled = false;

        if (IsEmpty)
        {
            // Retourne que c'est KO
            return false;
        }

        // Si les items sont de type différents ou que le total des 2 items est supérieur au maximum d'éléments de la Stack
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
        {
            // Stack de l'item à deplacer
            ObservableStack<Item> fromStack = new ObservableStack<Item>(from.items);

            // Vide la stack de l'item de l'emplacement d'origine
            from.items.Clear();

            // Ajoute la Stack de l'item de l'emplacement sélectionné sur l'emplacement d'origine
            from.MoveStackItems(items);

            // Vide l'emplacement sélectionné
            items.Clear();

            // Ajoute la Stack de l'item de l'emplacement d'origine sur l'emplacement sélectionné
            MoveStackItems(fromStack);

            // Retourne que c'est OK
            return true;
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Fusionne les emplacements
    /// </summary>
    public bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            // Retourne que c'est KO
            return false;
        }

        // Si les items sont de même type et que l'emplacement n'est pas plein
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            // Emplacement(s) restant(s) dans la stack
            int freeSpace = MyItem.MyStackSize - MyCount;

            // Pour chaque emplacement vide restant
            for (int i = 0; i < freeSpace; i++)
            {
                AddItem(from.items.Pop());
            }

            // Retourne que c'est OK
            return true;
        }

        // Retourne que c'est KO
        return false;
    }

    /// <summary>
    /// Entrée du curseur
    /// </summary>
    /// <param name="eventData">Evenement d'entrée</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // S'il y a un item dans l'emplacement
        if (!IsEmpty)
        {
            // Affiche le tooltip
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, MyItem);
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