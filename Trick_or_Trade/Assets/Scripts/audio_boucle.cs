using UnityEngine;
using System.Collections;

public class audio_boucle : MonoBehaviour
{
    public AudioSource premierAudio;
    public AudioSource secondAudio;

    void Start()
    {
        StartCoroutine(PlayNextAudio());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator PlayNextAudio()
    {
        premierAudio.Play();
        yield return new WaitWhile(() => premierAudio.isPlaying); // Attend la fin du premier clip
        secondAudio.Play();
    }
}
