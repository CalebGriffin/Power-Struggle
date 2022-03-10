using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to store all of the variables that the AI will need but also accessible to any class
public class bVar
{
    public static int batteryPercentage = 100; // The AI's battery percentage
    public static bool connectedToAnything = false; // Is the AI battery connected to any of the devices
    public static float batteryUpWaitTime = 1f; // How long does the battery have to wait every percentage when charging

    public static float connectWaitTime; // How long should the AI wait between connecting each device
}
