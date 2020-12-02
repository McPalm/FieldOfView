using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    public Transform parent;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = parent.transform.position;
    }
}
