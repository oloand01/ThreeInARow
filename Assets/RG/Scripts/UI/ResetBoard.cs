using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ResetBoard : MonoBehaviour
{
	private IGridController gridController;

	[Inject]
	public void Init(IGridController gridController)
	{
		this.gridController = gridController;
	}

	public void Reset()
	{
		gridController.ResetBoard();
	}
}
