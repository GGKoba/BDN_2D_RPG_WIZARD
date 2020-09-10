using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;



/// <summary>
/// Classe de gestion du Pathfinding
/// </summary>
public class AStar : MonoBehaviour
{
    // Map à vérifier
    [SerializeField]
    private Tilemap tilemap = default;

    // Propriété d'accès à la Map
    public Tilemap MyTilemap { get => tilemap; }

    // Noeud courant
    private Node current;

    // Chemin
    private Stack<Vector3> path;

    // Liste des noeuds à vérifier
    private HashSet<Node> openList;

    // Liste des noeuds vérifiés
    private HashSet<Node> closedList;

    // Liste de tous les noeuds
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    // Position de départ / d'arrivée
    private Vector3Int startPos, goalPos;

    // Liste des zones où l'on ne peut pas aller en diagonale
    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();

    // Propriété d'accès à la liste des zones où l'on ne peut pas aller en diagonale
    public static HashSet<Vector3Int> MyNoDiagonalTiles { get => noDiagonalTiles; }


    /// <summary>
    /// Algorithme de parcours
    /// </summary>
    /// <param name="start">Noeud de départ</param>
    /// <param name="goal">Noeud d'arrivée</param>
    /// <returns></returns>
    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = MyTilemap.WorldToCell(start);
        goalPos = MyTilemap.WorldToCell(goal);

        current = GetNode(startPos);

        // Liste des noeuds à verifier
        openList = new HashSet<Node>();

        // Liste des noeuds déjà verifiés
        closedList = new HashSet<Node>();

        //Réinitialise les noeuds
        foreach (KeyValuePair <Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }
        allNodes.Clear();

        // Ajoute le noeud courant dans la liste à vérifier
        openList.Add(current);

        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbours = FindNeighbours(current.Position);

            ExamineNeighbours(neighbours, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }

        if (path != null)
        {
            return path;
        }


        return null;

    }

    /// <summary>
    /// Parcours des voisins
    /// </summary>
    /// <param name="parentPosition"></param>
    /// <returns></returns>
    private List<Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();

        // Parcours de tous les noeuds voisins
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0)
                {
                    Vector3Int neighbourPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                    if (neighbourPosition != startPos && !GameManager.MyInstance.MyBlocked.Contains(neighbourPosition))
                    {
                        Node neighbour = GetNode(neighbourPosition);
                        neighbours.Add(neighbour);
                    }

                }
            }
        }

        return neighbours;
    }

    /// <summary>
    /// Check des voisins
    /// </summary>
    /// <param name="neighbours"></param>
    /// <param name="current"></param>
    private void ExamineNeighbours(List<Node> neighbours, Node current)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbour = neighbours[i];

            if (!ConnectedDiagonally(current, neighbour))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbour.Position, current.Position);

            // On passe la zone si c'est une diagonale
            if (gScore == 14 && noDiagonalTiles.Contains(neighbour.Position) && noDiagonalTiles.Contains(current.Position))
            {
                continue;
            }

            if (openList.Contains(neighbour))
            {
                if (current.G + gScore < neighbour.G)
                {
                    CalcValues(current, neighbour, goalPos, gScore);
                }
            }
            else if (!closedList.Contains(neighbour))
            {
                CalcValues(current, neighbour, goalPos, gScore);

                // Vérfiier si la liste ne contient pas déjà le noeud
                if (!openList.Contains(neighbour)) 
                {
                    // Ajoute le noeud dans la liste à vérifier
                    openList.Add(neighbour); 
                }
            }
        }
    }

    /// <summary>
    /// Vérification des diagonales
    /// </summary>
    /// <param name="currentNode">Noeud courant</param>
    /// <param name="neighbour">Noeud voisin</param>
    /// <returns></returns>
    private bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        // Récupère la direction
        Vector3Int direction = currentNode.Position - neighbour.Position;

        // Récupère la position des noeuds
        Vector3Int first = new Vector3Int(currentNode.Position.x + (direction.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(currentNode.Position.x, currentNode.Position.y + (direction.y * -1), currentNode.Position.z);

        // Vérifie si les noeuds sont "praticables"
        if (GameManager.MyInstance.MyBlocked.Contains(first) || GameManager.MyInstance.MyBlocked.Contains(second))
        {
            return false;
        }

        // Retoure que c'est OK
        return true;
    }

    /// <summary>
    /// Calcul du score G
    /// </summary>
    /// <param name="neighbour">Noeud voisin</param>
    /// <param name="current">Noeud courant</param>
    /// <returns></returns>
    private int DetermineGScore(Vector3Int neighbour, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbour.x;
        int y = current.y - neighbour.y;

        if (Math.Abs(x - y) % 2 == 1)
        {
            // 10 pour horizontal ou vertical
            gScore = 10;
        }
        else
        {
            // 10 pour diagonale
            gScore = 14;
        }

        return gScore;
    }

    /// <summary>
    /// Mise à jour du noeud
    /// </summary>
    /// <param name="current"></param>
    private void UpdateCurrentTile(ref Node current)
    {
        // Retire le noeud courant dans la liste à vérifier
        openList.Remove(current);

        // Le noeud courant est ajouté dans la liste des noeuds déjà verifiés
        closedList.Add(current);

        // Si le noeud est dans la liste à verifier, on trie sur les valeurs de F
        if (openList.Count > 0) 
        {
            // Tri sur les valeurs de F pour récupérer la valeur la plus petite
            current = openList.OrderBy(x => x.F).First();
        }
    }

    /// <summary>
    /// Stack du chemin
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private Stack<Vector3> GeneratePath(Node current)
    {
        // Si le noeud est l'arrivée, on a le chemin
        if (current.Position == goalPos)
        {
            // Création d'une stack contenant le chemin
            Stack<Vector3> finalPath = new Stack<Vector3>();

            // Ajout des noeuds du chemin
            while (current != null)
            {
                // Ajout le noeud courant du chemin
                finalPath.Push(MyTilemap.CellToWorld(current.Position));
               
                // Remontée sur tous les parents jusqu'au départ
                current = current.Parent;
            }

            // Retourne le chemin
            return finalPath;
        }

        return null;

    }

    /// <summary>
    /// Calcul des scores
    /// </summary>
    /// <param name="parent">Noeud parent</param>
    /// <param name="neighbour">Noeud voisin</param>
    /// <param name="goalPos">Noeud d'arrivée</param>
    /// <param name="cost">Coût du déplacement</param>
    private void CalcValues(Node parent, Node neighbour, Vector3Int goalPos, int cost)
    {
        // Actualise le parent
        neighbour.Parent = parent;

        // Calcul le G (G du parent + coût pour se déplacer sur ce noeud)
        neighbour.G = parent.G + cost;

        // Calcul de H (distance entre le noeud et l'arrivée x10)
        neighbour.H = ((Math.Abs((neighbour.Position.x - goalPos.x)) + Math.Abs((neighbour.Position.y - goalPos.y))) * 10);

        // Calcul de F (G + H)
        neighbour.F = neighbour.G + neighbour.H;
    }


    /// <summary>
    /// Récupèration d'un noeud
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
}


/// <summary>
/// Classe Noeud
/// </summary>
public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node Parent { get; set; }
    public Vector3Int Position { get; set; }

    //private TextMeshProUGUI MyText { get; set; }

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}

