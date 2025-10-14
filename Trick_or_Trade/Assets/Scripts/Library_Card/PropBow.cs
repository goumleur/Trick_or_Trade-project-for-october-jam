using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;



public class PropBow : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool vaVoler = false;
    public bool vaDetruire = false;
    bool discarded = false;

    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        Debug.Log("I'm Working");
        nom_Carte = "Prop: Bow";
        description_Carte = "Look at your opponents hand and choose a card for them to discard.";
        afficher_carte();
    }public void discard()
    {
        discarded = true;
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().discardCard(gameObject);
    }

    bool selectionActive = false;
    // Called when we want to start the effect: player should choose an opponent card
    public void StartSelectOpponentCard()
    {
        // Determine opponent zone
        string opponentZone = (gameObject.transform.parent.name == "main" || gameObject.transform.parent.name == "Deck") ? "IAHand" : "main";
        GameObject zone = GameObject.Find(opponentZone);
        if (zone == null) return;

        // Enable selection component on each opponent card
        // Use reflection to add the TargetForDiscard component by name to avoid compile-time coupling
        System.Type compType = null;
        foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            compType = asm.GetType("TargetForDiscard");
            if (compType != null) break;
        }
        foreach (Transform child in zone.transform)
        {
            var go = child.gameObject;
            if (compType != null)
            {
                var existing = go.GetComponent(compType);
                if (existing == null)
                {
                    var comp = go.AddComponent(compType);
                    // set origin field via reflection
                    var f = compType.GetField("origin");
                    if (f != null) f.SetValue(comp, this);
                }
                else
                {
                    var f = compType.GetField("origin");
                    if (f != null) f.SetValue(existing, this);
                }
            }
        }
        selectionActive = true;
        // Optionally show UI prompt to user here
    }

    // Called by TargetForDiscard when the player clicks an opponent card
    public void OnTargetSelected(GameObject targetCard)
    {
        if (!selectionActive) return;
        selectionActive = false;

        // Remove the selection components from opponent cards
        string opponentZone = (gameObject.transform.parent.name == "main" || gameObject.transform.parent.name == "Deck") ? "IAHand" : "main";
        GameObject zone = GameObject.Find(opponentZone);
        if (zone != null)
        {
            System.Type compType = null;
            foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                compType = asm.GetType("TargetForDiscard");
                if (compType != null) break;
            }
            foreach (Transform child in zone.transform)
            {
                if (compType != null)
                {
                    var existing = child.gameObject.GetComponent(compType);
                    if (existing != null)
                    {
                        Destroy(existing as Component);
                    }
                }
            }
        }

        // Force the target to be discarded
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().discardCard(targetCard);

        // Update hand lists and organization
        if (opponentZone == "main")
        {
            GameObject.Find("Main Camera").GetComponent<main_joueur>().cartesMain.Remove(targetCard);
            GameObject.Find("Main Camera").GetComponent<main_joueur>().OrganiserLaMain();
        }
        else
        {
            GameObject.Find("IAHand").GetComponent<MainAi>().cartesMain.Remove(targetCard);
            GameObject.Find("IAHand").GetComponent<MainAi>().OrganiserLaMain();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.transform.parent.name == "main" || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaDetruire == true || gameObject.transform.parent.name == "Deck")
        {
            Debug.Log("Souris sur la carte !");
            AfficherCarteZoom();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.transform.parent.name == "main" || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaDetruire == true ||  gameObject.transform.parent.name == "Deck")
        {
            Debug.Log("Souris quitte la carte !");
            EnleverCarteZoom();
        }
    }

    public void SetVoler()
    {
        Debug.Log("Le joueur s'apprête a voler une carte");
        GameObject.Find("CarteModel").GetComponent<PropBow>().vaVoler = true;
    }
    public void VaDetruire()
    {
        Debug.Log("Le joueur s'apprête a détruire une carte");
        GameObject.Find("CarteModel").GetComponent<PropBow>().vaDetruire = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (discarded == true && GameObject.Find("DiscardPile").GetComponent<DiscardPile>().carteASauver == null)
        {
            Debug.Log("Working");
            GameObject.Find("DiscardPile").GetComponent<DiscardPile>().CarteASauver(gameObject);
        }
        else if (gameObject.transform.parent.name == "Deck")
        {
            GameObject.Find("Deck").GetComponent<deck_joueur>().ChercherCarte(gameObject);
        }
        else if (GameObject.Find("CarteModel").GetComponent<PropBow>().vaVoler == true)
        {
            VolerCarteAdvairsaire(gameObject);
            GameObject.Find("CarteModel").GetComponent<PropBow>().vaVoler = false;
        }
        else if (GameObject.Find("CarteModel").GetComponent<PropBow>().vaDetruire == true)
        {
            DetruireCarteAdvairsaire(gameObject);
            GameObject.Find("CarteModel").GetComponent<PropBow>().vaDetruire = false;
        }
        else if (discarded == false)
        {
            // If the PropBow is in the player's hand, activate its effect: select an opponent card to discard
            if (gameObject.transform.parent != null && gameObject.transform.parent.name == "main")
            {
                if (!selectionActive)
                {
                    Debug.Log("Activation de la sélection de carte adverse");
                    StartSelectOpponentCard();
                }
            }
            else
            {
                // fallback: just discard this card
                discard();
            }
        }
    }

    // ensure the origin card is discarded after resolving the effect
    public void OnTargetSelected_PostResolve(GameObject targetCard)
    {
        // This is same as OnTargetSelected but also discards the origin card
        OnTargetSelected(targetCard);
        // discard the PropBow itself
        discard();
    }
}
