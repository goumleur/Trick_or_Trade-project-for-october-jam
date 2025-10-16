using UnityEngine;

public class Decks : MonoBehaviour
{
    virtual public void melanger_deck() { }
    virtual public void ChercherCarte(GameObject carte) { }
    virtual public void GetDeckCard() { }
    virtual public void PutDiscardCard() { }
    virtual public void VoirCarteDessus(int nombreCarteAVoir) { }
    virtual public void PutCarteAVoir() {}
}
