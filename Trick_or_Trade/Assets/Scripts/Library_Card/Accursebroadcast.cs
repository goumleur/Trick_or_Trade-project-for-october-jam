using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.EventSystems;


public class AccurseBroadcast : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Accurse broadcast";
        description_Carte = "When a candy card is stolen or put into discard, activate this to destroy it.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
        afficher_carte();
        typeCard = "Trick";
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

    public void SetVoler()
    {
        Debug.Log("Le joueur s'apprente a volé une carte");
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaVoler = true;
    }
    public void VaDetruire()
    {
        Debug.Log("Le joueur s'apprente a volé une carte");
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        click();
    }
    public override void EffetCarte()
    {
        if (discarded == false && discardPile.transform.childCount > 0)
        {
            discardPile.GetComponent<DiscardsPiles>().GetDicardCard();
            discard();
        }
    }
}

