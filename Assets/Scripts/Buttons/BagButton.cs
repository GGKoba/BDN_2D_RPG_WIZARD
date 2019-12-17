using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Classe de gestion des boutons des sacs
/// </summary>
public class BagButton : MonoBehaviour
{
    // Référence sur le sac
    private Pocket bag;

    // Propriété d'accès au sac
    public Pocket MyBag
    { 
        get => bag;
        set
        {
            GetComponent<Image>().sprite = (value != null) ? equiped : empty;
            bag = value;
        }
    }


    // Image du sac équipé
    [SerializeField]
    private Sprite equiped;

    // Image du sac non équipé
    [SerializeField]
    private Sprite empty;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        
    }
}