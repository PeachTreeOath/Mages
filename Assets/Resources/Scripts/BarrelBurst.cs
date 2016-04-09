using UnityEngine;
using System.Collections;

public class BarrelBurst : MonoBehaviour {

    public GameObject bulletPrefab;
    public float radius = 1f;
    public int numberOfBullets = 16;
    public float angularSpeed = 0f;
    public float angularDrag = 0f;
    public float speed = 1f;
    public int type;
    public bool isPassable = false;
    public float shotDelay = .3f;
    public float burstTime = 1f;
    public float burstDelay = 3f;
    public float perturbation = .1f;

    private bool readyToShoot = true;
    private bool inBurst = false;
    private float burstTimer;
    private Rigidbody2D body;
    private float preTheta;
    private float twoPi;

    // Use this for initialization
    void Start()
    {

        body = gameObject.GetComponentInParent<Rigidbody2D>();

        preTheta = (2f * Mathf.PI) / numberOfBullets; //calculate this outside of the loop
        twoPi = Mathf.PI * 2f;
        burstTimer = Time.time;
    }


    // Update is called once per frame
    void Update()
    {
        if (!inBurst && Time.time > burstTimer + burstDelay)
        {
            inBurst = true;
            burstTimer = Time.time;
        }
        if (inBurst && Time.time > burstTimer + burstTime)
        {
            inBurst = false;
            burstTimer = Time.time;
        }
        if (inBurst && readyToShoot)
        {
            CmdFire();
            readyToShoot = false;
            Invoke("ResetReadyToShoot", shotDelay);
        }
        transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime, Space.World);

    }

    void ResetReadyToShoot()
    {
        readyToShoot = true;
    }

    private void CmdFire()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            float theta = i * preTheta;
            float thetaInDegs = (360 * theta) / twoPi;

            Vector3 bulletPosition = transform.position;
            bulletPosition.x = transform.position.x + radius * Mathf.Cos(theta);
            bulletPosition.y = transform.position.y + radius * Mathf.Sin(theta);


            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, thetaInDegs - 90f);

            Bullet bullet = ((GameObject)Instantiate(bulletPrefab, bulletPosition, bulletRotation)).GetComponent<Bullet>();
            bullet.SetType(type);
            bullet.owner = GetComponentInParent<Player>();
            bullet.isPassable = isPassable;
            bullet.speed = speed;

            //bullet.transform.parent = transform;
            bullet.Fire();
        }



    }
}
