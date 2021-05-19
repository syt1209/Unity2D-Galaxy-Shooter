using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : Laser
{
    private bool _shootBackward = false;
    
    protected override void Move()
    {
        if (_shootBackward is true)
        {
            base.Move();
        }

        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -_maxPos)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
    }

    public void ShootBackward()
    {
        _shootBackward = true;
    }

    public void ShootForward()
    {
        _shootBackward = false;
    }
}
