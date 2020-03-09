using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des recettes de fabrication
/// </summary>
public class Recipe : MonoBehaviour, ICastable
{
    // Description de la recette de fabrication
    [SerializeField]
    private string description = default;

    // Propriété d'accès à la description de la recette de fabrication
    public string MyDescription { get => description; }

    // Image de la sélection d'une recette
    [SerializeField]
    private Image highlight = default;

    // Tableau des matériaux de fabrication
    [SerializeField]
    private CraftingMaterial[] materials = default;

    // Propriété d'accès au tableau des matériaux de fabrication
    public CraftingMaterial[] MyMaterials { get => materials; }

    // Item fabriqué
    [SerializeField]
    private Item craftedItem = default;

    // Propriété d'accès à l'item fabriqué
    public Item MyCraftedItem { get => craftedItem;}

    // Nombre d'item à fabriquer
    [SerializeField]
    private int craftedItemCount = default;

    // Propriété d'accès au nombre d'item à fabriquer
    public int MyCraftedItemCount { get => craftedItemCount; set => craftedItemCount = value; }

    // Propriété d'accès au titre de l'item
    public string MyTitle { get => craftedItem.MyTitle; }

    // Propriété d'accès à l'image de l'item
    public Sprite MyIcon { get => craftedItem.MyIcon; }

    // Temps de fabrication
    [SerializeField]
    private float craftTime = default;

    // Propriété d'accès au temps de fabrication
    public float MyCastTime { get => craftTime; }

    // Couleur de la barre d'incantation
    [SerializeField]
    private Color barColor = default;

    // Propriété d'accès à la couleur de la barre d'incantation
    public Color MyBarColor { get => barColor; }


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Initialise le nom de la recette de fabrication
        GetComponent<Text>().text = craftedItem.MyTitle;
    }


    /// <summary>
    /// Sélectionne une recette
    /// </summary>
    public void Select()
    {
        // Change la transparance de la recette sélectionnée
        Color color = highlight.color;
        color.a = 0.3f;
        highlight.color = color;
 
        // Affiche la description de la quête sélectionnée
        //QuestWindow.MyInstance.ShowDescription(MyQuest);
    }

    /// <summary>
    /// Désélectionne une recette
    /// </summary>
    public void DeSelect()
    {
        // Change la transparance de la recette sélectionnée
        Color color = highlight.color;
        color.a = 0;
        highlight.color = color;
    }
}