using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;



/// <summary>
/// Classe des élements de la map de type "Arbre"
/// </summary>
public class TreeTile : Tile
{
    /// <summary>
    /// StartUp : Surcharge la fonction StartUp du script Tile
    /// </summary>
    /// <param name="position">Position de la cellule</param>
    /// <param name="tilemap">Layer de la cellule</param>
    /// <param name="go">Objet de la cellule</param>
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // MAJ de son positionnement sur le layer (z-index)
        // Permet d'avoir un espace entre 2 arbres (-position.y * 2) et que les arbres de derrière ne passent pas au-dessus des arbres de devant]
        if (go != null)
        {
            go.GetComponent<SpriteRenderer>().sortingOrder = -position.y * 2;
        }

        // Appelle StartUp sur la classe mère
        return base.StartUp(position, tilemap, go);
    }


    // Exécution seulement dans l'éditeur UNITY
#if UNITY_EDITOR
    /// <summary>
    /// Ajoute un sous-menu dans le menu Assets : Tiles > TreeTile à partir de Assets > Create
    /// </summary>
    [MenuItem("Assets/Create/Tiles/TreeTile")]
    public static void CreateTreeTile()
    {
        // Affiche la fenêtre de sauvegarde [Titre : Enregistrer TreeTile - Default Name : treeTile - Extension : .asset - Message : Enregistrer TreeTile - Path : Assets]
        string path = EditorUtility.SaveFilePanelInProject("Enregistrer TreeTile", "treeTile", "asset", "Enregistrer TreeTile", "Assets");

        if (path == "")
        {
            return;
        }

        // Ajoute l'asset "TreeTile"
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TreeTile>(), path);
    }
#endif
}