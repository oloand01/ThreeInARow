using System.Collections;

public interface ICellManager
{
    void PoolAllCells();
    IEnumerator CreateCellsRoutine { get; set; }
}

