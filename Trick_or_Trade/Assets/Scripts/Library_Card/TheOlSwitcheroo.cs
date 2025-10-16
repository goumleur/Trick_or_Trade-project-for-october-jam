using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.EventSystems;

public class TheOlSwitcheroo :GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "The ol' switcheroo";
        description_Carte = "Exchange hands with your opponent.";
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
        // Effect: "Exchange hands with your opponent." - swap all cards between main and IAHand
        if (discarded == false && gameObject.transform.parent.name != "IAHand")
        {
            var player = GameObject.Find("Main Camera").GetComponent<main_joueur>();
            var ai = GameObject.Find("IAHand").GetComponent<MainAi>();

            if (player != null && ai != null)
            {
                // Copy the lists
                var playerCards = new System.Collections.Generic.List<GameObject>(player.cartesMain);
                var aiCards = new System.Collections.Generic.List<GameObject>(ai.cartesMain);

                // Clear both lists
                player.cartesMain.Clear();
                ai.cartesMain.Clear();

                // Reparent AI cards to player hand
                foreach (var c in aiCards)
                {
                    if (c == null) continue;
                    c.transform.SetParent(GameObject.Find("main").transform, worldPositionStays: true);
                    player.cartesMain.Add(c);
                }

                // Reparent player cards to AI hand
                foreach (var c in playerCards)
                {
                    if (c == null) continue;
                    c.transform.SetParent(GameObject.Find("IAHand").transform, worldPositionStays: true);
                    ai.cartesMain.Add(c);
                }

                // Reorganize both hands
                player.OrganiserLaMain();
                ai.OrganiserLaMain();
            }

            // discard this card after effect
            discard();
        }
    }
}
