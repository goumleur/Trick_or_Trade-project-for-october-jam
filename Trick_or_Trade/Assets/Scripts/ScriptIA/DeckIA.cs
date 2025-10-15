using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>

public class DeckIA : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
