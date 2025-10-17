using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class Infest : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Mummify";
        description_Carte = "When a candy card is put into an opponent's discard, activate this to destroy it.";
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
        if (discarded == false && GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().vaDetruire == false && mainAdvairsaire.transform.childCount > 2)
        {
            discard();
            List<Transform> enfants = new List<Transform>();
            for (int y = 0; y < deckAdvairsaire.transform.childCount; y++)
            {
                enfants.Add(deckAdvairsaire.transform.GetChild(y));
            }
            int nbATransformer = 3;
            for (int y = 0; y < deckAdvairsaire.transform.childCount; y++)
            {
                if( nbATransformer == 0)
                {
                    return;
                }
                if(enfants[y].GetComponent<GenerationCarte>().nom_Carte == "Candy")
                {
                    mainAdvairsaire.GetComponent<Mains>().modifierCarte(enfants[y], GameObject.Find("Maggots"));
                    nbATransformer -= 1;
                    Debug.Log(enfants[y].name);
                }
            }
        }
    }
}
