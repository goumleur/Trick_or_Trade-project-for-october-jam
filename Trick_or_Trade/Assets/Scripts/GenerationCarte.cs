using UnityEngine;
using TMPro;


public class GenerationCarte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public string nom_Carte = "Carte Inconnue";
    [SerializeField] protected string description_Carte = "Aucune description";

    [SerializeField] protected Sprite backRound;
    [SerializeField] protected Sprite illustration;
    protected bool discarded = false;

    public void discard()
    {
        discarded = true;
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().discardCard(gameObject);
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
        EnleverCarteZoom();
        if (advairsaire == "main")
        {
            GameObject.Find("Main Camera").GetComponent<main_joueur>().PrendreCarte(carteViser);
        }
        else if (advairsaire == "IAHand")
        {
            Debug.Log("Activer");
            GameObject.Find("IAHand").GetComponent<MainAi>().PrendreCarte(carteViser);
        }
        TrouverAdvairsaire();
    }
    protected void DetruireCarteAdvairsaire(GameObject carteViser) // Permet de détruire UNIQUEMENT la carte
    {
        Debug.Log(advairsaire);
        if (advairsaire == "main")
        {
            Debug.Log("Option 1");
            GameObject.Find("IAHand").GetComponent<MainAi>().DetruireCarte(carteViser);
        }
        else if (advairsaire == "IAHand")
        {
            Debug.Log("Option 2");
            GameObject.Find("Main Camera").GetComponent<main_joueur>().DetruireCarte(carteViser);
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
    }
    protected void clearMemoire() // Reset la memoire de MemoireDesCartes
    {
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = null;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = null;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = false;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler = false;
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler = null;
    }

    protected void click() // Condition auquelle toute les cartes peuvent etre affecter (discard, detruire, sauver, voler, trade(trade c 2 Vavoler))
    {
        if (discarded == true && GameObject.Find("DiscardPile").GetComponent<DiscardPile>().carteASauver == null) // Pour chercher une carte de la discard pile, ne pas mettre de condition dadans
        {
            GameObject.Find("DiscardPile").GetComponent<DiscardPile>().CarteASauver(gameObject); // Fonction qui va chercher la carte
        }

        else if (gameObject.transform.parent.name == "Deck" && GameObject.Find("Deck").GetComponent<deck_joueur>().regarder == false) // Si un joueur regarde le deck pour prendre une carte
        {
            if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "TradeForQuality") // Si TradeForQuality est utiliser (Apres la partie inférieur)
            {
                GameObject.Find("Deck").GetComponent<deck_joueur>().ChercherCarte(gameObject); // Va chercher la carte selectionner par le joueur
                GameObject.Find("Deck").GetComponent<deck_joueur>().PutCarteAVoir(); // Remet le deck a sa place pour reprendre sa main
                clearMemoire(); // Clear la memoire
            }
        }

        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler == true) // VaVoler sert a faire en sort qu'une carte change de main, peut etre utiliser pour les échange
        {
            if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "PressureTrade") // Si PressureTrade est utiliser
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler == null && advairsaire == "main") // Si le joueur n'a pas choisi la carte de l'opposant
                {
                    VolerCarteAdvairsaire(gameObject); // Execute VolerCarteAdvairsaire qui prend sa carte
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler = gameObject; //set objet voler dans la memoire
                }

                else if (advairsaire == "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetVoler != null) // Si le joueur a choisi la carte de l'opposant
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
        }

        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == true) // VaDetruire sert a sois détruire une carte ou de la discard
        {
            if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "Questionable technique") // Est true si Questionable technique est utiliser
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().choixEntreDiscardOuDestroy == "Destroy") // Est true si la carte choisi doit etre détruit
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<QuestionableTechnique>().advairsaire == advairsaire) // Si l'adversaire est le meme que la carte activer
                    {
                        DetruireCarteAdvairsaire(gameObject); // Fonction pour détruire la carte
                        GameObject.Find("Canvas").transform.GetChild(1).GetComponent<ChoisirDestroy>().DesactiverInteractionBouton();
                        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<ChoisirDiscard>().DesactiverInteractionBouton();
                        clearMemoire(); // Reset la mémoire de MemoireDesCartes
                    }
                }

                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().choixEntreDiscardOuDestroy == "Discard") // Est true si la carte choisi doit etre discard
                {
                    if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<QuestionableTechnique>().advairsaire == advairsaire)// Si l'adversaire est le meme que la carte activer
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
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<TradeForNumber>().advairsaire == advairsaire) // Si l'adversaire est le meme que la carte activer
                {
                    discard(); // Fonction pour discard la carte
                    clearMemoire(); // Fonction pour clear la mémoire de MemoireDesCartes
                    for (int i = 0; i < 3; i++) // Se repete 3 fois
                    {
                        GameObject.Find("Main Camera").GetComponent<main_joueur>().PigerUneCarte(); // Le joueur pige une carte
                    }
                }
            }

            else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "TradeForQuality") // Est true si TradeForQuality est utiliser
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser.GetComponent<TradeForQuality>().advairsaire == advairsaire) // Si l'adversaire est le meme que la carte activer
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

            else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "HastySearch" || GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "ZombieHand") // Si HastySearch ou ZombieHand est utiliser
            {
                if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 0) // Si c'est la premiere carte discard
                {
                    discard(); // Fonction pour discard
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 1; // Ajoute 1 au compteur de fois utiliser
                }
                else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard == 1)// Si c'Est la deuxieme carte discard
                {
                    discard();// Fonction pour discard
                    GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().multipleDiscard = 0; // remet a 0 le compteur de fois utiliser
                    clearMemoire(); // Fonction pour clear la memoire
                }
            }
        }
        else
        {
            EffetCarte(); // Effet de la carte utilliser
        }

        // NOTE : ici il y a toute les if qui sont dans toutes les cartes, sois pour volé ou detruire ou discard, etc. IMPORTANT D'UTILISER LA MEMOIRE ET DE LA CLEAR
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
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == true)
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
        else if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == true)
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
    }
}

// VaVoler : variable bool qui indique qu'une carte va etre envoyer de l'autre main (ex. main -> mainIA)
// VaDetruire : variable bool qui indique qu'une carte va etre sois détruite ou discard