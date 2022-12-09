﻿using UnityEngine;



/// <summary>
/// Classe de gestion des barres d'or
/// </summary>
[CreateAssetMenu(fileName = "GoldBar", menuName = "Items/GoldBar", order = 1)]
public class GoldBar : Item
{
    /// <summary>
    /// Retourne la description de l'item : Surcharge la fonction GetDescription du script Item
    /// </summary>
    public override string GetDescription()
    {
        // Appelle GetDescription sur la classe mère et ajoute la description de l'item
        return base.GetDescription() + string.Format("\n\n<color=#ECECEC>Une barre en or !!</color>");
    }
}
