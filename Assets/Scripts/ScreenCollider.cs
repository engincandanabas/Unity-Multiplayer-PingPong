using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System;
using System.Collections.Generic;


public class ScreenCollider : NetworkBehaviour
{
    private EdgeCollider2D edgeCollider2D;

    private void Awake()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        CreateEdgeCollider();
    }
    private void CreateEdgeCollider()
    {
        List<Vector2> edges=new List<Vector2>();
        edges.Add(Camera.main.ScreenToWorldPoint(Vector2.zero));
        edges.Add(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,0)));
        edges.Add(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));
        edges.Add(Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));
        edges.Add(Camera.main.ScreenToWorldPoint(Vector2.zero));

        edgeCollider2D.SetPoints(edges);
    }
}
