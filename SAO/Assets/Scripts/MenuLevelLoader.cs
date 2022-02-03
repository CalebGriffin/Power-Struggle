using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
