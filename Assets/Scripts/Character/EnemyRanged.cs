

using UnityEngine;
/// <summary>
/// Classe contenant les fonctionnalités spécifiques aux ennemis à distance
/// </summary>
public class EnemyRanged : Enemy
{
    // Prefab des "fleches"
    [SerializeField]
    private GameObject arrowPrefab = default;

    // Tableau des positions pour tirer
    [SerializeField]
    private Transform[] exitPoints = default;


    /// <summary>
    /// Attaque
    /// </summary>
    /// <param name="exitIndex">Index de la position d'attaque</param>
    public void Shoot(int exitIndex)
    {
        // Instantie la flèche
        SpellScript arrow = Instantiate(arrowPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

        // Affecte la cible et les dégâts de la flèche
        arrow.Initialize(MyTarget.MyHitBox, damage, this);
    }
}