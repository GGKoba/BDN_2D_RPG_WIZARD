using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Classe de gestion des messages liés à l'avancement des quêtes
/// </summary>
public class MessageFeedManager : MonoBehaviour
{
    // Instance de classe (singleton)
    private static MessageFeedManager instance;

    // Propriété d'accès à l'instance
    public static MessageFeedManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                // Retourne l'object de type MessageFeedManager (doit être unique)
                instance = FindObjectOfType<MessageFeedManager>();
            }

            return instance;
        }
    }

    // Prefab des messages de quêtes
    [SerializeField]
    private GameObject messagePrefab = default;


    /// <summary>
    /// Envoi un message
    /// </summary>
    /// <param name="message">Message</param>
    public void WriteMessage(string message)
    {
        // Si le message n'est pas vide
        if (message != string.Empty)
        {
            // Instantie un objet de message
            GameObject messageObject = Instantiate(messagePrefab, transform);

            // Actualise le texte du message
            messageObject.GetComponent<Text>().text = message;

            // Positionne le nouveau message en haut de liste
            messageObject.transform.SetAsFirstSibling();

            // Détruit le message après 3s
            Destroy(messageObject, 2);
        }
    }

    /// <summary>
    /// Envoi un message
    /// </summary>
    /// <param name="message">Message</param>
    public void WriteMessage(string message, Color color)
    {
        // Si le message n'est pas vide
        if (message != string.Empty)
        {
            // Instantie un objet de message
            GameObject messageObject = Instantiate(messagePrefab, transform);

            // Récupère la zone de texte
            Text textObject = messageObject.GetComponent<Text>();

            // Actualise le texte et la couleur du message
            textObject.text = message;
            textObject.color = color;

            // Positionne le nouveau message en haut de liste
            messageObject.transform.SetAsFirstSibling();

            // Détruit le message après 3s
            Destroy(messageObject, 2);
        }
    }
}