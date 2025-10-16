using UnityEngine;
using UnityEngine.EventSystems;

public class Fantastical_Reclaimation : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Fantastical reclaimation";
        description_Carte = "Shuffle your discard pile into your deck.";
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
        // Effect: "Shuffle your discard pile into your deck."
        if (discarded == false)
        {
            if (discardPile != null && deck != null && discardPile.transform.childCount > 0)
            {
                // Move all cards from discard to deck
                // Collect first because moving while iterating can cause issues
                var toMove = new System.Collections.Generic.List<Transform>();
                foreach (Transform child in discardPile.transform)
                {
                    toMove.Add(child);
                }
                foreach (var t in toMove)
                {
                    t.SetParent(deck.transform, worldPositionStays: true);
                }

                // Shuffle the deck using existing deck_joueur method
                var deckComp = deck.GetComponent<Decks>();
                if (deckComp != null) deckComp.melanger_deck();
            }

            // mark as used/discarded
            discard();
        }
    }
}
