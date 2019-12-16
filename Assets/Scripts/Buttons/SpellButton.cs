using UnityEngine;
using UnityEngine.EventSystems;



/// <summary>
/// Classe de gestion des boutons de sorts
/// </summary>
public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    // Nom du sort
    [SerializeField]
    private string spellName;

    /// <summary>
    /// Gestion du clic
    /// </summary>
    /// <param name="eventData">Evenement de clic</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Clic gauche
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Drag le sort
            Hand.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
}