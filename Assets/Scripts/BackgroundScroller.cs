using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;

    void Start()
    {
        startPos = transform.position;

        // ambil full width dari background (pakai collider atau sprite renderer)
        repeatWidth = GetComponent<BoxCollider2D>().size.x;
    }

    void Update()
    {
        // kalau background sudah keluar layar sejauh repeatWidth, reset ke posisi awal
        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}

