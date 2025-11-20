using UnityEngine;

public class WheelEffects : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tickSound;
    [SerializeField] private AudioClip winSound;

    public void PlayTick()
    {
        if (tickSound != null)
            audioSource.PlayOneShot(tickSound);
    }

    public void PlayWin()
    {
        if (winSound != null)
            audioSource.PlayOneShot(winSound);
    }
}
