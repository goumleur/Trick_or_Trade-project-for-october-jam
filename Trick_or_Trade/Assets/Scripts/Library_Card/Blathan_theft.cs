using UnityEngine;
using UnityEngine.EventSystems;
public class Blathan_theft :GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Blathan theft";
        description_Carte = "Draw a card from your opponent's deck.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
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
        // Effect: "Draw a card from your opponent's deck." (take top card from DeckIA)
        if (discarded == false && gameObject.transform.parent.name != "IAHand")
        {
            var deckIA = GameObject.Find("DeckIA");
            if (deckIA != null && deckIA.transform.childCount > 0)
            {
                var topCard = deckIA.transform.GetChild(0).gameObject;
                // Give the card to the player
                var player = GameObject.Find("Main Camera").GetComponent<main_joueur>();
                if (player != null && topCard != null)
                {
                    player.PrendreCarte(topCard);
                }
            }

            // discard this card after resolving
            discard();
        }
    }
}


