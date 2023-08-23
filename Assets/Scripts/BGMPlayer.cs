using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        soundManager.PlayBgm(clip);
    }
}
