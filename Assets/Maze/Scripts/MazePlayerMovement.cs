using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MazePlayerMovement : MonoBehaviour
{
    private HFTInput m_hftInput;
    private Rigidbody2D _rigidbody;

    public float moveDistance = 1f;

    protected bool prevHMove = false;
    protected bool prevVMove = false;

    protected void Start()
    {
        m_hftInput = GetComponent<HFTInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get left/right input (get both phone and local input)
        float move = m_hftInput.GetAxis("Horizontal") + Input.GetAxis("Horizontal");
        float vMove = m_hftInput.GetAxis("Vertical") + Input.GetAxis("Vertical");

        float minMove = 0.05f;
        if (Mathf.Abs(move) > minMove && !prevHMove)
        {
            transform.position += Vector3.right * move * moveDistance;
            prevHMove = true;
        }
        if (Mathf.Abs(move) < minMove)
        {
            prevHMove = false;
        }

        if (Mathf.Abs(vMove) > minMove && !prevVMove)
        {
            transform.position += Vector3.up * -vMove * moveDistance;
            prevVMove = true;
        }
        if (Mathf.Abs(vMove) < minMove)
        {
            prevVMove = false;
        }
    }
}
