using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des butins
/// </summary>
public class LootWindow : MonoBehaviour
{
    // Texte de la page courante
    [SerializeField]
    private Text pageNumber = default;

    // Boutons de pagination
    [SerializeField]
    private GameObject previousButton, nextButton = default;

    // Tableau des boutons des butins
    [SerializeField]
    private LootButton[] lootButtons = default;

    // [DEBUG] : Tableau des items du butin
    [SerializeField]
    private Item[] items = default;

    // Liste des pages de butin
    private List<List<Item>> pages = new List<List<Item>>();

    // Index de la page courante
    private int pageIndex = 0;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // [DEBUG] : Teste l'ajout de butins 

        List<Item> lootList = new List<Item>();

        // Pour tous les items de la page
        for (int i = 0; i < items.Length; i++)
        {
            lootList.Add(items[i]);
        }

        // Création de la liste des pages de butin
        CreatePages(lootList);
    }

    /// <summary>
    /// Ajoute les items de l'index de la page dans la fenêtre de butin
    /// </summary>
    private void AddLoot()
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
                    // Actualise l'icone du bouton
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyIcon;

                    // Actualise le titre du bouton
                    lootButtons[i].MyTitle.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyQuality], pages[pageIndex][i].MyTitle);

                    // Assigne l'item du bouton
                    lootButtons[i].MyLoot = pages[pageIndex][i];

                    // Affiche le bouton
                    lootButtons[i].gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Création de la liste des pages de butin
    /// </summary>
    /// <param name="items">Liste des items du butin</param>
    public void CreatePages(List<Item> items)
    {
        List<Item> currentPage = new List<Item>();

        // Pour tous les items
        for (int i = 0; i < items.Count; i++)
        {
            // Ajoute l'item dans la page courante
            currentPage.Add(items[i]);

            // Si la page a autant d'items que le nombre de boutons (4) ou que c'est le dernier item
            if (currentPage.Count == lootButtons.Length || i == items.Count - 1)
            {
                // Ajoute la page courante
                pages.Add(currentPage);

                // Création d'une nouvelle page
                currentPage = new List<Item>();
            }
        }

        // Ajoute les items de l'index de la page dans la fenêtre de butin
        AddLoot();
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

            // Ajoute les items de l'index de la page dans la fenêtre de butin
            AddLoot();
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

            // Ajoute les items de l'index de la page dans la fenêtre de butin
            AddLoot();
        }
    }

    /// <summary>
    /// Efface les boutons de la page courante
    /// </summary>
    public void ClearButtons()
    {
        // Pour tous les boutons
        foreach (LootButton button in lootButtons)
        {
            // Masque le bouton
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Retire l'item de la page et de la liste des butins
    /// </summary>
    /// <param name="loot">Item à retirer</param>
    public void TakeLoot(Item loot)
    {
        // Retire l'item de la page
        pages[pageIndex].Remove(loot);

        // Retire l'item de la liste des butins

        // Si la page n'a plus d'item
        if (pages[pageIndex].Count == 0)
        {
            // Supprime la page
            pages.Remove(pages[pageIndex]);

            // Si c'est la dernière page et que ce n'est pas la première
            if (pageIndex == pages.Count && pageIndex > 0)
            {
                // Décrémente l'index de la page
                pageIndex--;
            }

            // Ajoute les items de l'index de la page dans la fenêtre de butin
            AddLoot();
        }
    }
}