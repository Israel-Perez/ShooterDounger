using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;
    private bool isfacingright = true;

    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
        flip();
        animator.SetFloat("speed", input.magnitude);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = (input*speed);
    }

    void flip() {
        if (isfacingright && input.x < 0f || !isfacingright && input.x > 0f)
        { 
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
            isfacingright = !isfacingright;
        }
    }
    
    void move()
    {
        float xinput = Input.GetAxisRaw("Horizontal");
        float yinput = Input.GetAxisRaw("Vertical");
        input = new Vector2(xinput, yinput).normalized;
    }
}
