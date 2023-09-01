using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlinkCanvasGroup : MonoBehaviour
{
    public float DurationSeconds;
    public Ease EaseType;

    //private CanvasGroup canvasGroup;
    private TextMeshProUGUI txtStart;

    // Start is called before the first frame update
    void Start()
    {
        this.txtStart = this.GetComponent<TextMeshProUGUI>();
        this.txtStart.DOFade(0.0f, this.DurationSeconds).SetEase(this.EaseType).SetLoops(-1, LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
