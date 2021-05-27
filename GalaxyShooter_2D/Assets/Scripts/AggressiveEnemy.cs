using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : Enemy
{
    // state variables
    private bool _aggressive = false;
    private float _colliderXsize = 2.0f;
    private float _dodgeMagnitude = 2.0f;

    // cached reference
    private GameObject _laser;

    protected override void Start()
    {
        base.Start();
        _enemyPoints = 80;
    }

    protected override void Update()
    {
        base.Update();

        _laser = GameObject.FindGameObjectWithTag("Laser");
        if (_laser is null)
        {
            Debug.Log("No laser detected");
        }
        else
        {
            DodgingStatus(_laser.transform.localPosition);
        }

    }

    protected override void MoveAlongPath(Transform path)
    {
        if (_player != null)
        {
            AggressiveStatus(_player.transform.localPosition);
            if (_aggressive is false)
            {
                base.MoveAlongPath(path);
            }
            else
            {
                Vector3 currentPos = transform.position;
                _currentTarget = _player.transform.position;
                transform.position = Vector3.MoveTowards(currentPos, _currentTarget, _enemySpeed * Time.deltaTime);
            }
        }
    }

    private void AggressiveStatus(Vector3 playerPos)
    {
        Vector3 distanceVector = playerPos - transform.localPosition;
        float distance = distanceVector.magnitude;

        if (distance < 5.0f)
        {
            _aggressive = true;
        }
        else
        {
            _aggressive = false;
        }
    }

    private void DodgingStatus(Vector3 laserPos)
    {
        Vector3 currentPos = transform.localPosition;
        float x_distance = laserPos.x - currentPos.x;
        float y_distance = currentPos.y - laserPos.y;

        if (y_distance < 2.0f)
        {

            if (x_distance < 0 && x_distance >-_colliderXsize/2)
            {
                Debug.Log("Dodging right");
                transform.position = new Vector3(currentPos.x + _dodgeMagnitude, currentPos.y, currentPos.z);
            }

            else if (x_distance > 0 && x_distance < _colliderXsize/2)
            {
                Debug.Log("Dodging left");
                transform.position = new Vector3(currentPos.x - _dodgeMagnitude, currentPos.y, currentPos.z);
            }

        }

    }
}

