using TMPro;
using UnityEngine;

public class carte_de_jeu : MonoBehaviour
{
    public string nom_Carte = "Carte Inconnue";
    public int valeur_Carte = 0;
    public string description_Carte = "Aucune description";
    
    public void Start()
    {
        afficher_carte();
    }
    public void afficher_carte()
    {
       
        GetComponent<TextMeshPro>().text =  nom_Carte + "\n" + valeur_Carte + "\n" + description_Carte;
    }

}
