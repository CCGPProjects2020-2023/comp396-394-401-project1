/*  Script Name:    MainMenu.cs
 *  Author:         Marcus Ngooi
 *  Creation Date:  October 21, 2023
 *  Modified Date:  October 24, 2023
 *  Description:    Manages the state of the Main Menu. Will notify
 *                  the presenter when its state has changed.
 */

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
