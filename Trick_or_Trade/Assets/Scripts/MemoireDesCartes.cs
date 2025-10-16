using UnityEngine;

public class MemoireDesCartes : MonoBehaviour
{
    public bool vaVoler = false; 
    public bool vaDetruire = false;
    public GameObject objetUtiliser; // Carte utilliser dans le tour
    public string nomCarteUtiliser; // Nom de la carte (avec la variable nom_Carte, pas dans la hiérarchie)
    public string choixEntreDiscardOuDestroy; // Quand une carte a ce choix, il sert a dire a Generation Carte si il doit etre détruit ou discard
    public int multipleDiscard = 0; // Si il doit y avoir plusieur carte a discard, sert a indiquer a quelle discard tu est rendu (Commence par la variable 0)
    public GameObject objetVoler; // Carte qui a été voler
    public bool protect = false;
    public bool boutonUse = false;

    public void EstProtected()
    {
        protect = true;
        boutonUse = true;
    }
    public void EstPasProtected()
    {
        protect = false;
        boutonUse = true;
    }
}
