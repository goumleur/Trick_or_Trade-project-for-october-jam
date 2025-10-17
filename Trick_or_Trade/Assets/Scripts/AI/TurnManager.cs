using System.Collections;
using UnityEngine;

// Simple TurnManager that enforces a per-turn play budget and alternates between Player and AI.
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Tooltip("Number of cards playable per turn for each side")]
    public int playsPerTurn = 3;

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
        IsPlayerTurn = true;
        playsLeft = playsPerTurn;
        Debug.Log("TurnManager: Player turn started. Plays left = " + playsLeft);
    }

    public void StartAITurn()
    {
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
        }
    }
}
