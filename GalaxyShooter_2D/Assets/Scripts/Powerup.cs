﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // state variables
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _powerupClip;
    private float _speedTowardsPlayer = 5.0f;

    // config variables
    private float _yMin = -4.5f;

    // cached reference
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if (_playerTransform is null)
        {
            Debug.LogError("Player Transform is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // player collects powerup
        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speedTowardsPlayer * Time.deltaTime);
            return;
        }
        
        MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _yMin)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("ID" + _powerupID + "not found");
                        break;
                }
            }

            AudioSource.PlayClipAtPoint(_powerupClip, transform.position);

            Destroy(this.gameObject);
        }
    }
}
