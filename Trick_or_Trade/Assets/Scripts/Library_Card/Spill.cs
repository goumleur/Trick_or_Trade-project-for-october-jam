using UnityEngine;
using UnityEngine.EventSystems;
public class Spill : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Spill";
        description_Carte = "The opponents discard the top 4 cards of their deck.";
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
            for(int i = 0; i < 4; i++)
            {
                GameObject carteADiscard = GameObject.Find("DeckIA").transform.GetChild(0).gameObject;
                GameObject.Find("DiscardPileIA").GetComponent<IADiscardPile>().discardCard(carteADiscard);
            }
        }
    }
}
