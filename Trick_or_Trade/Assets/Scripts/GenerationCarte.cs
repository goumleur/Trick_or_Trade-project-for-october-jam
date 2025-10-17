using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using Unity.VisualScripting;


public class GenerationCarte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected GameObject main;
    protected GameObject discardPile;
    protected GameObject deck;
    protected GameObject mainAdvairsaire;
    protected GameObject discardPileAdvairsaire;
    protected GameObject deckAdvairsaire;
    



    [SerializeField] public string nom_Carte = "Carte Inconnue";
    [SerializeField] protected string description_Carte = "Aucune description";

    [SerializeField] protected Sprite backRound;
    [SerializeField] protected Sprite illustration;
    public bool discarded = false;
    public string typeCard;

    public void discard()
    {
        discarded = true;
        discardPile.GetComponent<DiscardsPiles>().discardCard(gameObject);
        // Notify turn manager that this card counted as a play (SendMessage avoids direct type reference)
        var tmObj = GameObject.Find("TurnManager");
        if (tmObj != null)
            tmObj.SendMessage("OnCardPlayed", gameObject.transform.parent != null ? gameObject.transform.parent.name : "main", SendMessageOptions.DontRequireReceiver);
    }

    public string advairsaire;

    GameObject clone;
    int oldOrderInLayer;

    public virtual void CreerLaCarte() { }
    public virtual void EffetCarte() { }
    public void afficher_carte()
    {
        TextMeshPro tmp = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
        tmp.text = "name : " + nom_Carte + "\n" + "\n";
        TextMeshPro t1 = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
        t1.text = "description : " + description_Carte;

    }
    protected void VolerCarteAdvairsaire(GameObject carteViser) // Permet de changer la carte du joueur
    {
        if (carteViser.GetComponent<GenerationCarte>().nom_Carte != "Trick Coin")
        {
            EnleverCarteZoom();
            mainAdvairsaire.GetComponent<Mains>().PrendreCarte(carteViser);
            TrouverAdvairsaire();
        }
    }
    protected void DetruireCarte(GameObject carteViser) // Permet de détruire UNIQUEMENT la carte
    {
        main.GetComponent<Mains>().DetruireCarte(carteViser);
        FlaretoLife();
    }
    void FlaretoLife()
    {
        for (int i = 0; i < mainAdvairsaire.transform.childCount; i++)
        {
            if (mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Flare to Life")
            {
                if (mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().main.name == "main")
                {
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Destroy";
                    GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Don't Destroy";
                    StartCoroutine(Coroutine2(mainAdvairsaire.transform.GetChild(i).gameObject));
                    return;
                }
                else
                {
                    if (main.transform.GetChild(i).GetComponent<GenerationCarte>().advairsaire == main.name)
                    {
                        DetruireCarte(main.transform.GetChild(i).gameObject);
                        for(int y = 0; i < 2; y++)
                        {
                            if (deck.transform.childCount == 0) break;
                            main.GetComponent<Mains>().PigerUneCarte();
                        }
                    }
                    else
                    {
                        DetruireCarte(main.transform.GetChild(i).gameObject);
                        for(int y = 0; i < 2; y++)
                        {
                            if (deck.transform.childCount == 0) break;
                            main.GetComponent<Mains>().PigerUneCarte();
                        }
                    }
                    return;
                }
            }
        }
        for (int i = 0; i < main.transform.childCount; i++)
        {
            if (main.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Flare to Life")
            {
                if (main.transform.GetChild(i).GetComponent<GenerationCarte>().main.name == "main")
                {
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Destroy";
                    GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Don't Destroy";
                    StartCoroutine(Coroutine2(main.transform.GetChild(i).gameObject));
                    return;
                }
                else
                {
                    if (main.transform.GetChild(i).GetComponent<GenerationCarte>().advairsaire == main.name)
                    {
                        DetruireCarte(main.transform.GetChild(i).gameObject);
                        for(int y = 0; i < 2; y++)
                        {
                            if (deck.transform.childCount == 0) break;
                            main.GetComponent<Mains>().PigerUneCarte();
                        }
                    }
                    else
                    {
                        DetruireCarte(main.transform.GetChild(i).gameObject);
                        for(int y = 0; i < 2; y++)
                        {
                            if (deck.transform.childCount == 0) break;
                            main.GetComponent<Mains>().PigerUneCarte();
                        }
                    }
                    return;
                }
            }
        }
    }
    IEnumerator Coroutine2(GameObject card)
    {
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction = true;
        yield return new WaitUntil(() => GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse == true);
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction = false;
        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect == true)
        {
            DetruireCarte(card);
            if (advairsaire == card.GetComponent<GenerationCarte>().advairsaire)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (deck.transform.childCount == 0) break;
                    main.GetComponent<Mains>().PigerUneCarte();
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (deckAdvairsaire.transform.childCount == 0) break;
                    mainAdvairsaire.GetComponent<Mains>().PigerUneCarte();
                }
            }
        }
        clearMemoire();
        GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
        GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse = false;
    }
    IEnumerator Coroutine3(GameObject card)
    {
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction = true;
        yield return new WaitUntil(() => GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse == true);
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction = false;
        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect == true)
        {
            GameObject.Find("DiscardPile").GetComponent<DiscardsPiles>().discardCard(card);
            for(int y = 0; y < discardPile.transform.childCount; y++)
            {

                GameObject card2 = discardPile.transform.GetChild(y).gameObject;
                GameObject.Find("IAHand").GetComponent<Mains>().modifierCarteMain(card2.transform, GameObject.Find("Maggots"));
                GameObject.Find("DiscardPileIA").GetComponent<DiscardsPiles>().discardCard(card2.transform.gameObject);
            }
        }
        clearMemoire();
        GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
        GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse = false;
    }
    void MaggotGive()
    {
        Debug.Log("Maggot");
        for (int i = 0; i < mainAdvairsaire.transform.childCount; i++)
        {
            if (mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Maggot Hive")
            {
                if (mainAdvairsaire.name == "main")
                {
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Infest";
                    GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Keep";
                    StartCoroutine(Coroutine3(mainAdvairsaire.transform.GetChild(i).gameObject));
                }
                else
                {
                    Debug.Log("Receved");
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect = true;
                    GameObject.Find("DiscardPileIA").GetComponent<DiscardsPiles>().discardCard(GameObject.Find("IAHand").GetComponent<Mains>().transform.GetChild(i).gameObject);
                    for(int y = 0; y < discardPile.transform.childCount; y++)
                    {
                        Debug.Log(GameObject.Find("DiscardPile").transform.GetChild(y).name);
                        GameObject card = GameObject.Find("DiscardPile").transform.GetChild(y).gameObject;
                        GameObject.Find("main").GetComponent<Mains>().modifierCarteMain(card.transform, GameObject.Find("Maggots"));
                        GameObject.Find("DiscardPile").GetComponent<DiscardsPiles>().discardCard(card.transform.gameObject);
                    }
                    return;
                }
            }
        }
    }
    protected void AfficherCarteZoom() // Zoom la carte sur de la main
    {
        Transform parent = GameObject.Find("Zoom").transform;
        clone = Instantiate(gameObject, parent, true);
        Destroy(clone.GetComponent<CarteDeJeu>());
        clone.transform.localPosition = new Vector2(0f, 0f);
        clone.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        clone.transform.localScale = new Vector2(1.4f, 1.4f);

        oldOrderInLayer = gameObject.GetComponent<Renderer>().sortingOrder;

        gameObject.GetComponent<Renderer>().sortingOrder = 110;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 110;
        gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 110;
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 110 + 1;
        gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 110;
        clone.transform.localScale = new Vector2(1.4f, 1.4f);
        gameObject.transform.localScale = new Vector2(1.1f, 1.1f);

    }
    protected void EnleverCarteZoom() // Dezoom la carte de la main
    {
        gameObject.GetComponent<Renderer>().sortingOrder = oldOrderInLayer;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = oldOrderInLayer;
        gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = oldOrderInLayer;
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = oldOrderInLayer + 1;
        gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = oldOrderInLayer + 1;
        gameObject.transform.localScale = new Vector2(1f, 1f);
        Destroy(clone);
    }
    protected void TrouverAdvairsaire() // Set l'advairsaire a la création de la carte
    {
        if (gameObject.transform.parent.name == "main" || gameObject.transform.parent.name == "Deck")
        {
            advairsaire = "IAHand";
        }
        else if (gameObject.transform.parent.name == "IAHand" || gameObject.transform.parent.name == "DeckIA")
        {
            advairsaire = "main";
        }
        JoueurOuIA();
    }
    protected void clearMemoire() // Reset la memoire de MemoireDesCartes
    {
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = null;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = null;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = false;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler = false;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler = null;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 0;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse = false;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect = false;
    }

    protected void click() // Condition auquelle toute les cartes peuvent etre affecter (discard, detruire, sauver, voler, trade(trade c 2 Vavoler))
    {
        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction == false)
        {
            IsProtected();
        }
        
    }

    protected void sourisSurCarte() // Permet de scale la carte
    {
        if (gameObject.transform.parent.name == "main" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            AfficherCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler == true && ((GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler == null && advairsaire == "main") || (advairsaire == "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler != null)) && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "PressureTrade")
        {
            AfficherCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == true && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser != "prop Bow")
        {
            AfficherCarteZoom();
        }
        else if (gameObject.transform.parent.name == "Deck")
        {
            AfficherCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "BlindTrade" && advairsaire == "IAHand")
        {
            AfficherCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "prop Bow" && advairsaire == "IAHand")
        {
            AfficherCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Graverobbing" && gameObject.transform.parent.name == discardPile.name)
        {
            AfficherCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Recycle" && gameObject.transform.parent.name == discardPile.name)
        {
            AfficherCarteZoom();
        }
        else if(GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Unload" && main.name == "main")
        {
            AfficherCarteZoom();
        }
    }
    protected void sourisSortiCarte() // Permet de remettre le scale a la bonne grandeur la carte
    {
        if (gameObject.transform.parent.name == "main" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler == true && ((GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler == null && advairsaire == "main") || (advairsaire == "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler != null)) && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "PressureTrade")
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == true && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser != "prop Bow")
        {
            EnleverCarteZoom();
        }
        else if (gameObject.transform.parent.name == "Deck")
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "BlindTrade" && advairsaire == "IAHand")
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "prop Bow" && advairsaire == "IAHand")
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Graverobbing" && gameObject.transform.parent.name == discardPile.name)
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Recycle" && gameObject.transform.parent.name == discardPile.name)
        {
            EnleverCarteZoom();
        }
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Unload" && main.name == "main")
        {
            EnleverCarteZoom();
        }
        
    }
    protected void JoueurOuIA()
    {
        if (advairsaire == "IAHand")
        {
            main = GameObject.Find("main");
            discardPile = GameObject.Find("DiscardPile");
            deck = GameObject.Find("Deck");

            mainAdvairsaire = GameObject.Find("IAHand"); // Temporaire
            discardPileAdvairsaire = GameObject.Find("DiscardPileIA");
            deckAdvairsaire = GameObject.Find("DeckIA");
        }
        else if (advairsaire == "main")
        {
            main = GameObject.Find("IAHand");
            discardPile = GameObject.Find("DiscardPileIA");
            deck = GameObject.Find("DeckIA");

            mainAdvairsaire = GameObject.Find("main");
            discardPileAdvairsaire = GameObject.Find("DiscardPile");
            deckAdvairsaire = GameObject.Find("Deck");
        }
    }
    protected void IsProtected()
    {
        for (int i = 0; i < mainAdvairsaire.transform.childCount; i++)
        {
            if (mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Prop Shield" && mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().advairsaire != advairsaire)
            {
                if (mainAdvairsaire.name == "main")
                {
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Protect";
                    GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Don't Protect";
                    StartCoroutine(MyCoroutine(mainAdvairsaire.transform.GetChild(i)));
                    return;
                }
                else
                {
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect = true;
                    discardPileAdvairsaire.GetComponent<DiscardsPiles>().discardCard(mainAdvairsaire.transform.GetChild(i).gameObject);
                    Continuer();
                    return;
                }
            }
        }
        for (int i = 0; i < mainAdvairsaire.transform.childCount; i++)
        {
            


            if (mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Foresight" && typeCard == "Trick")
            {
                if (mainAdvairsaire.name == "main")
                {
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Counter";
                    GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Don't Counter";

                    StartCoroutine(MyCoroutine(mainAdvairsaire.transform.GetChild(i)));
                    return;
                }
                else
                {
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect = true;
                    discardPileAdvairsaire.GetComponent<DiscardsPiles>().discardCard(mainAdvairsaire.transform.GetChild(i).gameObject);
                    Continuer();
                    return;
                }
            }
            else if(mainAdvairsaire.transform.GetChild(i).GetComponent<GenerationCarte>().nom_Carte == "Hasty Sabotage" && typeCard == "Action")
            {
                if (mainAdvairsaire.name == "main")
                {
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sabotage";
                    GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
                    GameObject.Find("Canvas").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Keep";

                    StartCoroutine(MyCoroutine(mainAdvairsaire.transform.GetChild(i)));
                    return;
                }
                else
                {
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect = true;
                    discardPileAdvairsaire.GetComponent<DiscardsPiles>().discardCard(mainAdvairsaire.transform.GetChild(i).gameObject);
                    Continuer();
                    return;
                }
            }
        }
        Continuer();
    }

    IEnumerator MyCoroutine(Transform card)
    {
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction = true;
        yield return new WaitUntil(() => GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse == true);
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().coroutineEnAction = false;
        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect == true)
        {
            if (card.GetComponent<GenerationCarte>().nom_Carte == "Prop Shield")
            {
                discardPileAdvairsaire.GetComponent<DiscardsPiles>().discardCard(card.gameObject);
            }
            else
            {
                discardPile.GetComponent<DiscardsPiles>().discardCard(card.gameObject);
            }
            
        }
        GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
        GameObject.Find("Canvas").transform.GetChild(5).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().boutonUse = false;
        Continuer();
    }
    void Continuer()
    {
        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().protect == true)
        {
            Debug.Log("Protect");
            discard();
            EnleverCarteZoom();
            clearMemoire(); // Clear la memoire
        }
        else
        {
            if (discarded == true && discardPile.GetComponent<DiscardsPiles>().carteASauver == null && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Recycle") // Pour chercher une carte de la discard pile, ne pas mettre de condition dadans
            {
                discardPile.GetComponent<DiscardsPiles>().CarteASauver(gameObject); // Fonction qui va chercher la carte
                clearMemoire();
                //MaggotGive();
            }
            else if (discarded == true && discardPileAdvairsaire.GetComponent<DiscardsPiles>().carteASauver == null && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Graverobbing") // Pour chercher une carte de la discard pile, ne pas mettre de condition dadans
            {
                Debug.Log("Hello");
                discardPile.GetComponent<DiscardsPiles>().CarteASauver(gameObject); // Fonction qui va chercher la carte
                clearMemoire();

            }
            else if (gameObject.transform.parent.name == "Deck" && GameObject.Find("Deck").GetComponent<deck_joueur>().regarder == false) // Si un joueur regarde le deck pour prendre une carte
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "TradeForQuality") // Si TradeForQuality est utiliser (Apres la partie inférieur)
                {
                    GameObject.Find("Deck").GetComponent<deck_joueur>().ChercherCarte(gameObject); // Va chercher la carte selectionner par le joueur
                    GameObject.Find("Deck").GetComponent<deck_joueur>().PutCarteAVoir(); // Remet le deck a sa place pour reprendre sa main
                    clearMemoire(); // Clear la memoire
                }
                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Pack of Death")
                {
                    GameObject.Find("Deck").GetComponent<deck_joueur>().ChercherCarte(gameObject); // Va chercher la carte selectionner par le joueur
                    GameObject.Find("Deck").GetComponent<deck_joueur>().PutCarteAVoir(); // Remet le deck a sa place pour reprendre sa main
                    clearMemoire(); // Clear la memoire
                }
            }
            else if (GameObject.Find("MainVirtuelle") != null)
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Cherry Pick")
                {
                    main.SetActive(true);
                    gameObject.transform.parent = deck.transform;
                    gameObject.transform.SetSiblingIndex(0);
                    main.GetComponent<Mains>().PigerUneCarte();
                    MainVirtuelle(GameObject.Find("MainVirtuelle"));
                }
                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Witchcraft")
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 0)
                    {
                        main.SetActive(true);
                        gameObject.transform.parent = deck.transform;
                        gameObject.transform.SetSiblingIndex(0);
                        main.GetComponent<Mains>().PigerUneCarte();
                        main.SetActive(false);
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 1;
                    }
                    else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 1)
                    {
                        main.SetActive(true);
                        main.GetComponent<Mains>().cartesDisponibles.Remove(gameObject);
                        discard();
                        MainVirtuelle(GameObject.Find("MainVirtuelle"));
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 0;
                        clearMemoire();
                    }
                }
            }

            else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler == true) // VaVoler sert a faire en sort qu'une carte change de main, peut etre utiliser pour les échange
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "PressureTrade") // Si PressureTrade est utiliser
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler == null && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire != advairsaire) // Si le joueur n'a pas choisi la carte de l'opposant
                    {
                        VolerCarteAdvairsaire(gameObject); // Execute VolerCarteAdvairsaire qui prend sa carte
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler = gameObject; //set objet voler dans la memoire
                    }

                    else if (advairsaire == "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler != null && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire == advairsaire) // Si le joueur a choisi la carte de l'opposant
                    {
                        VolerCarteAdvairsaire(gameObject); // Execute VolerCarteAdvairsaire qui donne sa carte a l'opposant
                        clearMemoire(); // Clear la memoire
                    }
                }

                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "BlindTrade") // si BlindTrade est utiliser
                {
                    if (advairsaire == "IAHand") // Si l'advairsaire est l'IA
                    {
                        VolerCarteAdvairsaire(gameObject); // Execute VolerCarteAdvairsaire qui donne sa carte a l'opposant
                        int carteRetour = Random.Range(0, GameObject.Find("IAHand").transform.childCount); // TEMPORAIRE !!! Selectionner la carte que l'ia revois aléatoirement
                        VolerCarteAdvairsaire(GameObject.Find("IAHand").transform.GetChild(carteRetour).gameObject); // Execute VolerCarteAdvairsaire qui prend sa carte
                        clearMemoire(); // Reset la memoire
                    }
                }

                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Unload")
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 0 || GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 1) // Si c'est la premiere ou deuxieme carte donnée
                    {
                        VolerCarteAdvairsaire(gameObject);
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard += 1;
                        if (main.transform.childCount == 0)
                        {
                            clearMemoire();
                        }
                    }
                    else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 2)
                    {
                        VolerCarteAdvairsaire(gameObject);
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 0;
                        clearMemoire();
                    }
                }
            }

            else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == true) // VaDetruire sert a sois détruire une carte ou de la discard
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Questionable technique") // Est true si Questionable technique est utiliser
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().choixEntreDiscardOuDestroy == "Destroy") // Est true si la carte choisi doit etre détruit
                    {
                        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire == advairsaire) // Si l'adversaire est le meme que la carte activer
                        {
                            DetruireCarte(gameObject); // Fonction pour détruire la carte
                            GameObject.Find("Canvas").transform.GetChild(1).GetComponent<ChoisirDestroy>().DesactiverInteractionBouton();
                            GameObject.Find("Canvas").transform.GetChild(2).GetComponent<ChoisirDiscard>().DesactiverInteractionBouton();
                            clearMemoire(); // Reset la mémoire de MemoireDesCartes
                        }
                    }

                    else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().choixEntreDiscardOuDestroy == "Discard") // Est true si la carte choisi doit etre discard
                    {
                        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire == advairsaire)// Si l'adversaire est le meme que la carte activer
                        {
                            discard(); // Fonction pour discard la carte
                            GameObject.Find("Canvas").transform.GetChild(1).GetComponent<ChoisirDestroy>().DesactiverInteractionBouton();
                            GameObject.Find("Canvas").transform.GetChild(2).GetComponent<ChoisirDiscard>().DesactiverInteractionBouton();
                            clearMemoire(); // Reset la mémoire de MemoireDesCartes
                        }
                    }
                }

                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "TradeForNumber") // Est true si TradeForNumber est utiliser
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire == advairsaire) // Si l'adversaire est le meme que la carte activer
                    {
                        discard(); // Fonction pour discard la carte
                        clearMemoire(); // Fonction pour clear la mémoire de MemoireDesCartes
                        for (int i = 0; i < 3; i++) // Se repete 3 fois
                        {
                            if (deck.transform.childCount == 0) break;
                            main.GetComponent<Mains>().PigerUneCarte(); // Le joueur pige une carte
                        }
                    }
                }

                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "TradeForQuality") // Est true si TradeForQuality est utiliser
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire == advairsaire) // Si l'adversaire est le meme que la carte activer
                    {
                        if (gameObject.transform.parent.name == "main") // Si le parent est main
                        {
                            discard(); // Fonction pour discard
                            GameObject.Find("Deck").GetComponent<deck_joueur>().VoirCarteDessus(5); // Fonction pour voir les 5 cartes dessus ton deck
                            GameObject.Find("Deck").GetComponent<deck_joueur>().regarder = false; // Desactiver la variable regarder pour choisir une carte au lieu de juste regarder
                            // Suite dans gameObject.transform.parent.name == "Deck"
                        }
                    }
                }

                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "HastySearch" || GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "ZombieHand") // Si HastySearch ou ZombieHand est utiliser (Si le joueur doit discard + d'une carte)
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 0) // Si c'est la premiere carte discard
                    {
                        discard(); // Fonction pour discard
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 1; // Ajoute 1 au compteur de fois utiliser
                        if (main.transform.childCount == 0)
                        {
                            clearMemoire();
                        }
                    }
                    else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 1)// Si c'Est la deuxieme carte discard
                    {
                        discard();// Fonction pour discard
                        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 0; // remet a 0 le compteur de fois utiliser
                        clearMemoire(); // Fonction pour clear la memoire
                    }
                }
                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "prop Bow")
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire == gameObject.transform.parent.name)
                    {
                        discard();
                        clearMemoire();
                    }
                }
                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Prop Sword")
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<GenerationCarte>().advairsaire != gameObject.transform.parent.name)
                    {
                        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 0) // Si c'est la premiere carte discard
                        {
                            discard(); // Fonction pour discard
                            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 1; // Ajoute 1 au compteur de fois utiliser
                            if (main.transform.childCount == 0)
                            {
                                clearMemoire();
                            }
                        }
                        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 1)// Si c'Est la deuxieme carte discard
                        {
                            discard();// Fonction pour discard
                            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 0; // remet a 0 le compteur de fois utiliser
                            clearMemoire(); // Fonction pour clear la memoire
                        }
                    }
                }
            }
            else if (main.name == "main" && nom_Carte != "Prop Shield" && nom_Carte != "Trick Coin" && nom_Carte != "Foresight" && nom_Carte != "Hasty Sabotage" && nom_Carte != "FlareToLife")
            {
                EffetCarte(); // Effet de la carte utilliser
                // Notify TurnManager that the active player played a card (SendMessage avoids direct type reference)
                var tmObj2 = GameObject.Find("TurnManager");
                if (tmObj2 != null)
                    tmObj2.SendMessage("OnCardPlayed", gameObject.transform.parent != null ? gameObject.transform.parent.name : "main", SendMessageOptions.DontRequireReceiver);
            }

            // NOTE : ici il y a toute les if qui sont dans toutes les cartes, sois pour volé ou detruire ou discard, etc. IMPORTANT D'UTILISER LA MEMOIRE ET DE LA CLEAR
        }
        if (deck.transform.childCount == 0)
        {
            Debug.Log("Deck vide");
        }
        else
        {
            if (main.GetComponent<Mains>().cartesMain.Count == 0)
            {
                if (main.GetComponent<Mains>().cartesDisponibles.Count > 0)
                {
                    main.GetComponent<Mains>().PigerMainDeDepart();
                }
            }
            if (mainAdvairsaire.GetComponent<Mains>().cartesMain.Count == 0)
            {
                if (mainAdvairsaire.GetComponent<Mains>().cartesDisponibles.Count > 0)
                {
                    mainAdvairsaire.GetComponent<Mains>().PigerMainDeDepart();
                }
            }
        }
    }

    public GameObject MainVirtuelle(GameObject mainVirtuelle)
    {
        if (mainVirtuelle == null)
        {
            main.SetActive(false);
            mainVirtuelle = Instantiate(main);
            mainVirtuelle.name = "MainVirtuelle";
            mainVirtuelle.SetActive(true);
            mainVirtuelle.GetComponent<Mains>().cartesMain.Clear();
            for (int i = 0; i < mainVirtuelle.transform.childCount; i++)
            {
                mainVirtuelle.GetComponent<Mains>().DetruireCarte(mainVirtuelle.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < 3; i++)
            {
                mainVirtuelle.GetComponent<main_joueur>().PigerUneCarteVirtuelle(mainVirtuelle);
            }
            //mainVirtuelle.GetComponent<Mains>().OrganiserLaMain();
            Destroy(mainVirtuelle.GetComponent<Mains>());
            return mainVirtuelle;
        }
        else
        {
            main.SetActive(true);
            if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Cherry Pick")
            {
                
                for (int i = 0; i < 2; i++)
                {
                    GameObject carte = mainVirtuelle.transform.GetChild(0).gameObject;
                    discardPile.GetComponent<DiscardsPiles>().discardCard(carte);
                    main.GetComponent<Mains>().cartesDisponibles.Remove(carte);

                }
            }
            else if(GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Witchcraft")
            {
                GameObject carte = mainVirtuelle.transform.GetChild(0).gameObject;
                DetruireCarte(carte.gameObject);
                main.GetComponent<Mains>().cartesDisponibles.Remove(carte);
            }
            Destroy(mainVirtuelle);
            return null;
        }
    }
}

// VaVoler : variable bool qui indique qu'une carte va etre envoyer de l'autre main (ex. main -> mainIA)
// VaDetruire : variable bool qui indique qu'une carte va etre sois détruite ou discard