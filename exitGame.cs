using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class exitGame : MonoBehaviour
{
    public void onClick()
    {
        Application.Quit(0);
    }
}
