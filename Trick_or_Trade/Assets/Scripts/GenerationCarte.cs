using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GenerationCarte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] protected string nom_Carte = "Carte Inconnue";
   [SerializeField] protected string description_Carte = "Aucune description";
   [SerializeField] protected Sprite image_Carte = null;

    public virtual void CreerLaCarte() { }
   [SerializeField] protected void afficher_carte()
    {
        Image img = gameObject.transform.GetChild(1).GetComponent<Image>();
        img.sprite = image_Carte;
        Debug.Log("Ce script est attaché à : " + gameObject.name);
        TextMeshPro tmp = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
        tmp.text = "name : " + nom_Carte + "\n" + "\n";
        TextMeshPro t1 = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
        t1.text =  "description : " + description_Carte;

    }
}