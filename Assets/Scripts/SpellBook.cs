using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe pour la bibiothèque des sorts
/// </summary>
public class SpellBook : MonoBehaviour
{
    // Tableau des sorts
    [SerializeField]
    private Spell[] spells;

    // Retourne un sort avec ses propriétés
    public Spell CastSpell(int spellIndex)
    {
        return spells[spellIndex];
    }
}
