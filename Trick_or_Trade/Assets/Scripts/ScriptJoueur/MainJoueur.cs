using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UIElements;

public class main_joueur : Mains
{
    //private List<GameObject> cartesDisponibles = new List<GameObject>(); 
    void Start()
    {
        // If the menu saved a selected deck CSV, use it. Otherwise use default preset.
        if (PlayerPrefs.HasKey("SelectedDeckCsv"))
        {
            string csv = PlayerPrefs.GetString("SelectedDeckCsv");
            if (!string.IsNullOrEmpty(csv))
            {
                var parts = csv.Split('|');
                var list = new System.Collections.Generic.List<string>(parts);
                DeckInitializer.PopulateDeckFromNames(list, "Deck");
                Debug.Log("Deck initialized from menu selection.");
            }
            else
            {
                InitialiserDeck();
                Debug.Log("Deck initialisé (preset 20 cards).");
            }

            // clear the saved selection so future runs don't reuse it unexpectedly
            PlayerPrefs.DeleteKey("SelectedDeckCsv");
        }
        else
        {
            InitialiserDeck();
            Debug.Log("Deck initialisé (preset 20 cards).");
        }

        GameObject.Find("Deck").GetComponent<deck_joueur>().melanger_deck();
        // Draw the opening hand after deck is ready
        Invoke("PigerMainDeDepart", 0.01f);
    }

    // Initialize the player's deck with a preset of 20 card prefabs (fallback to 'Candy')
    void InitialiserDeck()
    {
        cartesDisponibles.Clear();

        var preset = new List<string> {
            "Candy", "ToxicSpike", "PropShield", "PropBow", "LuckyFind", "FreshStart", "Fantastical_Reclaimation",
            "Blathan_theft", "Incinerate", "LightEmUp", "ScrabbledNote", "FaithlessAnnihilation", "Blade_Of_Nightmare",
            "TheOlSwitcheroo", "WatchTheWorldBurn", "GenerousDonation", "SpiderFromNowhere", "Spill", "Recycle", "CherryPick"
        };

        Transform parent = GameObject.Find("Deck").transform;
        for (int i = 0; i < preset.Count; i++)
        {
            string name = preset[i];
            GameObject model = GameObject.Find(name);
            if (model == null)
            {
                model = GameObject.Find("Candy");
            }
            if (model == null) continue; // nothing to instantiate

            GameObject clone = Instantiate(model, parent, true);
            clone.name = (i).ToString();
            clone.tag = "Untagged";
            cartesDisponibles.Add(clone);
        }
    }
    override public void modifierCarte(Transform carteAModifier, GameObject carteResultant)
    {
        GameObject carteModel = carteResultant; // Trouver le model de carte
        Transform parent = GameObject.Find("Deck").transform; // Trouver le deck pour le futur enfant
        GameObject carteClone = Instantiate(carteModel, parent, true); // cloner le model & set le parent de la carte (a deck)
        carteClone.name = carteAModifier.name; // Set le nom au numéro de génération
        cartesDisponibles.Add(carteClone); // Ajouter la carte dans la liste
        cartesDisponibles.Remove(carteAModifier.gameObject);
        Destroy(carteAModifier.gameObject);
        carteClone.tag = "Untagged";
    }
    override public void modifierCarteMain(Transform carteAModifier, GameObject carteResultant, GameObject parent)
    {
        Debug.Log("Marche");
        GameObject carteModel = carteResultant; // Trouver le model de carte
        GameObject carteClone = Instantiate(carteModel, parent.transform, true); // cloner le model & set le parent de la carte (a deck)
        carteClone.name = carteAModifier.name; // Set le nom au numéro de génération
        if(parent.name == "main")
        {
            cartesMain.Add(carteClone); // Ajouter la carte dans la liste
            cartesMain.Remove(carteAModifier.gameObject);
        }  
        Destroy(carteAModifier.gameObject);
        carteClone.tag = "Untagged";
        OrganiserLaMain();
    }
    //  Pioche 8 cartes différentes au début du round
    override public void PigerMainDeDepart()
    {
        cartesMain.Clear();

        for (int i = 0; i < 8; i++)
        {
            // If cartesDisponibles is empty (e.g., deck was populated via DeckInitializer but list wasn't set),
            // repopulate it from Deck children so drawing can proceed.
            if (cartesDisponibles.Count == 0)
            {
                var deck = GameObject.Find("Deck");
                if (deck != null)
                {
                    for (int d = 0; d < deck.transform.childCount; d++) cartesDisponibles.Add(deck.transform.GetChild(d).gameObject);
                    Debug.Log("PigerMainDeDepart: cartesDisponibles was empty, repopulated from Deck children.");
                }
            }

            if (cartesDisponibles.Count == 0)
            {
                Debug.LogWarning("Le deck est vide !");
                return;
            }

            GameObject carte = GameObject.Find("Deck").transform.GetChild(0).gameObject;
            cartesDisponibles.Remove(carte); // Enleve dans la list
            cartesMain.Add(carte);
            GameObject parent = GameObject.Find("main"); // Chercher l'objet main
            carte.transform.SetParent(parent.transform, worldPositionStays: true); // Set le parent de la carte a main


            Invoke("OrganiserLaMain", 0.1f); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur
        }

    }

