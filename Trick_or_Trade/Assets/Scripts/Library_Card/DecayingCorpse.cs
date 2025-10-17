using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecayingCorpse : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Decaying Corpse";
        description_Carte = "When this card enters an opponents hand, transform the 2 first tricks in that players hand into maggots.";
        afficher_carte();
        typeCard = "Trick";
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
            List<Transform> enfants = new List<Transform>();
            for (int y = 0; y < mainAdvairsaire.transform.childCount; y++)
            {
                enfants.Add(mainAdvairsaire.transform.GetChild(y));
            }
            int nbATransformer = 2;
            for (int y = 0; y < mainAdvairsaire.transform.childCount; y++)
            {
                if( nbATransformer == 0)
                {
                    return;
                }
                if(enfants[y].GetComponent<GenerationCarte>().typeCard == "Trick")
                {
                    mainAdvairsaire.GetComponent<Mains>().modifierCarteMain(enfants[y], GameObject.Find("Maggots"));
                    nbATransformer -= 1;
                }
            }
        }
    }
}
