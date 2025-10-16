using UnityEngine;
using UnityEngine.EventSystems;

public class FaithlessAnnihilation : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Faithless Annihilation";
        description_Carte = "Destroy the top 10 cards of both player's decks.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
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
            // Destroy top 10 cards from player's Deck
            if (deck != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (deck.transform.childCount == 0) break;
                    var top = deck.transform.GetChild(0).gameObject;
                    if (top != null) DetruireCarte(top);
                }
            }

            // Destroy top 10 cards from AI deck
            if (deckAdvairsaire != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (deckAdvairsaire.transform.childCount == 0) break;
                    var top = deckAdvairsaire.transform.GetChild(0).gameObject;
                    if (top != null) DetruireCarte(top);
                }
            }

            // discard this card after effect
            discard();
        }
    }
}