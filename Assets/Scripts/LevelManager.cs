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
        // Hauteur de la 1ère map
        int height = mapData[0].height;

        // Largeur de la 1ère map
        int width = mapData[0].width;

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

                        // Si l'objet est un arbre
                        if (newElement.MyTileTag == "Tree")
                        {
                            // MAJ de son positionnement sur le layer (z-index)
                            // Permet d'avoir un espace entre 2 arbres (height * 2 - y * 2) et que les arbres de derrière ne passent pas au-dessus des arbres de devant]
                            gameObjectElement.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;   
                        }

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
