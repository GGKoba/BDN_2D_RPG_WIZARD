using UnityEngine;


/// <summary>
/// Classe de gestion des emplacements des armes du personnage
/// </summary>
class WeaponSocket : GearSocket
{
    // Valeur Y
    private readonly float currentY = 0;

    // Référence sur le SpriteRenderer du personnage
    // [SerializeField]
    // private SpriteRenderer parentSpriteRenderer = default;


    /// <summary>
    /// Actualise les paramètres de l'animation : Surcharge la fonction SetDirection du script GearSocket
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    public override void SetDirection(float x, float y)
    {
        // Appelle SetDirection sur la classe mère
        base.SetDirection(x, y);

        // Si y n'a plus la même valeur (changement de direction)
        if (currentY != y)
        {
            // Vue de dos
            if (y == 1)
            {
                // L'arme sera derrière le personnage (cachée par sa main)
                transform.localPosition = new Vector3(0, 0.854f, 0);
            }
            // Vue de face
            else
            {
                // L'arme sera devant le personnage
                transform.localPosition = new Vector3(0, 0.849f, 0);
            }
        }
    }
}