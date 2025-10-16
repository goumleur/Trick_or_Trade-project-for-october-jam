using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor.VersionControl;

public class Blade_Of_Nightmare : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Blade of nightmares";
        description_Carte = "Your opponent discards all trick cards from their hand.";
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
        // Effect: "Your opponent discards all trick cards from their hand." (cards tagged "trick")
        if (discarded == false && gameObject.transform.parent.name != "IAHand")
        {
            var iaHand = GameObject.Find("IAHand");
            if (iaHand != null)
            {
                // Collect trick cards to avoid modifying while iterating
                var toDiscard = new System.Collections.Generic.List<GameObject>();
                foreach (Transform child in iaHand.transform)
                {
                    if (child == null || child.gameObject == null) continue;
                    if (child.gameObject.tag == "trick") toDiscard.Add(child.gameObject);
                }

                if (toDiscard.Count > 0)
                {
                    var iadiscard = GameObject.Find("DiscardPileIA");
                    if (iadiscard != null)
                    {
                        var iadp = iadiscard.GetComponent<IADiscardPile>();
                        foreach (var card in toDiscard)
                        {
                            if (iadp != null) iadp.discardCard(card);
                            else GameObject.Find("DiscardPileIA").GetComponent<IADiscardPile>().discardCard(card);
                        }
                    }
                    else
                    {
                        // Fallback: try to move cards to generic DiscardPile
                        var dp = GameObject.Find("DiscardPile");
                        if (dp != null)
                        {
                            var dpc = dp.GetComponent<DiscardPile>();
                            foreach (var card in toDiscard)
                            {
                                if (dpc != null) dpc.discardCard(card);
                            }
                        }
                    }
                }
            }

            // mark and discard this card
            discard();
        }
    }
}

