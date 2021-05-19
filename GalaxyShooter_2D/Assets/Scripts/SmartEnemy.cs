using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy
{
    // cached reference
    [SerializeField] private GameObject _laserPrefab;

    // state variables
    private Vector3 _offset = new Vector3(0, -0.85f, 0);
    private WaitForSeconds _firingDelay = new WaitForSeconds(3.0f);
    
    protected override void Start()
    {
        base.Start();
        StartCoroutine(FireRoutine(_laserPrefab));
    }

    IEnumerator FireRoutine(GameObject laser)
    {
        while (true)
        {
            GameObject enemyLaserObject = Instantiate(laser, transform.position + _offset, Quaternion.identity) as GameObject;
            EnemyLaser enemyLaser = enemyLaserObject.GetComponent<EnemyLaser>();
            if (_player != null)
            {
                if (_player.transform.position.y > transform.position.y)
                {
                    enemyLaser.ShootBackward();
                }

                else
                {
                    enemyLaser.ShootForward();
                }
            }

            yield return _firingDelay;
        }
    }
}
