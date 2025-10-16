using UnityEngine;
using UnityEngine.EventSystems;
public class ScrabbledNote : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Scrabbled Note";
        description_Carte = "Draw card until you have 4 cards in hand.";
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
        // Effect: "Draw cards until you have 4 cards in hand." Works for player and AI depending on who played it.
        if (discarded == false)
        {
            // If card is in player's hand
            if (gameObject.transform.parent.name != "IAHand")
            {
                var player = GameObject.Find("Main Camera").GetComponent<main_joueur>();
                if (player != null)
                {
                    while (player.cartesMain.Count < 4)
                    {
                        // If deck empty, break
                        var deck = GameObject.Find("Deck");
                        if (deck == null || deck.transform.childCount == 0) break;
                        player.PigerUneCarte();
                    }
                }
            }
            else // card is in IA hand
            {
                var ai = GameObject.Find("IAHand").GetComponent<MainAi>();
                if (ai != null)
                {
                    while (ai.cartesMain.Count < 4)
                    {
                        var deckIA = GameObject.Find("DeckIA");
                        if (deckIA == null || deckIA.transform.childCount == 0) break;
                        ai.PigerUneCarte();
                    }
                }
            }

            // discard this card after effect
            discard();
        }
    }
}


