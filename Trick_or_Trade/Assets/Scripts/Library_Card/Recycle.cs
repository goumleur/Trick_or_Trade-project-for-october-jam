using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;



public class CarteDeJeu : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
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
        nom_Carte = "Recycle";
        description_Carte = "Put a card from any discard pile into your hand.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
        afficher_carte();
        if(gameObject.name != "CarteModel")
        {
            Invoke("TrouverAdvairsaire",0.001f);
        }
    }

    public void discard()
    {
        discarded = true;
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().discardCard(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameObject.transform.parent.name == "main" || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaDetruire == true ||  gameObject.transform.parent.name == "Deck")
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
        Debug.Log("Le joueur s'apprente a volé une carte");
        GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler = true;
    }
    public void VaDetruire()
    {
        Debug.Log("Le joueur s'apprente a volé une carte");
        GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaDetruire = true;
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
        else if (GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true)
        {
            VolerCarteAdvairsaire(gameObject);
            GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler = false;
        }
        else if (GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaDetruire == true)
        {
            DetruireCarteAdvairsaire(gameObject);
            GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaDetruire = false;
        }
        else if (discarded == false)
        {
            //discard();
        }
    }
}
