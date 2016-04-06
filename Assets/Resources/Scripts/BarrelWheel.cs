﻿using UnityEngine;
using System.Collections;

public class BarrelWheel : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float radius = 1f;
    public int numberOfBullets = 1;
    public float angularVelocity = 60f;
    public int type;
    public bool isPassable = false;
    
    private Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        
        body = gameObject.GetComponentInParent<Rigidbody2D>();

        body.angularVelocity = angularVelocity;

        float preTheta = (2f * Mathf.PI) / numberOfBullets; //calculate this outside of the loop
        float piOver2 = Mathf.PI / 2f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float theta = i * preTheta;

            Vector3 bulletPosition = transform.position;
            bulletPosition.x = transform.position.x + radius * Mathf.Cos(theta);
            bulletPosition.y = transform.position.y + radius * Mathf.Sin(theta);


            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, theta + piOver2) ;

            Bullet bullet = ((GameObject)Instantiate(bulletPrefab, bulletPosition, bulletRotation)).GetComponent<Bullet>();
            bullet.SetType(type);
            bullet.owner = GetComponentInParent<Player>();
            bullet.isPassable = isPassable;
            bullet.speed = 0f;
            bullet.angularVelocity = angularVelocity;

            bullet.transform.parent = transform;
            //bullet.GetComponent<Rigidbody2D>().velocity += body.velocity;
            
        }


        
    }


    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0f, 0f, 50f * angularVelocity * Time.deltaTime, Space.World);
        //Bullet[] bullets = gameObject.GetComponentsInChildren<Bullet>();
        //foreach (Bullet bullet in bullets)
        //{
        //    bullet.transform.Rotate(0f, 0f, angularVelocity * Time.deltaTime, Space.World );
        //    bullet.transform.Rotate(0f, 0f, angularVelocity * Time.deltaTime, Space.Self);
        //}
    }

}