using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>

public class DiscardPile : MonoBehaviour
{
    Vector3 oldScale;
    Vector3 oldPosition;
    Vector3 oldAngle;
    GameObject main;
    public GameObject carteASauver;


    public void CarteASauver(GameObject carte)
    {
        carteASauver = carte;
        Invoke("PutDiscardCard", 0.01f);
    }
    public void discardCard(GameObject carte)
    {
        Debug.Log("Début");
        carte.transform.localPosition = new Vector3(0f, 0f, 0f);
        carte.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        carte.transform.localScale = new Vector3(1f, 1f, 1f);
        carte.transform.SetParent(GameObject.Find("DiscardPile").transform, false);
        GameObject.Find("Main Camera").GetComponent<main_joueur>().DiscardFait(carte);
    }


    public void GetDicardCard()
    {
        main = GameObject.Find("main");
        List<Transform> enfants = new List<Transform>();
        Transform discard = GameObject.Find("DiscardPile").transform;

        oldScale = discard.localScale;
        oldPosition = discard.localPosition;
        oldAngle = discard.localEulerAngles;
        
        discard.transform.SetPositionAndRotation(main.transform.position, Quaternion.Euler(0f, 0f, 0f));
        discard.transform.localScale = main.transform.localScale;

        foreach (Transform enfant in discard)
        {
            enfants.Add(enfant);
        }
        main.SetActive(false);
        AfficherDiscardOrDeck(enfants);
    }

    public void PutDiscardCard()
    {
        if (GameObject.Find("Memoire").GetComponent<MemoireDesCartes>().nomCarteUtiliser == "LuckyFind")
        {
            GameObject parent = GameObject.Find("Deck"); // Chercher l'objet main
            carteASauver.transform.SetParent(parent.transform, worldPositionStays: true);
        }
        else
        {
            main.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<main_joueur>().SauverCarte(carteASauver);
            Transform discard = GameObject.Find("DiscardPile").transform;
            discard.transform.SetPositionAndRotation(oldPosition, Quaternion.Euler(oldAngle));
            discard.transform.localScale = oldScale;
            foreach (Transform enfant in discard)
            {
                Transform cFrameCarte = enfant.GetComponent<Transform>();
                GameObject discardPile = GameObject.Find("DiscardPile");
                Transform cFrameDiscardPile = discardPile.GetComponent<Transform>();
                cFrameCarte.transform.SetPositionAndRotation(cFrameDiscardPile.transform.position, Quaternion.Euler(0f, 90f, 0f));
            }
            carteASauver = null;
        }
    }
    public void AfficherDiscardOrDeck(List<Transform> carteAAfficher)
    {

        float angleTotal = 30f;
        float angleParCarte = angleTotal / (carteAAfficher.Count - 1);
        float rayon = 10f;

        for (int i = 0; i < carteAAfficher.Count; i++)
        {
            if (carteAAfficher.Count > 1)
            {
                float angle = -angleTotal / 2 + i * angleParCarte; // Met l'angle de la carte
                float rad = -angle * Mathf.Deg2Rad; // Le fait que les carte sont mis en cercle
                Vector3 pos = new Vector3(Mathf.Sin(rad) * rayon, Mathf.Cos(rad) * rayon, i * 10); // Calculer la position de la carte
                carteAAfficher[i].transform.localPosition = pos; // Mettre la carte en position
                carteAAfficher[i].transform.localRotation = Quaternion.Euler(0f, 0f, angle); // set l'angle de la carte

                // Set l'orderInLayer de la carte
                carteAAfficher[i].transform.GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                carteAAfficher[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                carteAAfficher[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * i;
                carteAAfficher[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 101 - 2 * i;
                carteAAfficher[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 100 - 2 * i;
            }
            else
            {
                carteAAfficher[i].transform.localPosition = new Vector2(0f, 10f); // Mettre la carte en position
                carteAAfficher[i].transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // set l'angle de la carte
            }
        }

    }
}

