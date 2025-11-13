using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _damage = 8f;
    [SerializeField] private float _minDamageVelocity = 1.0f;
    [SerializeField] private float _destroyDelay = 2.0f;
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private UnityEvent method;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        if (collisionSpeed > _minDamageVelocity)
        {
            HealthController _colHealthCtr = collision.collider.GetComponent<HealthController>();
            float damage = collisionSpeed;

            if (collision.rigidbody != null)
            {
                damage = damage * collision.rigidbody.mass;
            }

            if (_colHealthCtr)
            {
                float colDmg = _colHealthCtr.GetDamage();
                damage = damage * colDmg;
            }

            UpdateHealth(damage);
            Debug.Log(damage);
        }
    }

    public void UpdateHealth(float damage)
    {
        _health = _health - damage;

        if(_health > 99)
        {
            _renderer.sprite = _sprites[0];
        }
        else if (_health < 99 && _health > 50)
        {
            _renderer.sprite = _sprites[1];
        }
        else if (_health > 0)
        {
            _renderer.sprite = _sprites[2];
        }
        else
        {
            gameObject.SetActive(false);
            SlingshotController.instance.StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSecondsRealtime(_destroyDelay);

        Destroy(gameObject);
        method?.Invoke();
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetDamage()
    {
        return _damage;
    }
}
