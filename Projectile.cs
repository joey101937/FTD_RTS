using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject target = null;
    public GameObject explosionEffect = null; //to be set in inspector
    public int damage = 0;
    public float speed = 1.7f;
    bool hasLaunched = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        if (hasLaunched)
        {
            Vector3 destination = target.transform.position;
            BaseUnit unit = target.GetComponent<BaseUnit>();
            if (unit != null)
            {
                destination += unit.getHitOffset();
            }
            transform.LookAt(destination);
            gameObject.transform.Translate(0, 0, speed);
        }
    }

    /*
     * launches the projectile at target
     */
    public void launch()
    {
        if (target == null)
        {
            return;
        }
        hasLaunched = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        BaseUnit hit = other.gameObject.GetComponent<BaseUnit>();
        if (hit != null && hit==target.GetComponent<BaseUnit>())
        {
            OnImpact(hit);
        }
    }

    /*
     * what we want to have happen when we hit another unit
     */
    public virtual void OnImpact(BaseUnit hit)
    {
        hit.TakeDamage(damage);
        GameObject.Destroy(this.gameObject);
        if (explosionEffect != null)
        {
            GameObject impactEffect = Instantiate(this.explosionEffect,this.transform.position,this.transform.rotation);
            impactEffect.transform.localScale = new Vector3(.1f, .1f, .1f);
        }
    }
}