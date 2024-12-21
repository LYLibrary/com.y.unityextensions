using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public float angle = 1;
    void Update()
    {
        transform.Rotate(transform.forward, angle);
    }
}
