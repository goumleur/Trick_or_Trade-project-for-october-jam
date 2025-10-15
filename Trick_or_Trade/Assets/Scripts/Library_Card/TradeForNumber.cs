using UnityEngine;
using UnityEngine.EventSystems;

public class TradeForNumber : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool vaVoler = false;
    public bool vaDetruire = false;
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "TradeForNumber";
        description_Carte = "Discard a card then draw 3 cards.";
        afficher_carte();
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
        if(discarded == false && gameObject.transform.parent.name != "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            discard();
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = gameObject;
        }
    }
}
