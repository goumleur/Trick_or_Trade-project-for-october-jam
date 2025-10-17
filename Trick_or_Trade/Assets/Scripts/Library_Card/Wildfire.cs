using UnityEngine;
using UnityEngine.EventSystems;

public class Wildfire : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Wildfire";
        description_Carte = "Destroy 2 random non-candy cards in your opponent's deck.";
        backRound = Resources.Load<Sprite>("Assets/Images/CardFrames/CommonEnemyTrick.png");
        illustration = Resources.Load<Sprite>("Assets/Images/CardIcon/ImageRecycle.jpg");
        afficher_carte();
        typeCard = "Action";
        if (gameObject.tag == "Untagged")
        {
            Invoke("TrouverAdvairsaire", 0.001f);
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
        // Effect: "Your opponent discards all trick cards from their hand." (cards tagged "trick")
        if (discarded == false && deckAdvairsaire.transform.childCount > 1)
        {
            discard();
            for (int i = 0; i < 2; i++)
            {
                int nbAleatoire = Random.Range(0, deckAdvairsaire.transform.childCount);
                if(deckAdvairsaire.transform.GetChild(nbAleatoire).GetComponent<GenerationCarte>().nom_Carte == "Candy") continue;
                DetruireCarte(deckAdvairsaire.transform.GetChild(nbAleatoire).gameObject);
            }
        }
    }
}
