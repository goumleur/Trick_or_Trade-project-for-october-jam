using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieHand : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "ZombieHand";
        description_Carte = "When this card enters an opponents hand, that player discards 2 cards.";
        afficher_carte();
        typeCard = "Trick";
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
            VolerCarteAdvairsaire(gameObject);
            if (advairsaire == "main")
            {
                for (int i = 0; i < 2; i++)
                {
                    int nbAleatoire = Random.Range(0, GameObject.Find("IAHand").transform.childCount);
                    GameObject carteDiscard = GameObject.Find("IAHand").transform.GetChild(nbAleatoire).gameObject;
                    GameObject.Find("DiscardPileIA").transform.GetComponent<IADiscardPile>().discardCard(carteDiscard);
                }
            }
            else if(advairsaire == "IAHand")
            {
                GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
                GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
                GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = gameObject;
            }
        }
    }
}
