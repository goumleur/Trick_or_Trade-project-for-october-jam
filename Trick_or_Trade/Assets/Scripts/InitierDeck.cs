using UnityEngine;

public class InitierDeck : MonoBehaviour
{
    public string Deck1(int i)
    {
        string carteAInitier;
        if (i == 0) // i c le num de la carte a initier. Tu peux mettre des || si il y a plusieur meme carte dans le deck
        {
            carteAInitier = "";//NOM de la carte (Dans la hiérarchie)
        }
        else
        {
            return "Candy";
        }
        return carteAInitier;

        // Cartes qui ne serons pas disponnible car faute de temps
        // Hundred Bites
        // Insectile Might
        // Fire bolts
        // Double Trouble
        // Incendiary

        // Cartes non fait car il faut le systeme de tours
        // Job Application
        // Bat's Eye
    }
}
