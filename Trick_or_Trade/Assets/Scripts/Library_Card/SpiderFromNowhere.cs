using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpiderFromNowhere : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "SpidersFromNowhere";
        description_Carte = "When this card is discarded, replace 2 candies in your opponents deck with Spiders from Nowhere.";
        afficher_carte();
        if (gameObject.tag == "Untagged")
        {
            Invoke("TrouverAdvairsaire", 0.001f);
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
        if (discarded == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            discard();
            if (advairsaire == "main")
            {
                List<Transform> enfants = new List<Transform>();
                for (int y = 0; y < GameObject.Find("Deck").transform.childCount; y++)
                {
                    enfants.Add(GameObject.Find("Deck").transform.GetChild(y));
                }
                int nbATransformer = 2;
                for (int y = 0; y < GameObject.Find("Deck").transform.childCount; y++)
                {
                    if( nbATransformer == 0)
                    {
                        return;
                    }
                    if(enfants[y].GetComponent<GenerationCarte>().nom_Carte == "Candy")
                    {
                        GameObject.Find("main").GetComponent<main_joueur>().modifierCarte(enfants[y], GameObject.Find("SpidersFromNowhere"));
                        nbATransformer -= 1;
                    }
                }
                }
            else if(advairsaire == "IAHand")
            {
                List<Transform> enfants = new List<Transform>();
                for (int y = 0; y < GameObject.Find("DeckIA").transform.childCount; y++)
                {
                    enfants.Add(GameObject.Find("DeckIA").transform.GetChild(y));
                }
                int nbATransformer = 2;
                for (int y = 0; y < GameObject.Find("DeckIA").transform.childCount; y++)
                {
                    if( nbATransformer == 0)
                    {
                        return;
                    }
                    if(enfants[y].GetComponent<GenerationCarte>().nom_Carte == "Candy")
                    {
                        GameObject.Find("IAHand").GetComponent<MainAi>().modifierCarte(enfants[y], GameObject.Find("SpidersFromNowhere"));
                        nbATransformer -= 1;
                    }
                }
            }
        }
    }
}
