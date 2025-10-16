using UnityEngine;
using UnityEngine.EventSystems;

public class PropSword : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool vaVoler = false;
    public bool vaDetruire = false;
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Prop Sword";
        description_Carte = "Your opponent discards two cards.";
        afficher_carte();
        typeCard = "Action";
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

    public void OnPointerDown(PointerEventData eventData)
    {
        click();
    }

    public override void EffetCarte()
    {
        if(discarded == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            Debug.Log("Je marche");
            discard();
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = gameObject;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
        }
    }
}
