using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;



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

    // Sprite Atlas des éléments de type "Water"
    [SerializeField]
    private SpriteAtlas waterAtlas = default;

    // Tableau des données des maps
    [SerializeField]
    private Texture2D[] mapData = default;

    // Tableau des éléments de la map
    [SerializeField]
    private MapElement[] mapElements = default;

    // Position de la caméra dans le jeu 
    private Vector3 WorldStartPosition { get => Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }

    // Dictionnaire des élements de type Water (Coordonnées => Element)
    private readonly Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();



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
                        if (newElement.MyTileTag == "Water")
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
            
            //         T
            //   | 2 | 4 | 7 |
            // L | 1 |   | 6 | R
            //   | 0 | 3 | 5 | 
            //         B

            // bordure Left Top
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
                UpdateTileSprite(tile.Value, "0");
            }

            // Bordure Top
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
                UpdateTileSprite(tile.Value, "1");
            }

            // bordure Right Top
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
                UpdateTileSprite(tile.Value, "2");
            }

            // Bordure Left
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
                UpdateTileSprite(tile.Value, "3");
            }

            // Bordure Right
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
                UpdateTileSprite(tile.Value, "4");
            }

            // bordure Left Bottom
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
                UpdateTileSprite(tile.Value, "5");
            }

            // Bordure Bottom
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
                UpdateTileSprite(tile.Value, "6");
            }

            // bordure Right Bottom
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
                UpdateTileSprite(tile.Value, "7");
            }

            //Bordure Right Top Bottom
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
                //UpdateTileSprite(tile.Value, "8");
            }

            //Bordure Left Top Bottom
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
                UpdateTileSprite(tile.Value, "9");
            }

            // Bordure Top Bottom
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
                UpdateTileSprite(tile.Value, "10");
            }

            // Bordure Left Right
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
                UpdateTileSprite(tile.Value, "11");
            }

            //Bordure Bottom Left Right
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
                UpdateTileSprite(tile.Value, "12");
            }

            //Bordure Top Left Right
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
                UpdateTileSprite(tile.Value, "13");
            }

            // Jointure Right Bottom
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                /*
                //Copie la cellule à la même position
                GameObject tileCopy = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);

                // Remplace l'image par celle de la jointure correspondante
                tileCopy.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");

                // Positionne la copie au-dessus de l'image
                tileCopy.GetComponent<SpriteRenderer>().sortingOrder = 1;
                */
                UpdateTileSprite(tile.Value, "14", true);
            }

            // Jointure Left Top
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                /*
                //Copie la cellule à la même position
                GameObject tileCopy = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);

                // Remplace l'image par celle de la jointure correspondante
                tileCopy.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");

                // Positionne la copie au-dessus de l'image
                tileCopy.GetComponent<SpriteRenderer>().sortingOrder = 1;
                */
                UpdateTileSprite(tile.Value, "15", true);
            }

            // Jointure Right Top
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                /*
                //Copie la cellule à la même position
                GameObject tileCopy = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);

                // Remplace l'image par celle de la jointure correspondante
                tileCopy.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");

                // Positionne la copie au-dessus de l'image
                tileCopy.GetComponent<SpriteRenderer>().sortingOrder = 1;
                */
                UpdateTileSprite(tile.Value, "16", true);
            }

            // Jointure Left Bottom
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                /*
                //Copie la cellule à la même position
                GameObject tileCopy = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);

                // Remplace l'image par celle de la jointure correspondante
                tileCopy.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");

                // Positionne la copie au-dessus de l'image
                tileCopy.GetComponent<SpriteRenderer>().sortingOrder = 1;
                */
                UpdateTileSprite(tile.Value, "17", true);
            }
           
            // Cellule entourée d'eau en haut, en bas, à gauche et à droite
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                //int randomChance = UnityEngine.Random.Range(0, 100);

                // 15% de chance d'ajouter une cellule de décoration (Vagues)
                if (GetRandomNumber() < 15)
                {
                    //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                    UpdateTileSprite(tile.Value, "19");
                }
            }

            // Cellule quasiement entourée d'eau
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                //int randomChance = UnityEngine.Random.Range(0, 100);

                // 15% de chance d'ajouter une cellule de décoration (Nénuphar)
                if (GetRandomNumber() < 10)
                {
                    //tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                    UpdateTileSprite(tile.Value, "20");
                }
            }
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
                    if (waterTiles.ContainsKey(new Point(currentPoint.XPosition + x, currentPoint.YPosition + y)))
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
        return composition;
    }


    private void UpdateTileSprite(GameObject tile, string atlasIndex, bool useCopy = false)
    {
        GameObject updatedTile;
        
        if (useCopy)
        {
            // Copie la cellule à la même position
            updatedTile = Instantiate(tile, tile.transform.position, Quaternion.identity, map);
        }
        else
        {
            // Utlisation de la cellule originale
            updatedTile = tile;
        }

        // Remplace l'image par celle de la jointure correspondante
        updatedTile.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite(atlasIndex);

        if (useCopy)
        {
            // Positionne la copie au-dessus de l'image
            updatedTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    private int GetRandomNumber()
    {
        return UnityEngine.Random.Range(0, 100);
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
    public int XPosition { get; set; }

    // Coordonnée Y
    public int YPosition { get; set; }


    // Constructeur
    public Point(int x, int y)
    {
        this.XPosition = x;
        this.YPosition = y;
    }
}