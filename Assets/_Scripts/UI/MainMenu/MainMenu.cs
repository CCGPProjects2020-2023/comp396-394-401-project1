using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public event Action VisibilityChanged;
    [SerializeField] private bool isVisible;
    public bool IsVisible { get => isVisible; set => isVisible = value; }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void UpdateVisibility()
    {
        VisibilityChanged?.Invoke();
    }
}
