using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour,ILevelManager
{
    public Level[] Levels;
    public Level CurrentLevel { get { return Levels[0]; } }
    
    void Awake()
    {
        Assert.IsTrue(Levels.Length > 0,"LevelManager must contain levels.");
        for (int i = 0; i < Levels.Length; i++)
        {
            Assert.IsNotNull(Levels[i], "Level is null");
        }

        for (int i = 0; i < Levels.Length; i++)
        {
            Assert.IsNotNull(Levels[i].BoardGrid, "Grid cant be null in level");
            Assert.IsTrue(Levels[i].Tiles.Length >= 3, "We have to use three or more types of tile in level.");
            Assert.IsTrue(Levels[i].BoardGrid.Columns!=0, "Grid columns cant be 0 in level");
            Assert.IsTrue(Levels[i].BoardGrid.Rows != 0, "Grid rows cant be 0 in level");
            Assert.IsNotNull(Levels[i].BoardGrid.cell, "Cell cant be null in grid");
        }
    }
}
