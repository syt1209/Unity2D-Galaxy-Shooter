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
        PowerupPickup();
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

    private void PowerupPickup()
    {
        GameObject positivePowerup = GameObject.FindGameObjectWithTag("PositivePower") as GameObject;
        if (positivePowerup != null)
        {
            Vector3 powerupPosition = positivePowerup.transform.localPosition;
            if (powerupPosition.y < transform.position.y && powerupPosition.x < 2.5f && powerupPosition.x > -2.5f)
            {
                Vector3 bossPosition = transform.position;
                positivePowerup.transform.position = Vector3.MoveTowards(powerupPosition, bossPosition, 10.0f*Time.deltaTime);

                float distance = (bossPosition - positivePowerup.transform.position).magnitude;
                if (distance < 0.5f)
                {
                    Destroy(positivePowerup.gameObject, 0.5f);
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PositivePower") || other.CompareTag("NegativePower"))
        {
            return;
        }
        else if (_enemyLife > 0)
        {
            if (_shield.active)
            {
                _shieldStrength--;
                ShieldVisualize();
            }
            else
            {
                base.OnTriggerEnter(other);
            }
        }
        else
        {
            Debug.Log("You Won!");
        }
    }
}
