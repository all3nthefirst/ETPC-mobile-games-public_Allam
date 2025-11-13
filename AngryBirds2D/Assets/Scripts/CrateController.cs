using UnityEngine;

public class CrateController : MonoBehaviour
{
    public float minVelocity = 1.0f;
    private HealthController _healthCtr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthCtr = GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        if(collisionSpeed >  minVelocity )
        {
            HealthController _colHealthCtr = collision.collider.GetComponent<HealthController>();
            float damage = collisionSpeed * collision.rigidbody.mass;

            if(_colHealthCtr)
            {
                float colDmg = _colHealthCtr.GetDamage();
                damage = damage * colDmg;
            }

            _healthCtr.UpdateHealth(damage);
            Debug.Log(damage);
        }
    }
}
