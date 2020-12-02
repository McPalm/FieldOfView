using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFuckingMovement : MonoBehaviour
{
    Rigidbody2D me;

    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var x = 0;
        var y = 0;
        if (Input.GetKey(KeyCode.DownArrow))
            y--;
        if (Input.GetKey(KeyCode.UpArrow))
            y++;
        if (Input.GetKey(KeyCode.LeftArrow))
            x--;
        if (Input.GetKey(KeyCode.RightArrow))
            x++;

        me.MovePosition(transform.position + new Vector3(x, y) * speed * Time.fixedDeltaTime);
        
        if (x > 0f)
            transform.rotation = Quaternion.identity;
        else if (x < 0f)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            
    }
}
