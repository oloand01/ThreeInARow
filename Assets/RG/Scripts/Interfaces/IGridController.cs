using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IGridController
{
    IEnumerator CreateBoardRoutine { get; set; }
    void ResizeItemsToFitScreen(GridLayoutGroup gridLayout, RectTransform gridContainer);
    bool GridInteractable { get; set; }
    float TileFallSpeed { get; }
    void ResetBoard();
}
