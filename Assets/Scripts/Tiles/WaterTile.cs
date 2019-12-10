using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;



/// <summary>
/// Classe des élements de la map de type "Eau"
/// </summary>
public class WaterTile : Tile
{
    // Tableau des sprites de type "Eau"
    [SerializeField]
    private Sprite[] waterSprites = default;


    /// <summary>
    /// StartUp : Ecrase la fonction StartUp du script Tile
    /// </summary>
    /// <param name="position">Position de la cellule</param>
    /// <param name="tilemap">Layer de la cellule</param>
    /// <param name="go">Objet de la cellule</param>
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Appelle StartUp sur la classe mère
        return base.StartUp(position, tilemap, go);
    }

    /// <summary>
    /// Mise à jour de la cellule : Ecrase la fonction RefreshTile du script Tile
    /// </summary>
    /// <param name="position">Position de la cellule</param>
    /// <param name="tilemap">Layer de la cellule</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // Boucle sur les colonnes
        for (int x = -1; x <= 1; x++)
        {
            // Boucle sur les lignes de la colonne
            for (int y = -1; y <= 1; y++)
            {
                // Coordonnées de cellule analysée
                Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, position.z);

                if (HasWater(position, tilemap))
                {
                    tilemap.RefreshTile(tilePosition);
                }
            }
        }
    }

    /// <summary>
    /// Informations de la cellule : Ecrase la fonction GetTileData du script Tile
    /// </summary>
    /// <param name="position">Position de la cellule</param>
    /// <param name="tilemap">Layer de la cellule</param>
    /// <param name="tileData">Données de la cellule</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // Appelle GetTileData sur la classe mère
        base.GetTileData(position, tilemap, ref tileData);

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
                    // Si la cellule est de type "Eau"
                    if (HasWater(new Vector3Int(position.x + x, position.y + y, position.z), tilemap))
                    {
                        // La cellule est de type "Water"
                        composition += 'W';
                    }
                    else
                    {
                        // La cellule est de type "Empty"
                        composition += 'E';
                    }
                }
            }
        }


        //         T
        //   | 2 | 4 | 7 |
        // L | 1 |   | 6 | R
        //   | 0 | 3 | 5 | 
        //         B


        // Nombre aléatoire
        int randomVal = Random.Range(0, 100);

        // Nenuphars
        if (randomVal < 15)
        {
            tileData.sprite = waterSprites[46];
        }
        // Vagues
        else if (randomVal >= 15 && randomVal < 35)
        {
            tileData.sprite = waterSprites[48];
        }
        // Original
        else
        {
            tileData.sprite = waterSprites[47];
        }


        // Bordure Left Bottom Top Right
        if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[0];
        }
        // Bordure Left Top
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[1];
        }
        // Jointure Bottom Right
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[2];
        }
        // Bordure Top
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[3];
        }
        // Bordure Right Top
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[4];
        }
        // Jointure Left Bottom
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[5];
        }
        // Bordure Top + Jointure Left Bottom
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[6];
        }
        // Bordure Left
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[7];
        }
        // Bordure Left + Jointure Bottom Right
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'E' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[8];
        }
        // Bordure Left + Jointure Top Right
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[9];
        }
        // Bordure Left + Jointure Bottom Top Right
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'E' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[10];
        }
        // Bordure Right + Jointure Left Bottom
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[11];
        }
        // Bordure Right
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[12];
        }
        // Bordure Right + Jointure Left Top
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[13];
        }
        // Bordure Top + Jointure Bottom Right
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[14];
        }
        // Bordure Right + Jointure Left Bottom Top
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[15];
        }
        // Bordure Top + Jointure Left Bottom Right
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[16];
        }
        // Bordure Left Bottom
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[17];
        }
        // Bordure Left Bottom + Jointure top Right
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[18];
        }
        // Bordure Bottom
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[19];
        }
        // Bordure Bottom + Jointure Top Right
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[20];
        }
        // Bordure Bottom + Jointure Left Top
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = waterSprites[21];
        }
        // Bordure Bottom + Jointure Left Top Right
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = waterSprites[22];
        }
        // Bordure Bottom Right
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[23];
        }
        // Bordure Bottom Right + Jointure Left Top
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[24];
        }
        // Bordure Bottom Top Right
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[25];
        }
        // Bordure Left Bottom Top
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[26];
        }
        // Bordure Bottom Top
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = waterSprites[27];
        }
        // Bordure Left Right
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[28];
        }
        // Bordure Left Bottom Right
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[29];
        }
        // Bordure Left Top Right
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = waterSprites[30];
        }
        // Jointure Left Bottom + Bottom Right
        else if (composition == "EWWWWEWW")
        {
            tileData.sprite = waterSprites[31];
        }
        // Jointure Left Top + Left Bottom + Top Right 
        else if (composition == "EWEWWWWE")
        {
            tileData.sprite = waterSprites[32];
        }
        // Jointure Left Top + Left Bottom
        else if (composition == "EWEWWWWW")
        {
            tileData.sprite = waterSprites[33];
        }
        // Jointure Bottom Right
        else if (composition == "WWWWWEWW")
        {
            tileData.sprite = waterSprites[34];
        }
        // Jointure Left Top + Top Right
        else if (composition == "WWEWWWWE")
        {
            tileData.sprite = waterSprites[35];
        }
        // Jointure Top Right
        else if (composition == "WWWWWWWE")
        {
            tileData.sprite = waterSprites[36];
        }
        // Jointure Left Bottom
        else if (composition == "EWWWWWWW")
        {
            tileData.sprite = waterSprites[37];
        }
        // Jointure Left Top
        else if (composition == "WWEWWWWW")
        {
            tileData.sprite = waterSprites[38];
        }
        // Jointure Left Bottom + Top Right
        else if (composition == "EWWWWWWE")
        {
            tileData.sprite = waterSprites[39];
        }
        // Jointure Left Bottom + Bottom Right + Top Right
        else if (composition == "EWWWWEWE")
        {
            tileData.sprite = waterSprites[40];
        }
        // Jointure Bottom Right + Top Right
        else if (composition == "WWWWWEWE")
        {
            tileData.sprite = waterSprites[41];
        }
        // Jointure Left Top + Bottom Right
        else if (composition == "WWEWWEWW")
        {
            tileData.sprite = waterSprites[42];
        }
        // Jointure Left Top + Left Bottom + Bottom Right
        else if (composition == "EWEWWEWW")
        {
            tileData.sprite = waterSprites[43];
        }
        // Jointure Left Top +  Bottom Right + Top Right
        else if (composition == "WWEWWEWE")
        {
            tileData.sprite = waterSprites[44];
        }
        // Jointure Left Top +  Left Bottom + Bottom Right + Top Right
        else if (composition == "EWEWWEWE")
        {
            tileData.sprite = waterSprites[45];
        }
    }

    /// <summary>
    /// La cellule est-elle de type "Eau"
    /// </summary>
    /// <param name="position">Position de la cellule</param>
    /// <param name="tilemap">Layer de la cellule</param>
    private bool HasWater(Vector3Int position, ITilemap tilemap)
    {
        return tilemap.GetTile(position) == this;
    }


// Exécution seulement dans l'éditeur UNITY
#if UNITY_EDITOR
    /// <summary>
    /// Ajoute un sous-menu dans le menu Assets : Tiles > WaterTile à partir de Assets > Create
    /// </summary>
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateWaterTile()
    {
        // Affiche la fenêtre de sauvegarde [Titre : Enregistrer WaterTile - Default Name : waterTile - Extension : .asset - Message : Enregistrer WaterTile - Path : Assets]
        string path = EditorUtility.SaveFilePanelInProject("Enregistrer WaterTile", "waterTile", "asset", "Enregistrer WaterTile", "Assets");

        if (path == "")
        {
            return;
        }

        // Ajoute l'asset "WaterTile"
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }
#endif
}