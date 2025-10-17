using UnityEngine;
using UnityEngine.EventSystems;

public class PackOfDeath : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Pack of Death";
        description_Carte = "Discard a candy card. Choose any card in your deck to add to your hand.";
        afficher_carte();
        typeCard = "Action";
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
        if (discarded == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false && CandyDisponible() == true)
        {
            discard();
            deck.GetComponent<Decks>().GetDeckCard();
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
        }
    }
    public bool CandyDisponible()
    {
        for (int i = 0; i < main.transform.childCount; i++)
        {
            if (main.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Candy")
            {
                discarded = true;
                discardPile.GetComponent<DiscardsPiles>().discardCard(main.transform.GetChild(i).gameObject);
                return true;
            }
        }
        return false;
    }
}
