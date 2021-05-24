using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    // state variables
    [SerializeField] private int _shieldStrength = 3;

    // config variable
    private Vector3 _targetPos = new Vector3(0, 2.0f, 0);

    // cached reference
    [SerializeField] private GameObject _shield;


    protected override void Start()
    {
        base.Start();
        _enemyPoints = 500;
        _enemyLife = 10;
    }

    protected override void Update()
    {
        MoveToCenter();
    }

    private void MoveToCenter()
    {
        Vector3 currentPos = transform.position;
        transform.position = Vector3.MoveTowards(currentPos, _targetPos, _enemySpeed/2 * Time.deltaTime);
    }

    private void ShieldVisualize()
    {
        Renderer shieldRd = _shield.GetComponent<Renderer>();
        switch (_shieldStrength) 
        {
            case 0:
                _shield.SetActive(false);
                break;
            case 1:
                _shield.SetActive(true);
                shieldRd.material.color = Color.red;
                break;
            case 2:
                _shield.SetActive(true);
                shieldRd.material.color = Color.magenta;
                break;
            default:
                _shield.SetActive(true);
                shieldRd.material.color = Color.cyan;
                break;

        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (_enemyLife > 0)
        {
            if (_shield.active)
            {
                _shieldStrength--;
                ShieldVisualize();
            }
            else
            {
                if (other.tag is "Player")
                {
                    _player.Damage();
                    _enemyLife--;

                    if (_enemyLife < 1)
                    {
                        EnemyDeathSequence();
                    }
                }

                if (other.tag is "Laser")
                {
                    Destroy(other.gameObject);
                    _enemyLife--;

                    if (_enemyLife < 1)
                    {
                        EnemyDeathSequence();
                    }
                }

                if (other.tag is "Missile")
                {
                    Destroy(other.gameObject);
                    _enemyLife--;

                    if (_enemyLife < 1)
                    {
                        EnemyDeathSequence();
                    }
                }
            }
        }
        else
        {
            Debug.Log("You Won!");
        }
    }
}
