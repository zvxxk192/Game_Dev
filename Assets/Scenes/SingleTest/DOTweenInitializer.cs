using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DOTweenInitializer : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(500, 50);
    }

    private void Start()
    {
        ////Move GameObject
        //panel.DOAnchorPos(new Vector2(1920, 0), 5).SetRelative().SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);

        ////Fade Alpha
        //fadeImage.DOFade(0f, 1f).SetUpdate(true).SetLoops(10, LoopType.Yoyo);

        ////Update HP
        //fadeImage.DOFillAmount(0f, 2f);

        //fadeImage.fillAmount = 1f;
        //fadeImage.DOFillAmount(0, 5);

        int _currentHp = 0;
        

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 10; i++)
        {
            _currentHp = Random.Range(0, 1000);
            sequence.Append(fadeImage.DOFillAmount(_currentHp / 1000f, 3));
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
           
    }
}
