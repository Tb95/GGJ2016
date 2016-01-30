using UnityEngine;

public class LoopRoundMusic : MonoBehaviour
{
    public AudioClip startClip;
    public AudioClip loopClip;

    bool isLooping;
    AudioSource audioSource;

    void Start()
    {
        isLooping = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isLooping && !audioSource.isPlaying)
        {
            audioSource.clip = loopClip;
            audioSource.Play();
            isLooping = true;
        }
    }
}
