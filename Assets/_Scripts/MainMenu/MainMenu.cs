using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public event Action VisibilityChanged;
    [SerializeField] private bool isVisible;
    public bool IsVisible { get => isVisible; set => isVisible = value; }

    public void UpdateVisibility()
    {
        VisibilityChanged?.Invoke();
    }
}
