using UnityEngine;
using UnityEngine.EventSystems;

public class LuckyFind : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "LuckyFind";
        description_Carte = "When this card is put into your discard pile, you may return a card from your discard pile to the top of your deck.";
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
        if (discarded == false && gameObject.transform.parent.name != "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false && GameObject.Find("DiscardPile").transform.childCount > 0)
        {
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
            GameObject.Find("DiscardPile").GetComponent<DiscardPile>().CarteASauver(GameObject.Find("DiscardPile").transform.GetChild(GameObject.Find("DiscardPile").transform.childCount - 1).gameObject);
            discard();
        }
    }
}
