using System;
using System.Collections.Generic;
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

    // Dictionnaire des élements de type Water (Coordonnées => Element)
    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Construction de la map
        GenerateMap();
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

                        // Si l'objet est a un tag "Tree"
                        if (newElement.MyTileTag == "Tree")
                        {
                            // MAJ de son positionnement sur le layer (z-index)
                            // Permet d'avoir un espace entre 2 arbres (height * 2 - y * 2) et que les arbres de derrière ne passent pas au-dessus des arbres de devant]
                            gameObjectElement.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;   
                        }

                        // Si l'objet est a un tag "Water"
                        if (newElement.MyTileTag == "Tree")
                        {
                            // On ajoute l'objet dans le dictionnaire des éléments de type Water, avec ses coordonnées sur la map
                            waterTiles.Add(new Point(x, y), gameObjectElement);
                        }

                        // Ajoute les objets comme des enfants de l'élement Map
                        gameObjectElement.transform.parent = map;
                    }
                }
            }
        }

        // Une fois la map générée, analyse des cellues de type "Water"
        CheckWater();

    }

    /// <summary>
    /// Vérifie toutes les cellues autour des cellules de type "Water" pour adapter le sprite
    /// </summary>
    private void CheckWater()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);
        }
    }

    /// <summary>
    /// Verifie tous les voisins de la cellule
    /// </summary>
    /// <param name="currentPoint">La position de le cellule</param>
    public string TileCheck(Point currentPoint)
    {
        // Chaine qui aura tous les types d'éléments de ses voisins [W(ater) OR E(mpty)]
        string composition = string.Empty;

        // Boucle sur les cellules autour de celle du point
        // | X=-1|X= 0| X=+1|
        // |-1,+1|0,+1|+1,+1| Y=+1
        // |-1, 0|0, 0|+1, 0| Y=0
        // |-1,-1|0,-1|+1,-1| Y=-1

        // Boucle sur les colonnes
        for (int x = -1; x <= 1; x++)
        {
            // Boucle sur les lignes de la colonne
            for (int y = -1; y <= 1; y++)
            {
                // Si je ne suis sur le point courant
                if (x != 0 || y != 0)
                {
                    // Si le dictionnaire contient les coordonnées de mon point
                    if (waterTiles.ContainsKey(new Point(currentPoint.xPosition + x, currentPoint.yPosition + y)))
                    {
                        // La cellule est de type "Water"
                        composition += "W";
                    }
                    else
                    {
                        // La cellule est de type "Empty"
                        composition += "E";
                    }
                }
            }
        }

        Debug.Log(composition);
        return composition;
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



/// <summary>
/// Structure des coordonnées de la map
/// </summary>
public struct Point
{
    // Coordonnée X
    public int xPosition { get; set; }

    // Coordonnée Y
    public int yPosition { get; set; }


    // Constructeur
    public Point(int x, int y)
    {
        this.xPosition = x;
        this.yPosition = y;
    }
}