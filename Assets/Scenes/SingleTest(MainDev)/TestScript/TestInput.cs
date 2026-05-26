using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{
    [SerializeField] private BaseUISequenceView pauseMenu;

    public bool IsPauseMenuOpen { get; private set; } = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPauseMenuOpen = !IsPauseMenuOpen;
        }
        if (IsPauseMenuOpen)
            pauseMenu.OpenPanel();
        else
            pauseMenu.ClosePanel();
    }
}
