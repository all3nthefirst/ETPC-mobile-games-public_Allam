using UnityEngine;

public class EggController : MonoBehaviour
{
    public float damageRadius = 1f;
    private Rigidbody2D _rBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Vector3 dir = _rBody.linearVelocity.normalized;
        int layerMask = LayerMask.GetMask("Box", "Enemy");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, damageRadius, Vector3.down, damageRadius, layerMask);

        if (hits != null)
        {
            Debug.Log("Collision detected! " + hits.Length);

            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(hits[i].collider.gameObject.name);
                Destroy(hits[i].collider.gameObject);
            }

            Destroy(this);
        }
    }
}
