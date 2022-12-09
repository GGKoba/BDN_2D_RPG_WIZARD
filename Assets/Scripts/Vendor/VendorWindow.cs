﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de la fenêtre du vendeur
/// </summary>
public class VendorWindow : Window
{
    // Tableau des boutons des items du vendeur
    [SerializeField]
    private VendorButton[] vendorButtons = default;

    // Liste des pages des items du vendeur
    private readonly List<List<VendorItem>> pages = new List<List<VendorItem>>();

    // Référence sur le vendeur
    // private Vendor vendor;

    // Index de la page courante
    private int pageIndex = 0;

    // Texte de la page courante
    [SerializeField]
    private Text pageNumber = default;

    // Boutons de pagination
    [SerializeField]
    private GameObject previousButton = default, nextButton = default;


    /// <summary>
    /// Création de la liste des pages du vendeur
    /// </summary>
    /// <param name="items">Liste des items du vendeur</param>
    public void CreatePages(VendorItem[] items)
    {
        // Supprime les pages
        pages.Clear();

        // Création d'une nouvelle page
        List<VendorItem> currentPage = new List<VendorItem>();

        // Pour tous les items
        for (int i = 0; i < items.Length; i++)
        {
            // Ajoute l'item dans la page courante
            currentPage.Add(items[i]);

            // Si la page a autant d'items que le nombre de boutons (4) ou que c'est le dernier item
            if (currentPage.Count == vendorButtons.Length || i == items.Length - 1)
            {
                // Ajoute la page courante
                pages.Add(currentPage);

                // Création d'une nouvelle page
                currentPage = new List<VendorItem>();
            }
        }

        // Ajoute les items de l'index de la page dans la fenêtre du vendeur
        AddVendorItems();
    }

    /// <summary>
    /// Affiche la page précédente
    /// </summary>
    public void PreviousPage()
    {
        // s'il y a une page précédente
        if (pageIndex > 0)
        {
            // Décrémente l'index de la page
            pageIndex--;

            // Efface les boutons de la page courante
            ClearButtons();

            // Ajoute les items de l'index de la page dans la fenêtre du vendeur
            AddVendorItems();
        }
    }

    /// <summary>
    /// Affiche la page suivante
    /// </summary>
    public void NextPage()
    {
        // s'il y a une page suivante
        if (pageIndex < pages.Count - 1)
        {
            // Incrémente l'index de la page
            pageIndex++;

            // Efface les boutons de la page courante
            ClearButtons();

            // Ajoute les items de l'index de la page dans la fenêtre du vendeur
            AddVendorItems();
        }
    }

    /// <summary>
    /// Ajoute les items de l'index de la page dans la fenêtre du vendeur
    /// </summary>
    private void AddVendorItems()
    {
        // S'il y a au moins une page
        if (pages.Count > 0)
        {
            // Actualise les informations de pagination
            pageNumber.text = string.Format("{0}/{1}", pageIndex + 1, pages.Count);

            // Affichage du bouton "Précédent"
            previousButton.SetActive(pageIndex > 0);

            // Affichage du bouton "Suivant"
            nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

            // Pour tous les items de la page
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                // S'il y a un item
                if (pages[pageIndex][i] != null)
                {
                    // Ajoute un item
                    vendorButtons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

    /// <summary>
    /// Efface les boutons de la page courante
    /// </summary>
    public void ClearButtons()
    {
        // Pour tous les boutons
        foreach (VendorButton button in vendorButtons)
        {
            // Masque le bouton
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Open : Surcharge la fonction Open du script Window
    /// </summary>
    /// <param name="npcRef"></param>
    public override void Open(NPC npcRef)
    {
        // Création de la liste des pages de butin
        CreatePages((npcRef as Vendor).MyItems);

        // Appelle Open sur la classe mère
        base.Open(npcRef);
    }
}