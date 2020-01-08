using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Classe de gestion des boutons des butins
/// </summary>
public class LootButton : MonoBehaviour
{
    // Image du bouton
    [SerializeField]
    private Image icon = default;

    // Propriété d'accès à l'image du bouton
    public Image MyIcon { get => icon; }

    // Titre du bouton
    [SerializeField]
    private Text title = default;

    // Propriété d'accès au titre du bouton
    public Text MyTitle { get => title; }
}