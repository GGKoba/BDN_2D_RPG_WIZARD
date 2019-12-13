using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des boutons d'actions
/// </summary>
public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    // Propriété d'accès à l'bjet utilisable
    public IUseable MyUseable { get; set; }

    // Propriété d'accès au bouton d'action
    public Button MyActionButton { get; private set; }

       
    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Référence sur le bouton de l'action
        MyActionButton = GetComponent<Button>();

        // Ecoute l'évènement OnClick sur le bouton et déclenche OnClick()
        MyActionButton.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Action de clic
    /// </summary>
    public void OnClick()
    {
        // S'il y a quelque chose à utiliser
        if (MyUseable != null)
        {
            MyUseable.Use();
        }
    }
    
    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
