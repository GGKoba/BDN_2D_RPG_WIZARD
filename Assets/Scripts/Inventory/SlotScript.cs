using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de gestion des emplacements des sacs
/// </summary>
public class SlotScript : MonoBehaviour
{
    // Image de l'emplacement
    [SerializeField]
    private Image icon;

    // Liste (stack) des items de l'emplacement
    private Stack<Item> items = new Stack<Item>();

    // Propriété d'accès sur l'indicateur du contenu de l'emplacement
    public bool IsEmpty { get => items.Count == 0; }


    /// <summary>
    /// Ajoute un item sur l'emplacement
    /// </summary>
    /// <param name="item">Item à ajouter</param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        // Ajoute l'item dans la stack
        items.Push(item);

        // Actualise l'image de l'emplacement
        icon.sprite = item.MyIcon;

        // Affiche l'image de l'emplacement
        icon.color = Color.white;

        //Retourne que c'est OK
        return true;
    }
}