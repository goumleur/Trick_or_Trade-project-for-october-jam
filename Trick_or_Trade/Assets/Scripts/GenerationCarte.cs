using UnityEngine;
using TMPro;

public class GenerationCarte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected string nom_Carte = "Carte Inconnue";
    protected string description_Carte = "Aucune description";

    public virtual void CreerLaCarte() { }
    protected void afficher_carte()
    {
        GameObject[] cartes = GameObject.FindGameObjectsWithTag("Carte");

        foreach (GameObject carte in cartes)
        {
            TextMeshPro tmp = carte.GetComponent<TextMeshPro>();
            tmp.text = "name : " + nom_Carte + "\n" + "\n" + "description : " + description_Carte;

        }
    }
}