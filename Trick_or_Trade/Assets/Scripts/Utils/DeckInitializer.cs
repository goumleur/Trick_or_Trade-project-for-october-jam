using System.Collections.Generic;
using UnityEngine;

public static class DeckInitializer
{
    // Create card instances in the given deck GameObject from a list of prefab names.
    // If a prefab name isn't found via GameObject.Find, falls back to the 'Candy' prefab.
    public static void PopulateDeckFromNames(List<string> prefabNames, string deckObjectName)
    {
        var deck = GameObject.Find(deckObjectName);
        if (deck == null)
        {
            Debug.LogWarning($"DeckInitializer: Could not find deck object '{deckObjectName}'.");
            return;
        }

        // Clear existing children for a fresh start
        for (int i = deck.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(deck.transform.GetChild(i).gameObject);
        }

        foreach (var name in prefabNames)
        {
            GameObject model = null;

            // Try loading from Resources first (if you keep prefabs in a Resources folder).
            try
            {
                model = Resources.Load<GameObject>(name);
                if (model != null)
                {
                    var clone = Object.Instantiate(model, deck.transform, true);
                    clone.name = name;
                    clone.tag = "Untagged";
                    continue;
                }
            }
            catch {
                // ignore Resources load errors
            }

            // Fallback: look for a scene object with that name (existing card model in scene)
            model = GameObject.Find(name);
            if (model == null)
            {
                // Final fallback: try to find a Candy model
                model = Resources.Load<GameObject>("Candy") ?? GameObject.Find("Candy");
                if (model == null)
                {
                    Debug.LogError("DeckInitializer: Neither '" + name + "' nor 'Candy' prefab found in Resources or scene. Skipping.");
                    continue;
                }
                else
                {
                    Debug.LogWarning($"DeckInitializer: Prefab '{name}' not found, using 'Candy' fallback.");
                }
            }

            var clone2 = Object.Instantiate(model, deck.transform, true);
            clone2.name = model.name;
            clone2.tag = "Untagged";
        }

        Debug.Log($"DeckInitializer: Populated '{deckObjectName}' with {deck.transform.childCount} cards.");

        // If this is the player's deck, update the main_joueur.cartesDisponibles list so drawing uses the newly created cards.
        if (deckObjectName == "Deck")
        {
            var mainCam = GameObject.Find("Main Camera");
            if (mainCam != null)
            {
                var mj = mainCam.GetComponent<main_joueur>();
                if (mj != null)
                {
                    mj.cartesDisponibles.Clear();
                    for (int i = 0; i < deck.transform.childCount; i++)
                    {
                        mj.cartesDisponibles.Add(deck.transform.GetChild(i).gameObject);
                    }
                    Debug.Log("DeckInitializer: main_joueur.cartesDisponibles updated.");
                }
            }
        }
        else if (deckObjectName == "DeckIA")
        {
            var iaHand = GameObject.Find("IAHand");
            if (iaHand != null)
            {
                var mai = iaHand.GetComponent<MainAi>();
                if (mai != null)
                {
                    mai.cartesDisponibles.Clear();
                    for (int i = 0; i < deck.transform.childCount; i++)
                    {
                        mai.cartesDisponibles.Add(deck.transform.GetChild(i).gameObject);
                    }
                    Debug.Log("DeckInitializer: MainAi.cartesDisponibles updated.");
                }
            }
        }
    }
}
