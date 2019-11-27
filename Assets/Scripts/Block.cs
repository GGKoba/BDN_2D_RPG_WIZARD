using System;
using UnityEngine;



/// <summary>
/// Classe pour la gestion de la ligne de mire
/// </summary>
[Serializable]
public class Block
{
    // Paire de blocs utilisés pour définir ce que l'on ne peut pas atteindre
    // Bloc 1
    [SerializeField]
    private GameObject first = default;
    // Bloc 2
    [SerializeField]
    private GameObject second = default;


    /// <summary>
    /// Active une paire de blocs
    /// </summary>
    public void Activate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }

    /// <summary>
    /// Désactive une paire de blocs
    /// </summary>
    public void Deactivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }
}
