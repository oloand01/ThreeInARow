using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StartGameButtonController : MonoBehaviour
{
    private ICanvasController canvasController;

    [Inject]
    public void Init(ICanvasController canvasController) => this.canvasController = canvasController;

    public void StartButtonClicked()
    {
        canvasController.StartGameWasClicked();
        SceneManager.LoadScene("PlayScene");
    }
}
