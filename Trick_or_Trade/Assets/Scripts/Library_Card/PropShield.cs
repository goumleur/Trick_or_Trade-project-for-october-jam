using UnityEngine;
using UnityEngine.EventSystems;

public class PropShield : GenerationCarte, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool vaVoler = false;
    public bool vaDetruire = false;
    void Start()
    {
        CreerLaCarte();
    }
    override public void CreerLaCarte()
    {
        nom_Carte = "Prop Shield";
        description_Carte = "When your opponent attempts to steal cards from you, you may activate this to counter that effect.";
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

    public void OnPointerDown(PointerEventData eventData)
    {
        click();
    }

    public override void EffetCarte()
    {

    }
}
