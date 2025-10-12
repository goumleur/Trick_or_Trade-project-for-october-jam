using TMPro;
using UnityEngine;

public class CarteDeJeu : GenerationCarte
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        Debug.Log("I'm Working");
        image_Carte = Resources.Load<Sprite>("Images/ImageRecycle");
        nom_Carte = "Recycle";
        description_Carte = "Put a card from any discard pile into your hand.";
        afficher_carte();
    }
}
