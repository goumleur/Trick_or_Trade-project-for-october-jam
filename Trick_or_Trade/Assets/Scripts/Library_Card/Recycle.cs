using TMPro;
using UnityEngine;

public class CarteDeJeu : GenerationCarte
{
    override public void CreerLaCarte()
    {
        nom_Carte = "Recycle";
        description_Carte = "Put a card from any discard pile into your hand.";
    }
}
