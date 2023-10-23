using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Canvas mainMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        UpdateView();
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
    public void UpdateView()
    {
        if (mainMenu == null) return;
        mainMenuCanvas.enabled = mainMenu.IsVisible;
    }
}
