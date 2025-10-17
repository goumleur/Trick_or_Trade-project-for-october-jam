using UnityEngine;
using UnityEngine.EventSystems;

public class Witchcraft : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Witchcraft";
        description_Carte = "Look at the top 3 card of your deck. Destroy one, discard one, and put one in your hand.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
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
        if (advairsaire == "IAHand" && main.transform.childCount > 2)
        {
            discard();
            MainVirtuelle(null);
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
            
        }
        else if(advairsaire == "main")
        {
            discardPile.GetComponent<DiscardsPiles>().discardCard(gameObject);
            for(int i = 0; i < 3; i++)
            {
                main.GetComponent<Mains>().PigerUneCarte();
                if (i == 1)
                {
                    main.transform.GetChild(i).GetComponent<GenerationCarte>().discard();
                }
                else if(i == 2)
                {
                    DetruireCarte(main.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}
