using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform target1, target2;
    [SerializeField] private float moveSpeed;

    private Transform currectTarget;

    private void Start()
    {
        currectTarget = target1;
    }

    private void FixedUpdate()
    {
        if (transform.position == target1.position)
        {
            currectTarget = target2;
        }

        if (transform.position == target2.position)
        {
            currectTarget = target1;
        }

        transform.position = Vector2.MoveTowards(transform.position, currectTarget.position, moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
