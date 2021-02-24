using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class TileController : MonoBehaviour
{
	private Button button;
	private IGridController gridController;
    private IObjectPoolManager objectPoolManager;
    private IGameLoopHandler gameLoopHandler;
    public int x;
	public int y;
	[HideInInspector] public RectTransform tileControllerRect;
	[HideInInspector] public TileImageController childImage;

	[Inject]
	public void Init(IGameLoopHandler gameLoopHandler,IGridController gridController, IObjectPoolManager objectPoolManager)
	{
		this.gridController = gridController;
		this.objectPoolManager = objectPoolManager;
		this.gameLoopHandler = gameLoopHandler;
	}

	void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(TileWasPressed);
		tileControllerRect = GetComponent<RectTransform>();

		Assert.IsTrue(button.interactable,"button must be interactable");
	}

	void OnDestroy() => button.onClick.RemoveListener(TileWasPressed);

	void TileWasPressed()
	{
		if (childImage == null)
			return;
		gridController.GridInteractable = false;
		objectPoolManager.PoolTileImageController(childImage);
		childImage = null;
		StartCoroutine(gameLoopHandler.GameLoopRoutine);
	}
}
