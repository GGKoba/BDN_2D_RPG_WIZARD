using System.Collections.Generic;



// Gestion de la mise à jour de la Stack
public delegate void StackUpdated();


/// <summary>
/// Classe de gestion des mises à jour de la Stack d'un emplacement
/// </summary>
public class ObservableStack<T> : Stack<T>
{
    // Evènement d'ajout dans la stack d'un emplacement
    public event StackUpdated PushEvent;

    // Evènement de retrait dans la stack d'un emplacement
    public event StackUpdated PopEvent;

    // Evènement de nettoyage de la stack d'un emplacement
    public event StackUpdated ClearEvent;

    // Constructeur vide
    public ObservableStack() { }

    // Constructeur paramétré
    public ObservableStack(ObservableStack<T> itemStack) : base(itemStack) { }


    /// <summary>
    /// Ajout dans la stack
    /// </summary>
    /// <param name="item">Item à ajouter</param>
    public new void Push(T item)
    {
        // Appelle Push sur la classe mère
        base.Push(item);

        // S'il existe un abonnement à cet évènement
        if (PushEvent != null)
        {
            // Déclenchement de l'évènement d'ajout dans la stack d'un emplacement
            PushEvent.Invoke();
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
        if (PopEvent != null)
        {
            // Déclenchement de l'évènement de retrait dans la stack d'un emplacement
            PopEvent.Invoke();
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
        if (ClearEvent != null)
        {
            // Déclenchement de l'évènement nettoyage de la stack d'un emplacement
            ClearEvent.Invoke();
        }
    }
}
