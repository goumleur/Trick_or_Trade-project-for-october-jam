using UnityEngine;
using System.Collections.Generic; // pour utiliser List<>
using TMPro;

public class main_joueur : MonoBehaviour
{
    private List<int> cartesDisponibles = new List<int>(); // Le "deck" complet (1 à 40)
    private List<int> cartesMain = new List<int>();        // Les cartes actuellement en main

    void Start()
    {
        InitialiserDeck();
        PigerMainDeDepart(); // Le joueur pioche 8 cartes au début
    }

    //  Remplit le deck avec toutes les cartes possibles
    void InitialiserDeck()
    {
        cartesDisponibles.Clear();
        for (int i = 1; i <= 40; i++)
        {
            cartesDisponibles.Add(i);
        }
        Debug.Log("Deck initialisé avec " + cartesDisponibles.Count + " cartes.");
    }

    //  Pioche 8 cartes différentes au début du round
    public void PigerMainDeDepart()
    {
        cartesMain.Clear();

        for (int i = 0; i < 8; i++)
        {
            if (cartesDisponibles.Count == 0)
            {
                Debug.LogWarning("Le deck est vide !");
                return;
            }

            int indexAleatoire = Random.Range(0, cartesDisponibles.Count);
            int carteTiree = cartesDisponibles[indexAleatoire];
            cartesDisponibles.RemoveAt(indexAleatoire);
            cartesMain.Add(carteTiree);

            AfficherCarte(carteTiree);
        }

        Debug.Log("Main de départ tirée (" + cartesMain.Count + " cartes)");
    }

    //  Pioche une seule carte pendant le jeu
    public void PigerUneCarte()
    {
        if (cartesDisponibles.Count == 0)
        {
            InitialiserDeck();
        }

        int indexAleatoire = Random.Range(0, cartesDisponibles.Count);
        int carteTiree = cartesDisponibles[indexAleatoire];
        cartesDisponibles.RemoveAt(indexAleatoire);
        cartesMain.Add(carteTiree);

        Debug.Log("Carte supplémentaire tirée : " + carteTiree);
        AfficherCarte(carteTiree);
    }

    //  Fonction qui affiche ou agit selon la carte tirée
    private void AfficherCarte(int numeroCarte)
    {

        switch (numeroCarte) // Switch statement to handle different card draws
        {
            case 1:
                Debug.Log("Recycle was drawn");
                break;
            case 2:
                Debug.Log("Card 2 drawn");
                break;
            case 3:
                Debug.Log("Card 3 drawn");
                break;
            case 4:
                Debug.Log("Card 4 drawn");
                break;
            case 5:
                Debug.Log("Card 5 drawn");
                break;
            case 6:
                Debug.Log("Card 6 drawn");
                break;
            case 7:
                Debug.Log("Card 7 drawn");
                break;
            case 8:
                Debug.Log("Card 8 drawn");
                break;
            case 9:
                Debug.Log("Card 9 drawn");
                break;
            case 10:
                Debug.Log("Card 10 drawn");
                break;
            case 11:
                Debug.Log("Card 11 drawn");
                break;
            case 12:
                Debug.Log("Card 12 drawn");
                break;
            case 13:
                Debug.Log("Card 13 drawn");
                break;
            case 14:
                Debug.Log("Card 14 drawn");
                break;
            case 15:
                Debug.Log("Card 15 drawn");
                break;
            case 16:
                Debug.Log("Card 16 drawn");
                break;
            case 17:
                Debug.Log("Card 17 drawn");
                break;
            case 18:
                Debug.Log("Card 18 drawn");
                break;
            case 19:
                Debug.Log("Card 19 drawn");
                break;
            case 20:
                Debug.Log("Card 20 drawn");
                break;
            case 21:
                Debug.Log("Card 21 drawn");
                break;
            case 22:
                Debug.Log("Card 22 drawn");
                break;
            case 23:
                Debug.Log("Card 23 drawn");
                break;
            case 24:
                Debug.Log("Card 24 drawn");
                break;
            case 25:
                Debug.Log("Card 25 drawn");
                break;
            case 26:
                Debug.Log("Card 26 drawn");
                break;
            case 27:
                Debug.Log("Card 27 drawn");
                break;
            case 28:
                Debug.Log("Card 28 drawn");
                break;
            case 29:
                Debug.Log("Card 29 drawn");
                break;
            case 30:
                Debug.Log("Card 30 drawn");
                break;
            case 31:
                Debug.Log("Card 31 drawn");
                break;
            case 32:
                Debug.Log("Card 32 drawn");
                break;
            case 33:
                Debug.Log("Card 33 drawn");
                break;
            case 34:
                Debug.Log("Card 34 drawn");
                break;
            case 35:
                Debug.Log("Card 35 drawn");
                break;
            case 36:
                Debug.Log("Card 36 drawn");
                break;
            case 37:
                Debug.Log("Card 37 drawn");
                break;
            case 38:
                Debug.Log("Card 38 drawn");
                break;
            case 39:
                Debug.Log("Card 39 drawn");
                break;
            case 40:
                Debug.Log("Card 40 drawn");
                break;
        }
    }
}