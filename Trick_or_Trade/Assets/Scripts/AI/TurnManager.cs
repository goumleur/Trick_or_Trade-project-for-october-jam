using System.Collections;
using UnityEngine;

// Simple TurnManager that enforces a per-turn play budget and alternates between Player and AI.
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Tooltip("Number of cards playable per turn for each side")]
    public int playsPerTurn = 1;

    public bool IsPlayerTurn { get; private set; } = true;
    int playsLeft = 0;

    AIController aiController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // Ensure the object can be found by name (GenerationCarte uses GameObject.Find("TurnManager").SendMessage(...))
        this.gameObject.name = "TurnManager";
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // find AIController if present
        var obj = GameObject.Find("AIController");
        if (obj != null) aiController = obj.GetComponent<AIController>();

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (gameOver) return;
        IsPlayerTurn = true;
        playsLeft = playsPerTurn;
        Debug.Log("TurnManager: Player turn started. Plays left = " + playsLeft);
    }

    public void StartAITurn()
    {
        if (gameOver) return;
        IsPlayerTurn = false;
        playsLeft = playsPerTurn;
        Debug.Log("TurnManager: AI turn started. Plays left = " + playsLeft);
        if (aiController == null)
            //aiController = GameObject.FindObjectOfType<AIController>();
        if (aiController != null)
        {
            aiController.playsPerTurn = playsPerTurn;
            aiController.StartAITurn();
        }
        else
        {
            // No AIController found -> immediately return control to player
            Debug.LogWarning("TurnManager: No AIController found to run AI turn.");
            StartPlayerTurn();
        }
    }

    // Called when a card is played. ownerParentName should be "main" or "IAHand"
    public void OnCardPlayed(string ownerParentName)
    {
        if (gameOver) return;
        if (playsLeft <= 0) return;
        playsLeft--;
        Debug.Log("TurnManager: Card played by " + ownerParentName + ", plays left = " + playsLeft);

        if (playsLeft <= 0)
        {
            // End this side's turn
            if (IsPlayerTurn)
            {
                // Start AI turn
                StartAITurn();
            }
            else
            {
                // AI finished, hand back to player
                StartPlayerTurn();
            }
            // After a turn ends, check victory/defeat conditions
            CheckVictory();
        }
    }

    bool gameOver = false;

    void CheckVictory()
    {
        // Count candies in each side (look in hand and deck)
        int playerCandies = CountCandies("main", "Deck");
        int aiCandies = CountCandies("IAHand", "DeckIA");

        Debug.Log($"TurnManager: Candy counts - Player: {playerCandies}, AI: {aiCandies}");

        // If either side has zero candies, evaluate winner
        if (playerCandies == 0 || aiCandies == 0)
        {
            // Per your rule: if the player has more candies he loses; otherwise AI loses
            bool playerLoses = playerCandies > aiCandies;
            EndGame(!playerLoses);
        }
    }

    int CountCandies(string handName, string deckName)
    {
        int count = 0;
        var hand = GameObject.Find(handName);
        if (hand != null)
        {
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                var c = hand.transform.GetChild(i).gameObject;
                if (IsCandy(c)) count++;
            }
        }
        var deck = GameObject.Find(deckName);
        if (deck != null)
        {
            for (int i = 0; i < deck.transform.childCount; i++)
            {
                var c = deck.transform.GetChild(i).gameObject;
                if (IsCandy(c)) count++;
            }
        }
        return count;
    }

    bool IsCandy(GameObject card)
    {
        if (card == null) return false;
        if (card.name != null && card.name.ToLower().Contains("candy")) return true;
        if (card.tag != null && card.tag.ToLower() == "candy") return true;
        return false;
    }

    void EndGame(bool playerWon)
    {
        gameOver = true;
        Debug.Log("Game Over! " + (playerWon ? "Player wins" : "AI wins"));
        // Disable AIController if present
        if (aiController != null) aiController.enabled = false;
        // Optional: set a flag in MemoireDesCartes to block further interactions
        var mem = GameObject.Find("Memoire")?.GetComponent<MemoireDesCartes>();
        if (mem != null) mem.boutonUse = false;
        // TODO: Add UI to show end game screen
    }
}
