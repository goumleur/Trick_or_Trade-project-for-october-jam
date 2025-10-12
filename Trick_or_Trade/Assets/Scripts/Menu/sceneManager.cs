using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void changescene(string Jeu_de_Carte)
    {
        SceneManager.LoadScene(Jeu_de_Carte);

    }
    public void setting(string MenuSetting)
    {
        SceneManager.LoadScene(MenuSetting);
    }
}
