using UnityEngine;



/// <summary>
/// Classe de gestion du Débuff de Feu
/// </summary>
class IgniteDebuff : Debuff
{
    // Propriété d'accès aux dégats par tick du débuff
    public float MyTickDamage { get; set; }

    // Temps d'application du débuff
    private float elapsed;


    // Constructeur
    public IgniteDebuff()
    {
        // Par défaut, le débuff dure 20s
        MyDuration = 20;

        // Par défaut, le débuff cause 5 points de dégâts par tick
        MyTickDamage = 5;
    }

    /// <summary>
    /// Actualise le débuff : Surcharge la fonction Update du script Character
    /// </summary>
    public override void Update()
    {
        // Actualise le temps d'application du débuff
        elapsed += Time.deltaTime;

        // Si le temps du tick est dépassé (un tick toutes les 4s)
        if (elapsed >= MyDuration/MyTickDamage)
        {
            // Inflige les dégâts le debuff
            character.TakeDamage(MyTickDamage, null);

            // Réinitialise le temps d'application du débuff
            elapsed = 0;
        }


        // Appelle Update sur la classe mère
        base.Update();  
    }

    /// <summary>
    /// Retire le debuff : Surcharge la fonction Update du script Character
    /// </summary>
    public override void Remove()
    {
        // Réinitialise le temps d'application du débuff
        elapsed = 0;

        // Appelle Remove sur la classe mère
        base.Remove();
    }
}

