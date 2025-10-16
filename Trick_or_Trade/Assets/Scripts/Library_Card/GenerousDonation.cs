using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.EventSystems;

public class GenerousDonation : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Generous donation";
        description_Carte = "Give your'e opponent your entire hand.";
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
        // Effect: "Give your opponent your entire hand." (excluding this card)
        if (discarded == false)
        {
            var player = main.GetComponent<Mains>();
            var ai = mainAdvairsaire.GetComponent<Mains>();

            if (player != null && ai != null)
            {
                // Copy player's hand because we'll modify during transfer
                var hand = new System.Collections.Generic.List<GameObject>(player.cartesMain);

                foreach (var card in hand)
                {
                    if (card == null) continue;
                    // Don't give the Generous Donation card itself (it should resolve and be discarded)
                    if (card == this.gameObject) continue;

                    // Use MainAi.PrendreCarte to transfer ownership (it removes from player's list internally)
                    ai.PrendreCarte(card);
                }

                // Reorganize both hands
                player.OrganiserLaMain();
                ai.OrganiserLaMain();
            }

            // Discard this card after giving away the hand
            discard();
        }
    }
}

