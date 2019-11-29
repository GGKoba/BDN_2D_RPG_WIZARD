using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion de l'interface
/// </summary>
public class UIManager : MonoBehaviour
{
    // Tableau des boutons d'action
    [SerializeField]
    private Button[] actionButtons = default;
    // Liste des touches liées aux actions
    private KeyCode action1, action2, action3;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Binding des touches
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // Si on presse la touche d'action 1 (touche 1)
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }

        // Si on presse la touche d'action 2 (touche 2)
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }

        // Si on presse la touche d'action 3 (touche 3)
        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
    }

    /// <summary>
    /// Clic sur un bouton d'action
    /// </summary>
    /// <param name="buttonIndex">Index du bouton d'action</param>
    private void ActionButtonOnClick(int buttonIndex)
    {
        // Appelle la fonction Onclick du bouton lié à l'action
        actionButtons[buttonIndex].onClick.Invoke();
    }
}
