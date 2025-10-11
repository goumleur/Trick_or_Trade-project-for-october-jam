using TMPro;
using UnityEngine;

public class carte_de_jeu : MonoBehaviour
{
  
    public string nom_Carte = "Carte Inconnue";
    public string valeur_Carte = "non-existant";
    public string description_Carte = "Aucune description";
    
  public void Start()
{
    GameObject[] cartes = GameObject.FindGameObjectsWithTag("Carte");

    foreach (GameObject carte in cartes)
    {
        TextMeshPro tmp = carte.GetComponent<TextMeshPro>();
        tmp.text = "name : "  + nom_Carte + " value : " + valeur_Carte + " description : " + description_Carte;
    }
}

}
