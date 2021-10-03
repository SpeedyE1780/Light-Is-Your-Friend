using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public PoolID ID;
    readonly float bulletSpeed = 10;

    public void SetBullet(Vector3 pos , Vector3 dir)
    {
        gameObject.SetActive(true);
        
        //Set bullet position rotation
        transform.position = pos;
        transform.forward = dir;

        Invoke(nameof(DestroyBullet), 0.5f);
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    void DestroyBullet()
    {
        PoolManager.Instance.AddToPool(ID, gameObject);
    }
}