using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from the LevelLoader class to hide the transition animation if the player is opening the game
public class MenuLevelLoader : LevelLoader
{
    protected override void OnEnable()
    {
        if (gVar.backToMenuUI)
        {
            base.OnEnable();
        }
        else
        {
            TransitionObj.SetActive(false);
        }
    }
}
