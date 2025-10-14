using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class MainAi : MonoBehaviour
{
    //private List<GameObject> cartesDisponibles = new List<GameObject>(); 
    private Dictionary<int, GameObject> cartesDisponibles = new Dictionary<int, GameObject>(); // Le "deck" complet (1 à 40)
    public List<GameObject> cartesMain = new List<GameObject>(); // Les cartes actuellement en main
    public GameObject carte;
    public Transform zoneMain; public SpriteRenderer spriteRenderer; // Assurez-vous d'assigner ce champ dans l'éditeur Unity
    public Sprite nouveauSprite; // Le nouveau sprite à afficher
    public Sprite ImageFacedown; // Le sprite à afficher face cachée (si nécessaire)
  




    void Start()
    {
        InitialiserDeck();
        PigerMainDeDepart(); // Le joueur pioche 8 cartes au début
    }

    //  Remplit le deck avec toutes les cartes possibles
    void InitialiserDeck()
    {
        cartesDisponibles.Clear();
        for (int i = 0; i < 40; i++)
        {
            GameObject carteModel = GameObject.Find("CarteModel"); // Trouver le model de carte
            Transform parent = GameObject.Find("DeckIA").transform; // Trouver le deck pour le futur enfant
            GameObject carteClone = Instantiate(carteModel, parent, true); // cloner le model & set le parent de la carte (a deck)
            carteClone.name = (i + 40).ToString(); // Set le nom au numéro de génération
            cartesDisponibles.Add(i, carteClone); // Ajouter la carte dans le dictionnaire
        }
        Debug.Log("Deck initialisé avec " + cartesDisponibles.Count + " cartes.");
        gameObject.GetComponent<MainAi>().spriteChanger();
    }

    //  Pioche 8 cartes différentes au début du round
    public void PigerMainDeDepart()
    {
        cartesMain.Clear();

        for (int i = 0; i < 8; i++)
        {
            if (cartesDisponibles.Count == 0)
            {
                Debug.LogWarning("Le deck est vide !");
                return;
            }

            var liste = cartesDisponibles.ToList(); // Transformer le dictionnaire en list
            int indexAleatoire = Random.Range(0, liste.Count); // Chercher un nombre aléatoire dans la liste
            var carteDictionnaire = liste[indexAleatoire]; // Prendre la valeur de la liste
            GameObject carte = carteDictionnaire.Value; // Prendre le model de carte dans le dictionnaire
            //Debug.Log($"La carte avec la clé : {carteDictionnaire.Key}"); // Afficher la carte piocher
            cartesDisponibles.Remove(carteDictionnaire.Key); // Enleve la clé du dictionnaire
            cartesMain.Add(carte);
            GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
            carte.transform.SetParent(parent.transform, worldPositionStays: true); // Set le parent de la carte a main


            Invoke("OrganiserLaMain", 0.1f); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur
        }

        Debug.Log("Main de départ tirée (" + cartesMain.Count + " cartes)");
    }

    //  Pioche une seule carte pendant le jeu
    public void PigerUneCarte()
    {
        if (cartesDisponibles.Count == 0)
        {
            InitialiserDeck();
        }

        var liste = cartesDisponibles.ToList(); // Transformer le dictionnaire en list
        int indexAleatoire = Random.Range(0, liste.Count); // Chercher un nombre aléatoire dans la liste
        var carteDictionnaire = liste[indexAleatoire]; // Prendre la valeur de la liste
        GameObject carte = carteDictionnaire.Value; // Prendre le model de carte dans le dictionnaire
        //Debug.Log($"La carte avec la clé : {carteDictionnaire.Key}"); // Afficher la carte piocher
        cartesDisponibles.Remove(carteDictionnaire.Key); // Enleve la clé du dictionnaire
        cartesMain.Add(carte);
        GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true);// Set le parent de la carte a main
        OrganiserLaMain();
    }
    public void PrendreCarte(GameObject carte)
    {
        cartesMain.Add(carte);
        GameObject parent = GameObject.Find("IAHand"); // Chercher l'objet main
        carte.transform.SetParent(parent.transform, worldPositionStays: true);
        GameObject.Find("Main Camera").GetComponent<main_joueur>().cartesMain.Remove(carte);
        GameObject.Find("Main Camera").GetComponent<main_joueur>().OrganiserLaMain();
        OrganiserLaMain(); // Appeller la fonction pour organiser la main a 0,1 sec apres pour éviter les erreur

    }
    public void DetruireCarte(GameObject carte)
    {
        cartesMain.Remove(carte);
        Destroy(carte);
        OrganiserLaMain();
    }
    public void OrganiserLaMain()
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
                cartesMain[i].GetComponent<GenerationCarte>().afficher_carte();
               TextMeshPro t1 = cartesMain[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
                 TextMeshPro TMP = cartesMain[i].transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
                t1.text = "";
                TMP.text = "";
                
            }
            else
            {
                cartesMain[i].transform.localPosition = new Vector2(0f, 10f); // Mettre la carte en position
                cartesMain[i].transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // set l'angle de la carte
                cartesMain[i].GetComponent<SpriteRenderer>().sprite = ImageFacedown;
                cartesMain[i].GetComponent<GenerationCarte>().afficher_carte();
                TextMeshPro t1 = cartesMain[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
                t1.text = "";
                TextMeshPro TMP = cartesMain[i].transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
                TMP.text = "";

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