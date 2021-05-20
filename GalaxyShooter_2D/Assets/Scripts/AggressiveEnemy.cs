using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : Enemy
{
    [SerializeField] private bool _aggressive = false;

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
}

