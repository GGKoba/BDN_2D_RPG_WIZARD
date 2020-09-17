using UnityEngine;



/// <summary>
/// Classe de gestion de la feuille du personnage
/// </summary>
public class CharacterPanel : MonoBehaviour
{
    // Instance de classe (singleton)
    private static CharacterPanel instance;

    // Propriété d'accès à l'instance
    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type CharacterPanel (doit être unique)
                instance = FindObjectOfType<CharacterPanel>();
            }

            return instance;
        }
    }

    // CanvasGroup de la feuille du personnage
    [SerializeField]
    private CanvasGroup canvasGroup = default;

    // Propriété d'accès sur l'indicateur d'ouverture de la feuille du personnage
    public bool IsOpen { get => canvasGroup.alpha > 0; }

    // Propriété d'accès au bouton sélectionné dans la feuille du personnage
    public CharacterButton MySelectedButton { get; set; }


    [Header("Slots")]
    //Emplacements des items d'équipement : Tête
    [SerializeField]
    private CharacterButton head = default;

    //Emplacements des items d'équipement : Epaules
    [SerializeField]
    private CharacterButton shoulders = default;

    //Emplacements des items d'équipement : Torse
    [SerializeField]
    private CharacterButton chest = default;

    //Emplacements des items d'équipement : Mains
    [SerializeField]
    private CharacterButton hands = default;

    //Emplacements des items d'équipement : Jambes
    [SerializeField]
    private CharacterButton legs = default;

    //Emplacements des items d'équipement : Pied
    [SerializeField]
    private CharacterButton feet = default;

    //Emplacements des items d'équipement : Arme Principale
    [SerializeField]
    private CharacterButton mainHand = default;

    //Emplacements des items d'équipement : Arme secondaire
    [SerializeField]
    private CharacterButton offHand = default;


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
        OpenClose();
    }

    /// <summary>
    /// Ouverture/Fermeture de la feuille du personnage
    /// </summary>
    public void OpenClose()
    {
        // Bloque/débloque les interactions
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;

        // Masque(0) /Affiche(1) le menu
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
    }

    /// <summary>
    /// Equipe un item
    /// </summary>
    /// <param name="item">Item à équiper</param>
    public void EquipArmor(Armor item)
    {
        // Si la feuille du personnage d'équipement est fermée
        if (!IsOpen)
        {
            //Ouvre la feuille du personnage
            OpenClose();
        }

        // Suivant le type d'équipement, équipe l'item dans l'emplacement approprié 
        switch (item.MyArmorType)
        {
            // Tête
            case ArmorType.Head:
                head.EquipArmor(item);
                break;
            // Epaule
            case ArmorType.Shoulders:
                shoulders.EquipArmor(item);
                break;
            // Torse
            case ArmorType.Chest:
                chest.EquipArmor(item);
                break;
            // Mains          
            case ArmorType.Hands:
                hands.EquipArmor(item);
                break;
            // Jambes
            case ArmorType.Legs:
                legs.EquipArmor(item);
                break;
            // Pied
            case ArmorType.Feet:
                feet.EquipArmor(item);
                break;
            // Main gauche
            case ArmorType.MainHand:
                mainHand.EquipArmor(item);
                break;
            // Main droite
            case ArmorType.OffHand:
                offHand.EquipArmor(item);
                break;
        }
    }
}