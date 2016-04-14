using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boss : MonoBehaviour
{
	public float hp;
	public int phase = 0;
	public float speed = 2.0F;
    public float pauseTime = 1;
    public GameObject hpCanvas;
    public BossPhase[] phases;

    private float journeyLength;
	private int lastIndexReached = 0;
	private float startTime;
	private Image hpMeterImage;
	private bool pauseStarted = false;
	private float totalHp;
    private int currentPhaseIndex = 0;
    private BossPhase currentPhase;
    private bool doneMoving = false;
    private Vector3 startingPosition; //remembers the position of the boss when the phase changes
    

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
		totalHp = hp;
		hpCanvas.SetActive (true);
		hpMeterImage = hpCanvas.GetComponentInChildren<GameObjectFinder> ().GetComponent<Image> ();

        BossPhase nextPhase = phases[currentPhaseIndex];
        currentPhase = (BossPhase)Instantiate(nextPhase, transform.position, transform.rotation);
        currentPhase.transform.parent = this.transform;

        startingPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
	{

        Transform[] markers = currentPhase.checkpoints;
        float distCovered;
        float fracJourney;

        //Use a movement strategy from the currently selected phase.
        switch (currentPhase.movementStrategy)
        {
            
            case (BossPhase.MovementStrategy.LERP_BETWEEN_POINTS):
                int nextIndex = (lastIndexReached + 1) % markers.Length;
                journeyLength = Vector3.Distance(markers[lastIndexReached].position, markers[nextIndex].position);
                distCovered = (Time.time - startTime) * speed;

                if ( journeyLength - distCovered <= 0f)
                {
                    lastIndexReached = (lastIndexReached + 1) % markers.Length;
                    nextIndex = (lastIndexReached + 1) % markers.Length;
                    journeyLength = Vector3.Distance(markers[lastIndexReached].position, markers[nextIndex].position);
                    startTime = Time.time;
                    distCovered = (Time.time - startTime) * speed;

                }

                fracJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(markers[lastIndexReached].position, markers[nextIndex].position, fracJourney);

                break;
            case (BossPhase.MovementStrategy.MOVE_TO_POINT_AND_STAY):
                if (!doneMoving)
                {
                    journeyLength = Vector3.Distance(startingPosition, markers[0].position);
                    distCovered = (Time.time - startTime) * speed;

                    if (journeyLength - distCovered <= 0f)
                    {
                        doneMoving = true;
                    }

                    fracJourney = distCovered / journeyLength;

                    transform.position = Vector3.Lerp(startingPosition, markers[0].position, fracJourney);
                }
                break;
            case (BossPhase.MovementStrategy.STATIONARY):
            default:
                //We don't need no movement
                break;
        }



        //change phases
        if (hp < .99f * totalHp)
        {
            if (currentPhaseIndex < phases.Length - 1)
            {
                
                if (currentPhase != null)
                {
                    Destroy(currentPhase.gameObject);
                }
                currentPhaseIndex++;

                BossPhase nextPhase = phases[currentPhaseIndex];
                currentPhase = (BossPhase)Instantiate(nextPhase, transform.position, transform.rotation);
                currentPhase.transform.parent = this.transform;

                //Remember the location and time of the boss during the phase change so we don't get lost on the next movement.
                startingPosition = transform.position;
                startTime = Time.time;
            }
        }


	}

	void OnTriggerEnter2D (Collider2D col)
	{
		Bullet bullet = col.GetComponent<Bullet> ();

		if (bullet != null) {
			hp -= bullet.damage;
			if (hp <= 0) {
				hpCanvas.SetActive (false);
			} else {
				hpMeterImage.fillAmount = hp / totalHp;
			}
		}
	}
}
