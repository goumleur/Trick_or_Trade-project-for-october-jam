using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class GenerationCarte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] protected string nom_Carte = "Carte Inconnue";
    [SerializeField] protected string description_Carte = "Aucune description";

    [SerializeField] protected Sprite backRound;
    [SerializeField] protected Sprite illustration;

    public string advairsaire;

    GameObject clone;
    int oldOrderInLayer;

    public virtual void CreerLaCarte() { }
   [SerializeField] protected void afficher_carte()
    {
        TextMeshPro tmp = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
        tmp.text = "name : " + nom_Carte + "\n" + "\n";
        TextMeshPro t1 = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshPro>();
        t1.text = "description : " + description_Carte;
        
    }

    protected void discardCard()
    {
        Debug.Log("Début");
        Transform cFrameCarte = gameObject.GetComponent<Transform>();
        GameObject discardPile = GameObject.Find("DiscardPile");
        Transform cFrameDiscardPile = discardPile.GetComponent<Transform>();
        cFrameCarte.transform.SetPositionAndRotation(cFrameDiscardPile.transform.position, Quaternion.Euler(0f, 90f, 0f));
    }

    protected void VolerCarteAdvairsaire(GameObject carteViser)
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
    protected void AfficherCarteZoom()
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
    protected void EnleverCarteZoom()
    {
        Debug.Log(oldOrderInLayer);
        gameObject.GetComponent<Renderer>().sortingOrder = oldOrderInLayer;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = oldOrderInLayer;
        gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = oldOrderInLayer;
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = oldOrderInLayer + 1;
        gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = oldOrderInLayer + 1;
        gameObject.transform.localScale = new Vector2(1f, 1f);
        Destroy(clone);
    }
    protected void TrouverAdvairsaire()
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
}