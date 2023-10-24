/*  Script Name:    OptionsMenu.cs
 *  Author:         Marcus Ngooi
 *  Creation Date:  October 23, 2023
 *  Modified Date:  October 24, 2023
 *  Description:    Manages the state of the Options Menu. Will notify
 *                  the presenter when its state has changed.
 */

using System;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public event Action VisibilityChanged;
    [SerializeField] private bool isVisible;
    public bool IsVisible { get => isVisible; set => isVisible = value; }

    public void UpdateVisibility()
    {
        VisibilityChanged?.Invoke();
    }
}
