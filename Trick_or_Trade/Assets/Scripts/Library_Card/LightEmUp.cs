using UnityEngine;
using UnityEngine.EventSystems;

public class LightEmUp : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Light 'em up";
        description_Carte = "Destroy a random card in your opponent's hand.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
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
        // Effect: "Destroy a random card in your opponent's hand." (choose random child of IAHand)
        if (discarded == false)
        {
            if (mainAdvairsaire != null && mainAdvairsaire.transform.childCount > 0)
            {
                int count = mainAdvairsaire.transform.childCount;
                int idx = Random.Range(0, count);
                var target = mainAdvairsaire.transform.GetChild(idx).gameObject;
                if (target != null)
                {
                    var ai = mainAdvairsaire.GetComponent<MainAi>();
                    if (ai != null) DetruireCarte(target);
                    else Destroy(target);
                }
            }

            // discard this card after effect
            discard();
        }
    }
}


