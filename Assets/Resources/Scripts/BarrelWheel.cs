using UnityEngine;
using System.Collections;

public class BarrelWheel : Barrel
{

	//public GameObject bulletPrefab;
	public float radius = 1f;
	public int numberOfBullets = 1;
	public float angularVelocity = 60f;
	//public int type;
	//public bool isPassable = false;
    public float thetaOffset = 0f;

    private Rigidbody2D body;
    private bool ringCreated = false;

    // Use this for initialization
    //protected override void Start()
    //{
        
        //body = gameObject.GetComponentInParent<Rigidbody2D>();

        //body.angularVelocity = angularVelocity;

        //float preTheta = (2f * Mathf.PI) / numberOfBullets; //calculate this outside of the loop
        //float twoPi = Mathf.PI * 2f;

        //for (int i = 0; i < numberOfBullets; i++)
        //{
        //    float theta = i * preTheta;
        //    float thetaInDegs = (360 * theta) / twoPi;

        //    Vector3 bulletPosition = transform.position;
        //    bulletPosition.x = transform.position.x + radius * Mathf.Cos(theta);
        //    bulletPosition.y = transform.position.y + radius * Mathf.Sin(theta);


        //    Quaternion bulletRotation = Quaternion.Euler(0f, 0f, thetaInDegs + 180f + thetaOffset) ;

        //    Bullet bullet = ((GameObject)Instantiate(bulletPrefab, bulletPosition, bulletRotation)).GetComponent<Bullet>();
        //    bullet.SetType(type);
        //    bullet.owner = GetComponentInParent<Player>();
        //    bullet.isPassable = isPassable;
        //    bullet.speed = 0f;

        //    bullet.transform.parent = transform;
            
        //}


        
    //}


    // Update is called once per frame
    protected override void Update()
	{
		if (!fireOn && burstDelay == 0) {
			FireOn ();
		}
        if (!inBurst && Time.time > burstTimer + burstDelay)
        {
            inBurst = true;
            burstTimer = Time.time;
        }else if (inBurst && Time.time > burstTimer + burstTime)
        {
            inBurst = false;
            burstTimer = Time.time;
        }

        if (fireOn && inBurst && !ringCreated)
        {
            CreateRing();
            ringCreated = true;
         
        }

        if ((!inBurst || !fireOn) && ringCreated)
        {
            DestroyRing();
            ringCreated = false;
        }

        transform.Rotate(0f, 0f, angularVelocity * Time.deltaTime, Space.World);

    }

    public override void FireOn()
    {
        base.FireOn();


    }

    public override void FireOff()
    {
        base.FireOff();

    }

    private void CreateRing()
    {
        body = gameObject.GetComponentInParent<Rigidbody2D>();

        body.angularVelocity = angularVelocity;

        float preTheta = (2f * Mathf.PI) / numberOfBullets; //calculate this outside of the loop
        float twoPi = Mathf.PI * 2f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float theta = i * preTheta;
            float thetaInDegs = (360 * theta) / twoPi;

            Vector3 bulletPosition = transform.position;
            bulletPosition.x = transform.position.x + radius * Mathf.Cos(theta);
            bulletPosition.y = transform.position.y + radius * Mathf.Sin(theta);


            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, thetaInDegs + 180f + thetaOffset);

            Bullet bullet = ((GameObject)Instantiate(bulletPrefab, bulletPosition, bulletRotation)).GetComponent<Bullet>();
            bullet.SetType(type);
            bullet.owner = GetComponentInParent<Player>();
            bullet.isPassable = isPassable;
            bullet.speed = 0f;

            bullet.transform.parent = transform;

        }
    }

    private void DestroyRing()
    {
        Bullet[] bullets = GetComponentsInChildren<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
    }

    
}