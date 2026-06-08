using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{
    [SerializeField] private BaseUISequenceView LoadingView;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadingView.OpenPanel();
        }
    }
}
