using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Script : MonoBehaviour
{
    public Rigidbody2D rigidb;
    string left_str = "a";
    string right_str = "d";
    string jump_str = "w";

    bool accelera = false;
    bool frena = false;
    bool left = false;
    bool right = false;

    public float movement_force = 100f;
    public float lateral_force = 100f;
    public float jump_velocity = 25f;
    public float max_velocity_x = 10f;
    public float jump_away_from_danger_velocity = 25f;
    public float margen = 1f;

    float direccion_giro;



    public float factor_freno_eje_x = 0.01f;

    void Update()
    {
        if (Input.GetKey(jump_str))
        {
            accelera = true;
        }
        if (Input.GetKey(left_str))
        {
            left = true;
        }
        if (Input.GetKey(right_str))
        {
            right = true;
        }
    }
    private void FixedUpdate()
    {
        Move();
        ResetBooleans();
        //CheckGameOver();
    }
    private void Move()
    {
        if (left)
        {
            direccion_giro = 1f;
        }
        if (right)
        {
            direccion_giro = -1f;
        }
        if (left == right)
        {
            direccion_giro = 0f;
            rigidb.velocity = new Vector2(rigidb.velocity.x * (1 - factor_freno_eje_x), rigidb.velocity.y);
        }
        //if (CheckMaxVelocity()){rigidb.AddForce(new Vector2(direccion_x * lateral_force, 0f), ForceMode2D.Force);}

        //, ForceMode2D.Force);
        //(direccion_x * lateral_force, 0f, ForceMode2D.Force);

        rigidb.angularVelocity += 2*direccion_giro;

        if (accelera)
        {
            //rigidb.velocity = new Vector2(rigidb.velocity.x, jump_velocity);

            //rigidb.velocity = new Vector2(Mathf.Sin(Mathf.Deg2Rad * rigidb.rotation) * (-movement_force), Mathf.Cos(Mathf.Deg2Rad * rigidb.rotation) * movement_force);
            //rigidb.AddForce(new Vector2(Mathf.Sin(Mathf.Deg2Rad * rigidb.rotation) * (-movement_force), Mathf.Cos(Mathf.Deg2Rad * rigidb.rotation) * movement_force));
            rigidb.AddRelativeForce(new Vector2(0f,movement_force));
            //rigidb.AddRelativeForce
            
            //rigidb.rotation
            //rigidb.velocity += Vector2.up * jump_velocity;
            //rigidb.AddForce(new Vector2(0f, jump_force), ForceMode2D.Impulse);
        }
        //Debug.Log("Rotacion en radianes: " + Mathf.Deg2Rad*rigidb.rotation +" Sin: "+ Mathf.Sin(Mathf.Deg2Rad * rigidb.rotation) + " Cos: " + Mathf.Cos(Mathf.Deg2Rad * rigidb.rotation));
    }
    private void ResetBooleans()
    {
        left = false;
        right = false;
        accelera = false;
    }
    private bool CheckMaxVelocity()
    {
        return !(Mathf.Abs(rigidb.velocity.x) >= max_velocity_x & direccion_giro * rigidb.velocity.x > 0f);
        //return true;
    }
}
