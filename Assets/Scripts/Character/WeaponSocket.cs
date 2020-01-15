using UnityEngine;


/// <summary>
/// Classe de gestion des emplacements des armes du personnage
/// </summary>
class WeaponSocket : GearSocket
{
    // Valeur Y
    private float currentY;

    // Référence sur le SpriteRenderer du personnage
    [SerializeField]
    private SpriteRenderer parentSpriteRenderer;


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
                spriteRenderer.sortingOrder = parentSpriteRenderer.sortingOrder - 1;
            }
            // Vue de face
            else
            {
                // L'arme sera devant le personnage
                spriteRenderer.sortingOrder = parentSpriteRenderer.sortingOrder + 5;
            }
        }
    }
}