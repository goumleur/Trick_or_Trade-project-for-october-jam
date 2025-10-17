using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unload : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Unload";
        description_Carte = "Give your opponent 3 cards from your hand.";
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
        if (discarded == false)
        {
            if (advairsaire == "main")
            {
                for (int i = 0; i < 3; i++)
                {
                    if (main.transform.childCount == 0) break;
                    int nbAleatoire = Random.Range(0, GameObject.Find("IAHand").transform.childCount);
                    GameObject carteDiscard = GameObject.Find("IAHand").transform.GetChild(nbAleatoire).gameObject;
                    mainAdvairsaire.transform.GetComponent<Mains>().PrendreCarte(carteDiscard);
                }
            }
            else if (advairsaire == "IAHand")
            {
                GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler = true;
                GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
                GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = gameObject;
            }
            discard();
        }
    }
}
