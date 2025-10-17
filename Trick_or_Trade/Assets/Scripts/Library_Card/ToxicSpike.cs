using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToxicSpike : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Toxic Spike";
        description_Carte = "Transform a random card in your opponents hand into a maggot.";
        afficher_carte();
        typeCard = "Action";
        if(gameObject.tag == "Untagged")
        {
            Invoke("TrouverAdvairsaire",0.001f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sourisSurCarte();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sourisSortiCarte();
    }

    public void SetVoler()
    {
        Debug.Log("Le joueur s'apprente a volé une carte");
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler = true;
    }
    public void VaDetruire()
    {
        Debug.Log("Le joueur s'apprente a volé une carte");
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        click();
    }
    public override void EffetCarte()
    {
        if (discarded == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false && mainAdvairsaire.transform.childCount > 0)
        {
            discard();
            int nbAleatoire = Random.Range(0, mainAdvairsaire.transform.childCount);
            mainAdvairsaire.GetComponent<Mains>().modifierCarteMain(mainAdvairsaire.transform.GetChild(nbAleatoire), GameObject.Find("Maggots"), mainAdvairsaire);
            Debug.Log(mainAdvairsaire.transform.GetChild(nbAleatoire));
        }
    }
}
