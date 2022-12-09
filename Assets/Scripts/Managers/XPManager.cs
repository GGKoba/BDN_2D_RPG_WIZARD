/// <summary>
/// Classe de gestion de l'expérience
/// </summary>
static class XPManager
{
    /// <summary>
    /// Calcule l'expérience donnée par un personnage
    /// </summary>
    /// <param name="enemy">Personnage ennemi</param>
    public static int CalculateXP(Enemy enemy)
    {
        // Experience de base du joueur
        int baseXP = (Player.MyInstance.MyLevel * 5) + 45;

        // Level qui ne rapporte aucune experience
        int grayLevel = CalculateGrayLevel();

        // Expérience gagnée
        int totalXP = 0;

        // Si le personnage a un niveau plus élevé que le joueur
        if (enemy.MyLevel >= Player.MyInstance.MyLevel)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (enemy.MyLevel - Player.MyInstance.MyLevel)));
        }
        // Si le personnage a un niveau qui rapporte de l'experience
        else if (enemy.MyLevel > grayLevel)
        {
            // Calcul du "ZeroDifference"
            totalXP = baseXP * (1 - (Player.MyInstance.MyLevel - enemy.MyLevel) / CalculateZD());
        }
        return totalXP;
    }

    /// <summary>
    /// Calcule l'expérience donnée par un personnage
    /// </summary>
    /// <param name="quest">Quête</param>
    public static int CalculateXP(Quest quest)
    {
        // Si le joueur a un niveau moins élevé ou égal au niveau de la quête + 5
        if (Player.MyInstance.MyLevel <= quest.MyLevel + 5)
        {
            // 100% 
            return quest.MyXp;
        }
        // Si le joueur a un niveau égal au niveau de la quête + 6
        else if (Player.MyInstance.MyLevel == quest.MyLevel + 6)
        {
            // 80% 
            return (int)((quest.MyXp * 0.8 / 5) * 5);
        }
        // Si le joueur a un niveau égal au niveau de la quête + 7
        else if (Player.MyInstance.MyLevel == quest.MyLevel + 7)
        {
            // 60% 
            return (int)((quest.MyXp * 0.6 / 5) * 5);
        }
        // Si le joueur a un niveau égal au niveau de la quête + 8
        else if (Player.MyInstance.MyLevel == quest.MyLevel + 8)
        {
            // 40% 
            return (int)((quest.MyXp * 0.4 / 5) * 5);
        }
        // Si le joueur a un niveau égal au niveau de la quête + 9
        else if (Player.MyInstance.MyLevel == quest.MyLevel + 9)
        {
            // 20% 
            return (int)((quest.MyXp * 0.2 / 5) * 5);
        }
        // Si le joueur a un niveau égal ou supérieur au niveau de la quête + 10
        else if (Player.MyInstance.MyLevel >= quest.MyLevel + 10)
        {
            // 10% 
            return (int)((quest.MyXp * 0.1 / 5) * 5);
        }

        return 0;
    }

    /// <summary>
    /// Calcul du "ZeroDifference"
    /// </summary>
    /// <returns></returns>
    private static int CalculateZD()
    {
        // Niveau du joueur inférieur ou égal à 7
        if (Player.MyInstance.MyLevel <= 7)
        {
            return 5;
        }
        // Niveau du joueur entre 8 et 9
        else if (Player.MyInstance.MyLevel >= 8 && Player.MyInstance.MyLevel <= 9)
        {
            return 6;
        }
        // Niveau du joueur entre 10 et 11
        else if (Player.MyInstance.MyLevel >= 10 && Player.MyInstance.MyLevel <= 11)
        {
            return 7;
        }
        // Niveau du joueur entre 12 et 15
        else if (Player.MyInstance.MyLevel >= 12 && Player.MyInstance.MyLevel <= 15)
        {
            return 8;
        }
        // Niveau du joueur entre 16 et 19
        else if (Player.MyInstance.MyLevel >= 16 && Player.MyInstance.MyLevel <= 19)
        {
            return 9;
        }
        // Niveau du joueur entre 20 et 29
        else if (Player.MyInstance.MyLevel >= 20 && Player.MyInstance.MyLevel <= 29)
        {
            return 11;
        }
        // Niveau du joueur entre 30 et 39
        else if (Player.MyInstance.MyLevel >= 30 && Player.MyInstance.MyLevel <= 39)
        {
            return 12;
        }
        // Niveau du joueur entre 40 et 44
        else if (Player.MyInstance.MyLevel >= 40 && Player.MyInstance.MyLevel <= 44)
        {
            return 13;
        }
        // Niveau du joueur entre 45 et 49
        else if (Player.MyInstance.MyLevel >= 45 && Player.MyInstance.MyLevel <= 49)
        {
            return 14;
        }
        // Niveau du joueur entre 50 et 54
        else if (Player.MyInstance.MyLevel >= 50 && Player.MyInstance.MyLevel <= 54)
        {
            return 15;
        }
        // Niveau du joueur entre 55 et 59
        else if (Player.MyInstance.MyLevel >= 55 && Player.MyInstance.MyLevel <= 59)
        {
            return 16;
        }

        // Niveau du joueur supérieur ou égal à 60
        return 17;
    }

    /// <summary>
    /// Calcul le niveau qui ne rapportera plus d'expérience
    /// </summary>
    /// <returns></returns>
    public static int CalculateGrayLevel()
    {
        // Niveau du joueur inférieur ou égal à 5
        if (Player.MyInstance.MyLevel <= 5)
        {
            return 0;
        }
        // Niveau du joueur entre 6 et 49
        else if (Player.MyInstance.MyLevel >= 6 && Player.MyInstance.MyLevel <= 49)
        {
            return Player.MyInstance.MyLevel - (Player.MyInstance.MyLevel / 10) - 5;
        }
        // Niveau du joueur égal à 50
        else if (Player.MyInstance.MyLevel == 50)
        {
            return Player.MyInstance.MyLevel - 10;  // 40
        }
        // Niveau du joueur entre 51 et 59
        else if (Player.MyInstance.MyLevel >= 51 && Player.MyInstance.MyLevel <= 59)
        {
            return Player.MyInstance.MyLevel - (Player.MyInstance.MyLevel / 5) - 1;
        }

        // Niveau du joueur supérieur ou égal à 60
        return Player.MyInstance.MyLevel - 9;
    }
}
