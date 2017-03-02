using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    float speed = 10;
    float damage = 1;
    float lifeTime = 2;
    //Adding more to projectile ray
    //Helps with collision detection
    float skinWidth = .1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);

        if(initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }
    }

    public void SetSpeed( float newSpeed)
    {
        speed = newSpeed;
    }

	void Update () {

        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);

	}

    void CheckCollisions(float moveDistance)
    {

        //Have Ray Move Forward from position
        Ray ray = new Ray(transform.position, transform.forward);
        //Ray will be hitting
        RaycastHit hit;

        //skinWidth adds more to the ray
        if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }




    void OnHitObject(Collider c, Vector3 hitPoint)
    {

        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        GameObject.Destroy(gameObject);
    }
}
