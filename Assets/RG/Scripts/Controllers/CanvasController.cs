using System.Collections;
using UnityEngine;

public class CanvasController:MonoBehaviour,ICanvasController
{
    private CanvasGroup canvasGroup;
    private void SetCanvasInteractible(bool interactible) => canvasGroup.interactable = interactible;
    public void StartGameWasClicked() => SetCanvasInteractible(false);

    void Awake() => canvasGroup = GetComponent<CanvasGroup>();

    IEnumerator Start()
    {
        SetCanvasInteractible(false);
        yield return new WaitForSeconds(2.0f);
        SetCanvasInteractible(true);
    }
}
