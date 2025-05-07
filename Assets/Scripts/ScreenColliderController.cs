using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColliderController : MonoBehaviour
{
    [SerializeField] private List<Collider2D> colliders;


    private void Awake()
    {
        SetColliderPositions(this,null);
    }
    private void OnEnable()
    {
        ScreenSizeManager.Instance.OnSizeChanged += SetColliderPositions;
    }
    private void SetColliderPositions(object sender,EventArgs eventArgs)
    {
        var screenBounds=Camera.main.ScreenToWorldPoint(new Vector2 (Screen.width, Screen.height));

        colliders[0].transform.position = new Vector2(0, screenBounds.y + colliders[0].bounds.size.y / 2);
        colliders[1].transform.position = new Vector2(0, -(screenBounds.y + colliders[1].bounds.size.y / 2));
        colliders[2].transform.position = new Vector2((screenBounds.x + colliders[2].bounds.size.x / 2),0);
        colliders[3].transform.position = new Vector2(-(screenBounds.x + colliders[3].bounds.size.x / 2),0);
    }


}
