/// <summary>
/// Classe de gestion des coffres
/// </summary>
public class ChestScript : BagScript
{
    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // Ajoute des emplacements au coffre
        AddSlots(48);
    }
}
