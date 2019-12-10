using UnityEngine;
using UnityEngine.Tilemaps;



/// <summary>
/// Classe de gestion de la camera
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // Cible de la caméra
    private Transform target;

    // Tailles mini/maxi 
    private float xMin, xMax, yMin, yMax;

    // Référence sur la Tilemap
    [SerializeField]
    private Tilemap tileMap = default;


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Position de la cellule en bas à gauche
        Vector3 minTile = tileMap.CellToWorld(tileMap.cellBounds.min);

        // Position de la cellule en haut à droite
        Vector3 maxTile = tileMap.CellToWorld(tileMap.cellBounds.max);

        // Limites de la caméra
        SetLimits(minTile, maxTile);
    }

    /// <summary>
    /// LateUpdate : Appellée après le FixedUpdate du Character
    /// </summary>
    private void LateUpdate()
    {
        // Position de la caméra
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
    }

    /// <summary>
    /// Définit les limites de la caméra
    /// </summary>
    /// <param name="minTile">Position de la cellule en bas à gauche</param>
    /// <param name="maxTile">Position de la cellule en haut à droite</param>
    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        // Référence sur la caméra principale
        Camera camera = Camera.main;

        // Hauteur de la caméra
        float height = 2.0f * camera.orthographicSize;

        // Largeur de la caméra
        float width = height * camera.aspect;

        // Position x minimum (ne pourra pas dépasser à gauche)
        xMin = minTile.x + width / 2;

        // Position x maximum  (ne pourra pas dépasser à droite)
        xMax = maxTile.x - width / 2;

        // Position y minimum  (ne pourra pas dépasser en bas)
        yMin = minTile.y + height / 2;

        // Position y maximum  (ne pourra pas dépasser en haut)
        yMax = maxTile.y - height / 2;
    }
}
