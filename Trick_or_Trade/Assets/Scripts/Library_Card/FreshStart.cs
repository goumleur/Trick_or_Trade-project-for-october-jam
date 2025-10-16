using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.EventSystems;

public class FreshStart : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Fresh start";
        description_Carte = "Discard your hand and draw 4 cards.";
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
        // Effect: "Discard your hand and draw 4 cards."
        if (discarded == false && gameObject.transform.parent.name != "IAHand")
        {
            // Reference to player's hand manager
            var player = GameObject.Find("Main Camera").GetComponent<main_joueur>();

            // Copy current hand because we'll modify the collection while iterating
            var hand = new System.Collections.Generic.List<GameObject>(player.cartesMain);

            // Discard each card in the player's hand using DiscardPile.discardCard
            foreach (var card in hand)
            {
                // Skip nulls just in case
                if (card == null) continue;
                // Use player's discard pile
                GameObject.Find("DiscardPile").GetComponent<DiscardPile>().discardCard(card);
            }

            // After discarding, draw 4 cards (use PigerUneCarte)
            for (int i = 0; i < 4; i++)
            {
                // If the deck is empty, break
                var deck = GameObject.Find("Deck");
                if (deck == null || deck.transform.childCount == 0) break;
                player.PigerUneCarte();
            }

            // Mark this card as discarded to avoid double effects
            discarded = true;
        }
    }
}
