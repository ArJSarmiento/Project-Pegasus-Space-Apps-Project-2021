using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    public float speed;
    private Transform target;
    public int damage = 50;
    public GameObject impactEffect;
    public float explosionRadius = 0f;
    bool hasEnemy = false;
	Vector2 shootingDirection;
	public float destroydelay = 2.0f;
    bool FacingRight;

    // Use this for initialization


    public void Seek (Transform _target, bool hasEnemy_, Vector2 shootingDirection_, bool m_FacingRight)
	{
		target = _target;
		hasEnemy = hasEnemy_;
        shootingDirection = shootingDirection_;
        FacingRight = m_FacingRight;
	}

    void Update()
    {
        if (hasEnemy)
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World); ;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);            
        }
        else
        {
            if (shootingDirection == Vector2.zero)
            {
                if (FacingRight)
                {
                    shootingDirection = Vector2.right;
                }
                else
                {          
                    shootingDirection = Vector2.left;
                    transform.Rotate(0f, 0f, 180f);
                }   
            }
            else
            {
                shootingDirection =  shootingDirection;
            }
            shootingDirection.Normalize();
			this.GetComponent<Rigidbody2D>().velocity = shootingDirection * speed;
			this.transform.Rotate(0.0f, 0.0f, Mathf.Atan2 (shootingDirection.y, shootingDirection.x)*(Mathf.Rad2Deg));
            
			Destroy(this, destroydelay);
        }
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }
    public void Damage(Transform enemy)
    {
    //    EnemyTopDown e = enemy.GetComponent<EnemyTopDown>();
        // if (e != null)
        // {
        //     e.TakeDamage(damage);
        // }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}