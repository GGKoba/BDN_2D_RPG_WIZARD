using System.Collections.Generic;



// Gestion de la mise à jour de la Stack
public delegate void StackUpdated();


/// <summary>
/// Classe de gestion des mises à jour de la Stack d'un emplacement
/// </summary>
class ObservableStack<T> : Stack<T>
{
    // Evènement d'ajout dans la stack d'un emplacement
    public event StackUpdated OnPush;

    // Evènement de retrait dans la stack d'un emplacement
    public event StackUpdated OnPop;

    // Evènement de nettoyage de la stack d'un emplacement
    public event StackUpdated OnClear;


    /// <summary>
    /// Ajout dans la stack
    /// </summary>
    /// <param name="item">Item à ajouter</param>
    public new void Push(T item)
    {
        // Appelle Push sur la classe mère
        base.Push(item);

        // S'il existe un abonnement à cet évènement
        if (OnPush != null)
        {
            // Déclenchement de l'évènement d'ajout dans la stack d'un emplacement
            OnPush();
        }

    }

    /// <summary>
    /// Retrait dans la stack
    /// </summary>
    public new T Pop()
    {
        // Appelle Pop sur la classe mère
        T item = base.Pop();

        // S'il existe un abonnement à cet évènement
        if (OnPop != null)
        {
            // Déclenchement de l'évènement de retrait dans la stack d'un emplacement
            OnPop();
        }

        // Retourne l'item
        return item;
    }

    /// <summary>
    /// Nettoyage de la stack
    /// </summary>
    public new void Clear()
    {
        // Appelle Clear sur la classe mère
        base.Clear();

        // S'il existe un abonnement à cet évènement
        if (OnClear != null)
        {
            // Déclenchement de l'évènement nettoyage de la stack d'un emplacement
            OnClear();
        }
    }

}
