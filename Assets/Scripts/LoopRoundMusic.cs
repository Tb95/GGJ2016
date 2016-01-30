using UnityEngine;

public class LoopRoundMusic : MonoBehaviour
{
    public AudioClip startClip;
    public AudioClip loopClip;

    bool isLooping;
    AudioSource audioSource;
    float changeTime;

    void Start()
    {
        isLooping = false;
        audioSource = GetComponent<AudioSource>();
        changeTime = Time.realtimeSinceStartup + startClip.length;
    }

    void Update()
    {
        if (!isLooping && Time.realtimeSinceStartup >= changeTime)
        {
            audioSource.clip = loopClip;
            audioSource.Play();
            audioSource.loop = true;
            isLooping = true;
        }
    }
}
