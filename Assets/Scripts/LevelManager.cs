using System;
using UnityEngine;



/// <summary>
/// Classe de gestion des maps
/// </summary>
public class LevelManager : MonoBehaviour
{
    // Conteneur des éléments
    [SerializeField]
    private Transform map = default;

    // Sprite par défaut
    [SerializeField]
    private Sprite defaultTile = default;

    // Tableau des données des maps
    [SerializeField]
    private Texture2D[] mapData = default;

    // Tableau des éléments de la map
    [SerializeField]
    private MapElement[] mapElements = default;

    // Position de la caméra dans le jeu 
    private Vector3 WorldStartPosition { get => Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Construction de la map
        GenerateMap();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        
    }

    /// <summary>
    /// Construction de la map
    /// </summary>
    private void GenerateMap()
    {
        // Boucle sur tous les layers de map
        for (int i = 0; i < mapData.Length; i++)
        {
            // Boucle sur toutes les colonnes du layer
            for (int x = 0; x < mapData[i].width; x++)
            {
                // Boucle sur toutes les lignes du layer
                for (int y = 0; y < mapData[i].height; y++)
                {
                    // Récupération de la couleur du pixel
                    Color cellColor = mapData[i].GetPixel(x, y);
   
                    // Vérification qu'il existe un élément qui a la couleur du pixel
                    MapElement newElement = Array.Find(mapElements, e => ColorUtility.ToHtmlStringRGB(e.MyTileColor) == ColorUtility.ToHtmlStringRGB(cellColor));
                    Debug.Log(newElement);
                    // S'il y a un élément correspondant
                    if (newElement != null)
                    {
                        // Création de l'objet (Prefab de l'élément correspondant)
                        GameObject gameObjectElement = Instantiate(newElement.MyTilePrefab);

                        // Calcul des coordonnées de positionnement de l'objet
                        float xPosition = WorldStartPosition.x + (defaultTile.bounds.size.x * x);
                        float yPosition = WorldStartPosition.y + (defaultTile.bounds.size.y * y);

                        // Placement de l'objet sur la map
                        gameObjectElement.transform.position = new Vector2(xPosition, yPosition);

                        // Ajoute les objets comme des enfants de l'élement Map
                        gameObjectElement.transform.parent = map;
                    }
                }
            }
        }
    }
}



/// <summary>
/// Classe des éléments de map
/// </summary>
[Serializable]
public class MapElement
{
    // Tag de l'élément
    [SerializeField]
    private string tileTag = default;

    // Couleur de l'élément
    [SerializeField]
    private Color tileColor = default;

    // Prefab de l'élément
    [SerializeField]
    private GameObject tilePrefab = default;

    
    // Propriété d'accès au tag de l'élément
    public string MyTileTag { get => tileTag; }

    // Propriété d'accès à la couleur de l'élément
    public Color MyTileColor { get => tileColor; }

    // Propriété d'accès à la préfab de l'élément
    public GameObject MyTilePrefab { get => tilePrefab; }
}
