using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TitelObject : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private AudioClip[] audioClip;
    // Start is called before the first frame update
    void Start()
    {
        //イベントトリガー
        var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
        // PointerDownを
        eventTrigger.OnPointerDownAsObservable()
            .Subscribe(_ => PlayAudioClip());
        GameObject objectSoundManager = CheckOtherSoundManager();
        soundManager = objectSoundManager.GetComponent<SoundManager>();
    }

    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    [ContextMenu("DebugCatVoice")]
    private void PlayAudioClip()
    {
        int num = Random.Range(0, audioClip.Length);
        soundManager.PlaySe(audioClip[num]);
    }
}
