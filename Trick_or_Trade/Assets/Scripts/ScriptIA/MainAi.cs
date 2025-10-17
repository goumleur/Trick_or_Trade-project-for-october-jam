using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class MainAi : Mains
{
    //private List<GameObject> cartesDisponibles = new List<GameObject>(); 
    public SpriteRenderer spriteRenderer; // Assurez-vous d'assigner ce champ dans l'éditeur Unity
    public Sprite nouveauSprite; // Le nouveau sprite à afficher
    public Sprite ImageFacedown; // Le sprite à afficher face cachée (si nécessaire)




    void Start()
    {
        
        InitialiserDeck();
        GameObject.Find("DeckIA").GetComponent<DeckIA>().melanger_deck();
        PigerMainDeDepart(); // Le joueur pioche 8 cartes au début
    }

    //  Remplit le deck avec toutes les cartes possibles
    void InitialiserDeck()
    {
        cartesDisponibles.Clear();
        for (int i = 0; i < 40; i++)
        {
            GameObject carteModel = GameObject.Find("Candy"); // Trouver le model de carte
            Transform parent = GameObject.Find("DeckIA").transform; // Trouver le deck pour le futur enfant
            GameObject carteClone = Instantiate(carteModel, parent, true); // cloner le model & set le parent de la carte (a deck)
            carteClone.name = (i + 40).ToString(); // Set le nom au numéro de génération
            cartesDisponibles.Add(carteClone); // Ajouter la carte dans la liste
            carteClone.tag = "Untagged";
        }
        Debug.Log("Deck initialisé avec " + cartesDisponibles.Count + " cartes.");
        gameObject.GetComponent<MainAi>().spriteChanger();
    }
    override public void modifierCarte(Transform carteAModifier, GameObject carteResultant)
    {
        Debug.Log("Marche");
        GameObject carteModel = carteResultant; // Trouver le model de carte
        Transform parent = GameObject.Find("DeckIA").transform; // Trouver le deck pour le futur enfant
        GameObject carteClone = Instantiate(carteModel, parent, true); // cloner le model & set le parent de la carte (a deck)
        carteClone.name = carteAModifier.name; // Set le nom au numéro de génération
        cartesDisponibles.Add(carteClone); // Ajouter la carte dans la liste
        cartesDisponibles.Remove(carteAModifier.gameObject);
        Destroy(carteAModifier.gameObject);
        carteClone.tag = "Untagged";
    }
    override public void modifierCarteMain(Transform carteAModifier, GameObject carteResultant, GameObject parent)
    {
        GameObject carteModel = carteResultant; // Trouver le model de carte
        GameObject carteClone = Instantiate(carteModel, parent.transform, true); // cloner le model & set le parent de la carte (a deck)
        carteClone.name = carteAModifier.name; // Set le nom au numéro de génération
        if(parent.name == "IAHand")
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
            if (cartesDisponibles.Count == 0)
            {
                Debug.LogWarning("Le deck est vide !");
                return;
            }
            GameObject carte = GameObject.Find("DeckIA").transform.GetChild(0).gameObject;
            cartesDisponibles.Remove(carte); // Enleve dans la list
            cartesMain.Add(carte);
            GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
            carte.transform.SetParent(parent.transform, worldPositionStays: true); // Set le parent de la carte a main


            Invoke("OrganiserLaMain", 0.1f); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur
        }

        Debug.Log("Main de départ tirée (" + cartesMain.Count + " cartes)");
    }

    //  Pioche une seule carte pendant le jeu
    override public void PigerUneCarte()
    {
        GameObject carte = GameObject.Find("DeckIA").transform.GetChild(0).gameObject;
        cartesDisponibles.Remove(carte); // Enleve dans la list
        cartesMain.Add(carte);
        GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true); // Set le parent de la carte a main
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
        GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true);
        GameObject.Find("main").GetComponent<main_joueur>().cartesMain.Remove(carte);
        GameObject.Find("main").GetComponent<main_joueur>().OrganiserLaMain();
        OrganiserLaMain(); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur

    }
    override public void DetruireCarte(GameObject carte)
    {
        cartesMain.Remove(carte);
        Destroy(carte);
        OrganiserLaMain();
    }
    override public void SauverCarte(GameObject cartesASauver)
    {
        cartesMain.Add(cartesASauver);
        GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
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
                Vector3 pos = new Vector3(Mathf.Sin(rad) * rayon, Mathf.Cos(-rad) * rayon, i * 10); // Calculer la position de la carte
                cartesMain[i].transform.localPosition = pos; // Mettre la carte en position
                cartesMain[i].transform.localRotation = Quaternion.Euler(0f, 0f, angle); // set l'angle de la carte

                // Set l'orderInLayer de la carte
                cartesMain[i].transform.GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 101 - 2 * i;
                cartesMain[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 100 - 2 * i;
                cartesMain[i].GetComponent<SpriteRenderer>().sprite = ImageFacedown;
            }
            else
            {
                cartesMain[i].transform.localPosition = new Vector2(0f, 10f); // Mettre la carte en position
                cartesMain[i].transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // set l'angle de la carte
                cartesMain[i].GetComponent<SpriteRenderer>().sprite = ImageFacedown;
            }
        }

    }
    public void spriteChanger()
    {
        if (spriteRenderer != null && nouveauSprite != null)
        {
            spriteRenderer.sprite = nouveauSprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer ou nouveauSprite n'est pas assigné dans l'inspecteur.");
        }
    }
}