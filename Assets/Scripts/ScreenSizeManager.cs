using System;
using UnityEngine;

public class ScreenSizeManager : MonoBehaviour
{
    public static ScreenSizeManager Instance { get; private set; }

    public event EventHandler OnSizeChanged;

    public Vector2 ScreenSize { get; private set; }

    private void Awake()
    {
        Instance = this;

        ScreenSize = new Vector2(Screen.width, Screen.height);
    }
    private void Update()
    {
        if (ScreenSize != new Vector2(Screen.width, Screen.height))
        {
            ScreenSize = new Vector2(Screen.width, Screen.height);

            Debug.Log("Screen size changed");
            OnSizeChanged?.Invoke(this,null);                      
        }
    }
}
