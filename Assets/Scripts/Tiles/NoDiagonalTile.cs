using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;



/// <summary>
/// Classe des éléments de la map de type "NoDiagonal"
/// </summary>
public class NoDiagonalTile : Tile
{
    /// <summary>
    /// StartUp : Surcharge la fonction StartUp du script Tile
    /// </summary>
    /// <param name="position">Position de la cellule</param>
    /// <param name="tilemap">Layer de la cellule</param>
    /// <param name="go">Objet de la cellule</param>
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Ajoute la zone dans la liste des zones "non diagonales"
        AStar.MyNoDiagonalTiles.Add(position);

        // Appelle StartUp sur la classe mère
        return base.StartUp(position, tilemap, go);
    }


    // Exécution seulement dans l'éditeur UNITY
#if UNITY_EDITOR
    /// <summary>
    /// Ajoute un sous-menu dans le menu Assets : Tiles > TreeTile à partir de Assets > Create
    /// </summary>
    [MenuItem("Assets/Create/Tiles/NoDiagonalTile")]
    public static void CreateNoDiagonalTile()
    {
        // Affiche la fenêtre de sauvegarde [Titre : Enregistrer NoDiagonalTile - Default Name : noDiagonalTile - Extension : .asset - Message : Enregistrer NoDiagonalTile - Path : Assets]
        string path = EditorUtility.SaveFilePanelInProject("Enregistrer NoDiagonalTile", "noDiagonalTile", "asset", "Enregistrer NoDiagonalTile", "Assets");

        if (path == "")
        {
            return;
        }

        // Ajoute l'asset "TreeTile"
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GrassTile>(), path);
    }
#endif
}