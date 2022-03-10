using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to store all of the variables that the Player will need but also accessible to any class
public class gVar
{
    public static int batteryPercentage = 100; // The Player's battery percentage
    public static bool connectedToAnything = false; // Is the Player battery connected to any of the devices
    public static bool backToMenuUI = false; // Has the Player clicked the button to return to the menu
}
