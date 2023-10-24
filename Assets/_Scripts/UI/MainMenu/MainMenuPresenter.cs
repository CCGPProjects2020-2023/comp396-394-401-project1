/*  Script Name:    MainMenuPresenter.cs
 *  Author:         Marcus Ngooi
 *  Creation Date:  October 21, 2023
 *  Modified Date:  October 24, 2023
 *  Description:    Receives the user inputs via UI events (e.g., Button click)
 *                  and, in turn manipulates the Main Menu's data (state).
 */

using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private Instructions instructions;

    // Start is called before the first frame update
    void Start()
    {
        ShowMenu();
    }
    public void ShowMenu()
    {
        mainMenu.IsVisible = true;
        UpdateView();
    }
    public void HideMenu()
    {
        mainMenu.IsVisible = false;
        UpdateView();
    }
    public void OnPlayButtonClicked()
    {
        HideMenu();
        instructions.IsVisible = true;
    }
    public void OnOptionsButtonClicked()
    {
        HideMenu();
        optionsMenu.IsVisible = true;
    }
    public void OnQuitButtonClicked()
    {
        mainMenu.Quit();
    }
    public void UpdateView()
    {
        if (mainMenu == null) return;
        mainMenuCanvas.enabled = mainMenu.IsVisible;
    }
}
