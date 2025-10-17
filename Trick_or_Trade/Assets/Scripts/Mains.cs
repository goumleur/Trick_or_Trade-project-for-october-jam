using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>

public class Mains : MonoBehaviour
{
    
    public List<GameObject> cartesDisponibles = new List<GameObject>(); // Le "deck" complet (1 à 40)
    public List<GameObject> cartesMain = new List<GameObject>(); // Les cartes actuellement en main
    public Transform zoneMain;

    virtual public void modifierCarte(Transform carteAModifier, GameObject carteResultant) { }
    virtual public void PigerUneCarte() { }
    virtual public void DiscardFait(GameObject card) {}
    virtual public void PrendreCarte(GameObject carte) { }
    virtual public void DetruireCarte(GameObject carte) { }
    virtual public void SauverCarte(GameObject cartesASauver) { }
    virtual public void OrganiserLaMain() { }
    virtual public void PigerMainDeDepart() { }
    virtual public void modifierCarteMain(Transform carteAModifier, GameObject carteResultant, GameObject parent) {}
}
