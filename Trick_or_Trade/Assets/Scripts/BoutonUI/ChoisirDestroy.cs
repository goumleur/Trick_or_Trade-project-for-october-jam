using UnityEngine;

public class ChoisirDestroy : MonoBehaviour
{
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DesactiverInteractionBouton();
    }

    public void ActiverInteractionBouton()
    {
        CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = 1f; // invisible
        cg.interactable = true; // désactive les clics
        cg.blocksRaycasts = true; // empêche les raycasts

    }
    
    public void DesactiverInteractionBouton()
    {
        CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = 0f; // invisible
        cg.interactable = false; // désactive les clics
        cg.blocksRaycasts = false; // empêche les raycasts

        
    }
    public void Choisi()
    {
        GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().choixEntreDiscardOuDestroy = "Destroy";
    }
}
