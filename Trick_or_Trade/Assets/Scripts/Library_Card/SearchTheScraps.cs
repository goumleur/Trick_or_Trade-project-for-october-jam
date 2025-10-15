using UnityEngine;
using UnityEngine.EventSystems;

public class SearchTheScraps : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Search the Scraps";
        description_Carte = "Put all candy card from your discard pile to your hand.";
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
            Debug.Log("Marche");
            discard();
            foreach(Transform card in GameObject.Find("DiscardPile").transform)
            {
                if(card.GetComponent<GenerationCarte>().nom_Carte == "Candy")
                {
                    GameObject.Find("DiscardPile").GetComponent<DiscardPile>().CarteASauver(card.gameObject);
                }
            }
        }
    }
}
