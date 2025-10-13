using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;



public class CarteDeJeu : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool vaVoler = false;
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
            Debug.Log(advairsaire);
        }
    }

    public void discard()
    {
        discardCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameObject.transform.parent.name == "main" || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true)
        {
            Debug.Log("Souris sur la carte !");
            AfficherCarteZoom();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.transform.parent.name == "main" || GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true)
        {
            Debug.Log("Souris quitte la carte !");
            EnleverCarteZoom();
        }
    }
    
    public void SetVoler()
    {
        Debug.Log("Le joueur s'apprente a volé une carte");
        new WaitForSeconds(1f);
        GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler = true;
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler == true)
        {
            VolerCarteAdvairsaire(gameObject);
            GameObject.Find("CarteModel").GetComponent<CarteDeJeu>().vaVoler = false;
        }
    }
}
