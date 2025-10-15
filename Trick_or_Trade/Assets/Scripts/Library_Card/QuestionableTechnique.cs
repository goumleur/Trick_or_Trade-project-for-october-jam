using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;



public class QuestionableTechnique : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool vaVoler = false;
    public bool vaDetruire = false;
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Questionable technique";
        description_Carte = "Discard or destroy a cards from your hand.";
        afficher_carte();
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
        if(discarded == false && gameObject.transform.parent.name != "IAHand" && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            discard();
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = gameObject;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
            GameObject.Find("Canvas").transform.GetChild(1).GetComponent<ChoisirDestroy>().ActiverInteractionBouton();
            GameObject.Find("Canvas").transform.GetChild(2).GetComponent<ChoisirDiscard>().ActiverInteractionBouton();
        }
    }

}
