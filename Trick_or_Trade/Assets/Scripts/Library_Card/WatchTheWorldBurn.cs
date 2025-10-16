using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.EventSystems;

public class WatchTheWorldBurn : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Watch the world burn";
        description_Carte = "Destroy all cards in both player's hands.";
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
        // Effect: "Destroy all cards in both player's hands." (when played by the player)
        if (discarded == false)
        {
            var player = GameObject.Find("main").GetComponent<main_joueur>();
            var ai = GameObject.Find("IAHand").GetComponent<MainAi>();

            if (player != null)
            {
                // Copy list to avoid modifying while iterating
                var pCards = new System.Collections.Generic.List<GameObject>(player.cartesMain);
                foreach (var c in pCards)
                {
                    if (c == null) continue;
                    DetruireCarte(c);
                    sourisSortiCarte();
                }
            }

            if (ai != null)
            {
                var aiCards = new System.Collections.Generic.List<GameObject>(ai.cartesMain);
                foreach (var c in aiCards)
                {
                    if (c == null) continue;
                    DetruireCarte(c);
                }
            }

            // discard this card
            discard();
        }
    }
}
