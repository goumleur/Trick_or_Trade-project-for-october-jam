using UnityEngine;
using UnityEngine.EventSystems;

public class Candy : GenerationCarte, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Candy";
        description_Carte = "";
        afficher_carte();
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
}
