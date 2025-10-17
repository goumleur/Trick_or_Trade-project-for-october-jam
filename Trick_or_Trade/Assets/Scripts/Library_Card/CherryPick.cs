using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CherryPick : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Cherry Pick";
        description_Carte = "Discard a candy card. Choose any card in your deck to add to your hand.";
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
        if (advairsaire == "IAHand" && main.transform.childCount > 2)
        {
            discard();
            MainVirtuelle(null);
            GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser = nom_Carte;
            
        }
        else if(advairsaire == "main")
        {
            for(int i = 0; i < 3; i++)
            {
                main.GetComponent<Mains>().PigerUneCarte();
                if(i < 2)
                {
                    main.transform.GetChild(i).GetComponent<GenerationCarte>().discard();
                }
            }
            discardPile.GetComponent<DiscardsPiles>().discardCard(gameObject);
        }
        
    }

    
    
}
