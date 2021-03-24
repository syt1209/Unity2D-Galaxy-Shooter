using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state variables
    private Vector3 _startPos = new Vector3(0, 0, 0);
    [SerializeField] private float _speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _speed);
    }
}
