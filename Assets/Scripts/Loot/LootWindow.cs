using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des butins
/// </summary>
public class LootWindow : MonoBehaviour
{
    // Instance de classe (singleton)
    private static LootWindow instance;

    // Propriété d'accès à l'instance
    public static LootWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type LootWindow (doit être unique)
                instance = FindObjectOfType<LootWindow>();
            }

            return instance;
        }
    }

    // Texte de la page courante
    [SerializeField]
    private Text pageNumber = default;

    // Boutons de pagination
    [SerializeField]
    private GameObject previousButton = default, nextButton = default;

    // Tableau des boutons des butins
    [SerializeField]
    private LootButton[] lootButtons = default;

    // Liste des pages de butin
    private readonly List<List<Drop>> pages = new List<List<Drop>>();

    // Liste des butins
    // private List<Drop> droppedLoots = new List<Drop>();

    // Index de la page courante
    private int pageIndex = 0;

    // CanvasGroup de la fenêtre des butins
    private CanvasGroup canvasGroup;

    // Propriété d'accès sur l'indicateur d'ouverture de la fenêtre des butins
    public bool IsOpen { get => canvasGroup.alpha > 0; }

    // Propriété d'accès sur l'élément interactif
    public IInteractable MyInteractable { get; set; }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Référence sur le canvasGroup de la fenêtre des butins
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Fermeture de la fenêtre des butins
        Close();
    }

    /// <summary>
    /// Ajoute les items de l'index de la page dans la fenêtre des butins
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
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyItem.MyIcon;

                    // Actualise le titre du bouton
                    lootButtons[i].MyTitle.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyItem.MyQuality], pages[pageIndex][i].MyItem.MyTitle);

                    // Assigne l'item du bouton
                    lootButtons[i].MyLoot = pages[pageIndex][i].MyItem;

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
    public void CreatePages(List<Drop> items)
    {
        // Si la fenêtre de butin n'est pas ouverte
        if (!IsOpen)
        {
            // Référence sur la liste des items du butin
            // droppedLoots = items;

            // Création d'une nouvelle page
            List<Drop> currentPage = new List<Drop>();

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
                    currentPage = new List<Drop>();
                }
            }

            // Ajoute les items de l'index de la page dans la fenêtre des butins
            AddLoot();

            // Affiche la fenêtre des butins
            Open();
        }
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

            // Ajoute les items de l'index de la page dans fenêtre des butins
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

            // Ajoute les items de l'index de la page dans la fenêtre des butins
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
        // Trouve l'item
        Drop drop = pages[pageIndex].Find(x => x.MyItem == loot);

        // Retire l'item de la page
        pages[pageIndex].Remove(drop);

        // Retire l'item de la liste des butins
        drop.Remove();

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

            // Ajoute les items de l'index de la page dans la fenêtre des butins
            AddLoot();
        }
    }

    /// <summary>
    /// Ouverture de la fenêtre des butins
    /// </summary>
    public void Open()
    {
        // Bloque les interactions
        canvasGroup.blocksRaycasts = true;

        // Affiche la fenêtre des butins
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Fermeture de la fenêtre des butins
    /// </summary>
    public void Close()
    {
        // Réinitialise l'index de la page
        pageIndex = 0;

        // Vide la liste des butins
        pages.Clear();

        // Efface les boutons de la page courante
        ClearButtons();

        // Débloque les interactions
        canvasGroup.blocksRaycasts = false;

        // Masque la fenêtre des butins
        canvasGroup.alpha = 0;

        // S'il y a un élément interactif
        if (MyInteractable != null)
        {
            MyInteractable.StopInteract();
        }

        // Réinitialise la référence sur l'élément interactif
        MyInteractable = null;
    }
}