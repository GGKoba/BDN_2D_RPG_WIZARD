using UnityEngine;



/// <summary>
/// Classe de gestion des butins
/// </summary>
public class LootWindow : MonoBehaviour
{
    // Tableau des boutons des butins
    [SerializeField]
    private LootButton[] lootButtons = default;

    // [DEBUG] : Tableau des items du butin
    [SerializeField]
    private Item[] items = default;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // [DEBUG] : Teste l'ajout de butins 
        AddLoot();
    }

    /// <summary>
    /// Ajout d'un butin
    /// </summary>
    private void AddLoot()
    {
        int lootIndex = 2;


        // Actualise l'icone du bouton
        lootButtons[lootIndex].MyIcon.sprite = items[lootIndex].MyIcon;

        // Actualise le titre du bouton
        lootButtons[lootIndex].MyTitle.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[items[lootIndex].MyQuality], items[lootIndex].MyTitle);

        // Assigne l'item du bouton
        lootButtons[lootIndex].MyLoot = items[lootIndex];

        // Affiche le bouton
        lootButtons[lootIndex].gameObject.SetActive(true);
    }
}