using UnityEngine;

public class WeaponSound : MonoBehaviour
{
    public AudioClip shootSound;
    public AudioClip impactSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }

    public void PlayImpactSound()
    {
        audioSource.PlayOneShot(impactSound);
    }
}