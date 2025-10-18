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
            GameObject model = GameObject.Find(name);
            if (model == null)
            {
                model = GameObject.Find("Candy");
                if (model == null)
                {
                    Debug.LogError("DeckInitializer: Neither '" + name + "' nor 'Candy' prefab found in scene. Skipping.");
                    continue;
                }
                else
                {
                    Debug.LogWarning($"DeckInitializer: Prefab '{name}' not found, using 'Candy' fallback.");
                }
            }

            var clone = Object.Instantiate(model, deck.transform, true);
            clone.name = model.name;
            clone.tag = "Untagged";
        }

        Debug.Log($"DeckInitializer: Populated '{deckObjectName}' with {deck.transform.childCount} cards.");
    }
}
