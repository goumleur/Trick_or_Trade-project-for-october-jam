using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Attach this to a UI manager object. Wire the three public methods to three UI Buttons.
public class DeckSelector : MonoBehaviour
{
    // Three presets of card names (will be repeated to reach 60)
    readonly List<string> presetA = new List<string>() {
        "Candy","ToxicSpike","PropShield","PropBow","LuckyFind","FreshStart","Fantastical_Reclaimation","Blathan_theft","Incinerate","LightEmUp",
        "ScrabbledNote","FaithlessAnnihilation","Blade_Of_Nightmare","TheOlSwitcheroo","WatchTheWorldBurn","GenerousDonation","SpiderFromNowhere","Spill","Recycle","CherryPick"
    };

    readonly List<string> presetB = new List<string>() {
        "Candy","LuckyFind","HastySearch","SearchTheScraps","Recycle","Spill","PropShield","PropBow","PressureTrade","BlindTrade",
        "TradeForNumber","TradeForQuality","QuestionableTechnique","FreshStart","ScrabbledNote","LightEmUp","Incinerate","Blathan_theft","SpiderFromNowhere","CherryPick"
    };

    readonly List<string> presetC = new List<string>() {
        "Candy","ToxicSpike","SpiderFromNowhere","WatchTheWorldBurn","FaithlessAnnihilation","Blade_Of_Nightmare","TheOlSwitcheroo","GenerousDonation","Blathan_theft","Incinerate",
        "LightEmUp","ScrabbledNote","Recycle","Spill","LuckyFind","PressureTrade","BlindTrade","FreshStart","SearchTheScraps","CherryPick"
    };

    // Populate Deck with 60 cards chosen from preset (repeating if necessary)
    void PopulateDeck(List<string> preset)
    {
        var deck = GameObject.Find("Deck");
        if (deck == null)
        {
            Debug.LogError("Deck GameObject not found in scene. Make sure an object named 'Deck' exists.");
            return;
        }

        // Clear existing deck children
        for (int i = deck.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(deck.transform.GetChild(i).gameObject);
        }

        int needed = 60;
        int idx = 0;
        for (int i = 0; i < needed; i++)
        {
            string name = preset[idx % preset.Count];
            GameObject model = GameObject.Find(name) ?? GameObject.Find("Candy");
            if (model == null)
            {
                Debug.LogWarning($"Neither '{name}' nor 'Candy' prefab found. Skipping slot {i}.");
                idx++;
                continue;
            }
            var clone = Instantiate(model, deck.transform, true);
            clone.name = "Card_" + i;
            clone.tag = "Untagged";
            idx++;
        }

        // Update player's available list if script exists
        var main = GameObject.Find("Main Camera")?.GetComponent<main_joueur>();
        if (main != null)
        {
            main.cartesDisponibles.Clear();
            for (int i = 0; i < deck.transform.childCount; i++) main.cartesDisponibles.Add(deck.transform.GetChild(i).gameObject);
        }

        Debug.Log($"Deck populated with {deck.transform.childCount} cards from preset.");
    }

    // When a deck is chosen from the menu scene, expand it to 60 entries and store in DeckChoice, then load the game scene.
    public void SelectDeck1() { ChooseAndLoad(presetA); }
    public void SelectDeck2() { ChooseAndLoad(presetB); }
    public void SelectDeck3() { ChooseAndLoad(presetC); }

    void ChooseAndLoad(List<string> preset)
    {
        // Expand preset to 60 names
        var expanded = new List<string>(60);
        for (int i = 0; i < 60; i++) expanded.Add(preset[i % preset.Count]);

    // Save to PlayerPrefs as a pipe-separated string so the game scene can read it after loading.
    string csv = string.Join("|", expanded);
    PlayerPrefs.SetString("SelectedDeckCsv", csv);
    PlayerPrefs.Save();

        // Load the game scene where main_joueur will read the saved deck.
        SceneManager.LoadScene("Jeu_de_Carte");
    }

    // Uses public DeckNameList (DeckNameList.cs) for JSON serialization
}

