using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>

public class deck_joueur : MonoBehaviour
{


    GameObject main;
    GameObject carteAChercher;
    Vector3 oldScale;
    Vector3 oldPosition;
    Vector3 oldAngle;
    public void test()
    {
        VoirCarteDessus(5);
    }
    public void melanger_deck()
    {
        // Stocker les enfants dans une liste
        List<Transform> enfants = new List<Transform>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            enfants.Add(gameObject.transform.GetChild(i));
        }

        // Mélanger la liste
        for (int i = 0; i < enfants.Count; i++)
        {
            Transform temp = enfants[i];
            int randomIndex = Random.Range(i, enfants.Count);
            enfants[i] = enfants[randomIndex];
            enfants[randomIndex] = temp;
        }
        // Réappliquer l’ordre dans la hiérarchie
        for (int i = 0; i < enfants.Count; i++)
        {
            enfants[i].SetSiblingIndex(i);
        }

        Debug.Log("Deck Mélanger");
    }

    public void ChercherCarte(GameObject carte)
    {
        carteAChercher = carte;
        Invoke("PutDiscardCard", 0.01f);

    }
    public void GetDeckCard()
    {
        main = GameObject.Find("main");
        List<Transform> enfants = new List<Transform>();

        oldScale = gameObject.transform.localScale;
        oldPosition = gameObject.transform.localPosition;
        oldAngle = gameObject.transform.localEulerAngles;

        gameObject.transform.SetPositionAndRotation(main.transform.position, Quaternion.Euler(0f, 0f, 0f));
        gameObject.transform.localScale = main.transform.localScale;

        foreach (Transform enfant in gameObject.transform)
        {
            enfants.Add(enfant);
            enfant.localScale = new Vector3(1f, 1f, 1f);
        }
        main.SetActive(false);
        AfficherDiscardOrDeck(enfants);
    }

    public void PutDiscardCard()
    {
        main.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<main_joueur>().SauverCarte(carteAChercher);
        Transform discard = GameObject.Find("Deck").transform;
        discard.transform.SetPositionAndRotation(oldPosition, Quaternion.Euler(oldAngle));
        discard.transform.localScale = oldScale;
        foreach (Transform enfant in discard)
        {
            Transform cFrameCarte = enfant.GetComponent<Transform>();
            GameObject discardPile = GameObject.Find("Deck");
            Transform cFrameDiscardPile = discardPile.GetComponent<Transform>();
            cFrameCarte.transform.SetPositionAndRotation(cFrameDiscardPile.transform.position, Quaternion.Euler(0f, 90f, 0f));
        }
        carteAChercher = null;

    }

    public void VoirCarteDessus(int nombreCarteAVoir)
    {
        main = GameObject.Find("main");
        List<Transform> carteAVoir = new List<Transform>();

        oldScale = gameObject.transform.localScale;
        oldPosition = gameObject.transform.localPosition;
        oldAngle = gameObject.transform.localEulerAngles;

        gameObject.transform.SetPositionAndRotation(main.transform.position, Quaternion.Euler(0f, 0f, 0f));
        gameObject.transform.localScale = main.transform.localScale;

        for (int i = 0; i < nombreCarteAVoir; i++)
        {
            carteAVoir.Add(gameObject.transform.GetChild(i));
            gameObject.transform.GetChild(i).localScale = new Vector3(1f, 1f, 1f);
        }
        main.SetActive(false);
        AfficherDiscardOrDeck(carteAVoir);
        GameObject.Find("Canvas").transform.GetChild(1).GetComponent<BoutonDeposerCarteDessusDeck>().ActiverInteractionBouton();
    }
    public void PutCarteAVoir()
    {
        main.SetActive(true);
        Transform discard = GameObject.Find("Deck").transform;
        discard.transform.SetPositionAndRotation(oldPosition, Quaternion.Euler(oldAngle));
        discard.transform.localScale = oldScale;
        foreach (Transform enfant in discard)
        {
            Transform cFrameCarte = enfant.GetComponent<Transform>();
            GameObject discardPile = GameObject.Find("Deck");
            Transform cFrameDiscardPile = discardPile.GetComponent<Transform>();
            cFrameCarte.transform.SetPositionAndRotation(cFrameDiscardPile.transform.position, Quaternion.Euler(0f, 90f, 0f));
        }
        GameObject.Find("Canvas").transform.GetChild(1).GetComponent<BoutonDeposerCarteDessusDeck>().DesactiverInteractionBouton();
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