    //  Pioche une seule carte pendant le jeu
    override public void PigerUneCarte()
    {
        GameObject carte = GameObject.Find("Deck").transform.GetChild(0).gameObject;
        cartesDisponibles.Remove(carte); // Enleve dans la list
        cartesMain.Add(carte);
        GameObject parent = GameObject.Find("main"); // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true); // Set le parent de la carte a main
        OrganiserLaMain();
    }
    public void PigerUneCarteVirtuelle(GameObject mainVirtuel)
    {
        GameObject carte = GameObject.Find("Deck").transform.GetChild(0).gameObject;
        cartesDisponibles.Remove(carte); // Enleve dans la list
        cartesMain.Add(carte);
        GameObject parent = mainVirtuel; // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true); // Set le parent de la carte a main
        //carte.transform.localPosition = new Vector3(0f, 0f, 0f);
        OrganiserLaMain();
    }
    override public void DiscardFait(GameObject card)
    {
        cartesMain.Remove(card);
        OrganiserLaMain();
    }
    override public void PrendreCarte(GameObject carte)
    {
        cartesMain.Add(carte);
        GameObject parent = GameObject.Find("main"); // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true);
        GameObject.Find("IAHand").GetComponent<MainAi>().cartesMain.Remove(carte);
        GameObject.Find("IAHand").GetComponent<MainAi>().OrganiserLaMain();
        Invoke("OrganiserLaMain", 0.1f); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur
    }
    override public void DetruireCarte(GameObject carte)
    {
        Debug.Log("Detruit");
        cartesMain.Remove(carte);
        Destroy(carte);
        OrganiserLaMain();
    }
    override public void SauverCarte(GameObject cartesASauver)
    {
        cartesMain.Add(cartesASauver);
        GameObject parent = GameObject.Find("main"); // Chercher l'objet main
        cartesASauver.transform.SetParent(parent.transform, worldPositionStays: true);
        Invoke("OrganiserLaMain", 0.1f); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur
    }
    override public void OrganiserLaMain()
    {

        float angleTotal = 30f;
        float angleParCarte = angleTotal / (cartesMain.Count - 1);
        float rayon = 10f;

        for (int i = 0; i < cartesMain.Count; i++)
        {
            if (cartesMain.Count > 1)
            {
                float angle = -angleTotal / 2 + i * angleParCarte; // Met l'angle de la carte
                float rad = -angle * Mathf.Deg2Rad; // Le fait que les carte sont mis en cercle
                Vector3 pos = new Vector3(Mathf.Sin(rad) * rayon, Mathf.Cos(rad) * rayon, i * 10); // Calculer la position de la carte
                cartesMain[i].transform.localPosition = pos; // Mettre la carte en position
                cartesMain[i].transform.localRotation = Quaternion.Euler(0f, 0f, angle); // set l'angle de la carte

                // Set l'orderInLayer de la carte
                cartesMain[i].transform.GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 101 - 2 * i;
                cartesMain[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 100 - 2 * i;
            }
            else
            {
                cartesMain[i].transform.localPosition = new Vector2(0f, 10f); // Mettre la carte en position
                cartesMain[i].transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // set l'angle de la carte
            }
        }

    }

}