using UnityEngine;
using UnityEngine.EventSystems;

public class HastySearch : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "HastySearch";
        description_Carte = "Draw 2 card then discard 2 card. You may play an additional action card this turn.";
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
        if(discarded == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false)
        {
            discard();
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire = true;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().objetUtiliser = gameObject;

            for (int i = 0; i < 2; i++)
            {
                main.GetComponent<Mains>().PigerUneCarte();
            }
        }
    }
}
