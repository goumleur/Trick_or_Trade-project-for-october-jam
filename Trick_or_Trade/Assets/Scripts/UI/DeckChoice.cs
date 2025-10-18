using System.Collections.Generic;

// Simple static holder to pass the selected deck between scenes.
public static class DeckChoice
{
    // If null or empty, the game scene should fall back to default initialization.
    public static List<string> SelectedNames = null;
}
