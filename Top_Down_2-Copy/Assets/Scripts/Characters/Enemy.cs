using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State { Idle, Chasing, Attacking};
    State currState;

    public ParticleSystem deathEffect;

    NavMeshAgent pathFinder;
    Transform target;

    Material skinMaterial;
    Color originalColor;


    float attackDistanceThreshhold = .5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;

    float myCollisionRadius;
    float targetCollisionRadius;

    LivingEntity targetEntity;

    public bool hasTarget;
    Spawner spawner;
    float damage = 1;

	protected override void Start () {

        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

   
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {

                currState = State.Chasing;
                hasTarget = true;
                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetEntity = target.GetComponent<LivingEntity>();
                targetEntity.OnDeath += OnTargetDeath;

                myCollisionRadius = GetComponent<CapsuleCollider>().radius;
                targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
                StartCoroutine(UpdatePath());
    
            }

        
	}

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if(damage >= health)
        {
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection))as GameObject, deathEffect.startLifetime);

        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }


    void OnTargetDeath()
    {
        hasTarget = false;
        currState = State.Idle;
    }

    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float squrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (squrDstToTarget < Mathf.Pow(attackDistanceThreshhold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }

    }


    IEnumerator Attack()
    {
        currState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);


        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        bool hasAppliedDamage = false;

        while(percent <= 1)
        {
            if(percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 3 * percent) + percent) * 3;

            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }

        skinMaterial.color = originalColor;
        currState = State.Chasing;
        pathFinder.enabled = true;
    }

    //Fixed Timer
    //Repeats Loop at every RefreshRate
    IEnumerator UpdatePath()
    {
        float refreshRate = .025f;

        while (hasTarget)
        {
            if (currState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshhold/2);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);

                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
