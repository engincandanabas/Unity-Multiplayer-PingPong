using System;
using UnityEngine;

public class ScreenSizeManager : MonoBehaviour
{
    public static ScreenSizeManager Instance { get; private set; }

    public event EventHandler OnSizeChanged;

    public Vector2 ScreenSize { get; private set; }

    private Vector2 lastScreenSize;
    private void Awake()
    {
        Instance = this;

        ScreenSize = new Vector2(Screen.width, Screen.height);
        lastScreenSize = ScreenSize;
    }
    private void Update()
    {
        if (this.lastScreenSize != ScreenSize)
        {
            this.lastScreenSize = ScreenSize;
            Debug.Log("Screen size changed");
            OnSizeChanged?.Invoke(this,null);                      
        }
    }
}
