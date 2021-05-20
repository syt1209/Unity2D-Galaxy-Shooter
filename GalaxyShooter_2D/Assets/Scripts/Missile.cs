using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // state variable
    [SerializeField] private float _speed = 50.0f;
    [SerializeField] private float _rotateSpeed = 100.0f;

    // config variable
    private float _maxY = 8.0f;
    
    // cached references
    private Transform _target;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (_rb is null)
        {
            Debug.LogError("Rigid Body is NULL"); 
        }
        if (_target is null)
        {
            Debug.LogError("No enemy detected. Move up");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (_target is null)
        {
            MoveUpward();
            return;
        }
        else
        {
            Homing();
        }
    }

    private void MoveUpward()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > _maxY)
        {
            Destroy(this.gameObject);
        }   
    }

    private void Homing()
    {
        _rb.velocity = transform.up * _speed * Time.deltaTime;
        Vector3 direction = (_target.position - _rb.position).normalized;
        float rotateAmplitude = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = new Vector3(0, 0, -rotateAmplitude * _rotateSpeed);
    }
}
