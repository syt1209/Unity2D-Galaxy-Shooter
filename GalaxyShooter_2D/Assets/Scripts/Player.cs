using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state variables
    private Vector3 _startPos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
