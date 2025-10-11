using UnityEngine;

public class GenerationCarte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected string nom_Carte = "Carte Inconnue";
    protected string description_Carte = "Aucune description";

    public virtual void CreerLaCarte() {}
    protected void afficher_carte()
    {
        GetComponent<TextMeshPro>().text = nom_Carte + "\n" + description_Carte;
    }
    
    
}
