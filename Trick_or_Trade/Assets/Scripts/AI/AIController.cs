using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple AI controller (MVP)
// - StartAITurn(): disables player input (by setting a flag on Memoire), tries to play one non-interactive card from IAHand.
// - If no suitable card, the AI will draw one card instead.
// - Plays are executed by calling the card's EffetCarte() directly and waiting short delays.
public class AIController : MonoBehaviour
{
    [Tooltip("How many cards the AI attempts to play in one turn. TurnManager controls this value at runtime.")]
    public int playsPerTurn = 3;

    // Names of cards that require additional player interaction. The AI will avoid them for now.
    // Add more names here if you know a card requires multi-step choices.
    readonly HashSet<string> interactiveCardNames = new HashSet<string>() {
        "PressureTrade", "BlindTrade", "prop Bow", "PropBow", "Questionable technique",
        "TradeForQuality", "TradeForNumber", "HastySearch", "ZombieHand", "prop Bow"
    };

    // Public entry point to start the AI turn
    public void StartAITurn()
    {
        StartCoroutine(AITurnRoutine());
    }

    IEnumerator AITurnRoutine()
    {
        // Signal: disable player interactions that rely on Memoire (best-effort)
        var mem = GameObject.Find("Memoire")?.GetComponent<MemoireDesCartes>();
        if (mem != null) mem.boutonUse = false;

        // Small delay to make behavior visible
        yield return new WaitForSeconds(0.4f);

    int plays = 0;
    for (; plays < playsPerTurn; plays++)
        {
            var iaHand = GameObject.Find("IAHand");
            if (iaHand == null || iaHand.transform.childCount == 0)
            {
                // Nothing to play: draw one card and end
                var ai = GameObject.Find("IAHand")?.GetComponent<MainAi>();
                if (ai != null)
                {
                    ai.PigerUneCarte();
                }
                break;
            }

            // Build list of playable (non-interactive) cards
            var playable = new List<GameObject>();
            for (int i = 0; i < iaHand.transform.childCount; i++)
            {
                var card = iaHand.transform.GetChild(i).gameObject;
                var gen = card.GetComponent<GenerationCarte>();
                if (gen == null) continue;
                var name = gen.nom_Carte ?? card.name;
                if (!interactiveCardNames.Contains(name)) playable.Add(card);
            }

            GameObject toPlay = null;
            if (playable.Count > 0)
            {
                // Simple heuristic: prefer highest index (closer to top) or random
                toPlay = playable[Random.Range(0, playable.Count)];
            }
            else
            {
                // No safe playable card: draw and stop
                var ai = GameObject.Find("IAHand")?.GetComponent<MainAi>();
                if (ai != null) ai.PigerUneCarte();
                break;
            }

            // Play the card by invoking its effect directly
            var genCard = toPlay.GetComponent<GenerationCarte>();
            if (genCard != null)
            {
                // Some cards expect GameObject.Find("Memoire") settings; ensure defaults
                if (mem != null)
                {
                    mem.objetUtiliser = genCard.gameObject;
                    mem.nomCarteUtiliser = genCard.nom_Carte;
                    mem.vaVoler = true;
                    mem.vaDetruire = true;
                }

                // Call the effect. Many card implementations call discard() themselves.
                genCard.EffetCarte();

                // Wait a short moment for game objects to reparent/discard logic to run
                yield return new WaitForSeconds(0.35f);
                // Notify TurnManager that AI played a card
                TurnManager.Instance?.OnCardPlayed("IAHand");
            }
        }

        // End of AI turn: give control back to the player.
        if (mem != null)
        {
            mem.boutonUse = false;
            mem.protect = false;
            mem.objetUtiliser = null;
            mem.nomCarteUtiliser = null;
            mem.vaVoler = false;
            mem.vaDetruire = false;
            mem.objetVoler = null;
            mem.multipleDiscard = 0;
        }

        // Reorganize both hands to update their visuals
        GameObject.Find("main")?.GetComponent<main_joueur>()?.OrganiserLaMain();
        GameObject.Find("IAHand")?.GetComponent<MainAi>()?.OrganiserLaMain();

        yield break;
    }
}
