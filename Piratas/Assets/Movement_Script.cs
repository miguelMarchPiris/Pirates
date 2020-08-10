using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Script : MonoBehaviour
{
    public Rigidbody2D rigidb;
    string left_str = "a";
    string right_str = "d";
    string front_str = "w";
    string back_str = "s";

    bool front = false;
    bool back = false;
    bool left = false;
    bool right = false;



    public float factor_turn_drag = 0.1f;
    public float torque = 10f;
    public float movement_force = 100f;
    public float lateral_force = 100f;
    public float jump_velocity = 25f;
    public float max_velocity_x = 10f;
    public float jump_away_from_danger_velocity = 25f;
    public float margen = 1f;
    public float MAX_VELOCITY = 30f;

    float direccion_giro;

    private Vector2 empuje;


    public float factor_freno_eje_x = 0.01f;

    private void Start()
    {
        empuje = new Vector2(0f, movement_force);
    }

    void Update()
    {
        if (Input.GetKey(front_str))
        {
            front = true;
        }
        if (Input.GetKey(left_str))
        {
            left = true;
        }
        if (Input.GetKey(right_str))
        {
            right = true;
        }
        if (Input.GetKey(back_str))
        {
            back = true;
        }
    }
    private void FixedUpdate()
    {
        Move();
        TurnDrag();



        //Al final siempre
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
        float factor_torque = Sigmoide(rigidb.velocity.magnitude/ MAX_VELOCITY); // no funciona del todo bien
        rigidb.AddTorque(direccion_giro * torque );

        //rigidb.angularVelocity += incremento_rotacion*direccion_giro;

        if (front)
        {
            //rigidb.velocity = new Vector2(rigidb.velocity.x, jump_velocity);

            //rigidb.velocity = new Vector2(Mathf.Sin(Mathf.Deg2Rad * rigidb.rotation) * (-movement_force), Mathf.Cos(Mathf.Deg2Rad * rigidb.rotation) * movement_force);
            //rigidb.AddForce(new Vector2(Mathf.Sin(Mathf.Deg2Rad * rigidb.rotation) * (-movement_force), Mathf.Cos(Mathf.Deg2Rad * rigidb.rotation) * movement_force));
            
            rigidb.AddRelativeForce(empuje);

            
            //rigidb.AddRelativeForce
            
            //rigidb.rotation
            //rigidb.velocity += Vector2.up * jump_velocity;
            //rigidb.AddForce(new Vector2(0f, jump_force), ForceMode2D.Impulse);
        }
        if (back)
        {
            rigidb.velocity *= 0f;
            //rigidb.rotation *= 0f;
            rigidb.angularVelocity *= 0f;

        }
        //Debug.Log("Rotacion en radianes: " + Mathf.Deg2Rad*rigidb.rotation +" Sin: "+ Mathf.Sin(Mathf.Deg2Rad * rigidb.rotation) + " Cos: " + Mathf.Cos(Mathf.Deg2Rad * rigidb.rotation));
    }

    private void TurnDrag()
    {
        //Vector2 dir = //transform.rotation;
        float rot = rigidb.rotation;
        Vector2 vel = rigidb.velocity;
        Vector2 v = new Vector2(0f, 1f);
        float ang = Vector2.Angle(vel, v);
        float ang2 = Vector2.Angle(v, vel);
        if (vel.x > 0)
        {
            ang = -ang;
        }
        rot = ModAngle(rot);
        ang = ModAngle(ang);

        float sin_dif = Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * (rot - ang)));
        //      rozamiento = abs_sin_diff * velocidad * (vector opuesto a la direccion de la velocidad)
        Vector2 drag = Mathf.Pow(sin_dif,2) * vel.magnitude * (-1f * vel.normalized) ;
        drag = drag * factor_turn_drag;
        rigidb.AddForce(drag);

        //Debug.Log("Dir: "+ rot+ " Vel: "+ ang+ " Dir-Vel: " +(rot-ang));
        //Debug.Log("Sin: "+Mathf.Sin(Mathf.Deg2Rad*(rot - ang)));
        Debug.DrawRay(rigidb.position, vel  , Color.green);
        Debug.DrawRay(rigidb.position, drag , Color.red);
        

        Vector2 frente;
        if (front)
        {
            frente = transform.localToWorldMatrix.MultiplyVector(empuje);
        }
        else
        {
            frente = new Vector2(0, 0);
        }
        Debug.DrawRay(rigidb.position, frente + drag, Color.magenta);

        
        Debug.Log(transform.forward);
        //Debug.DrawRay(rigidb.position, transform.rotation, Color.red);
        Debug.Log("Velocity:" + vel + "Angulo vel:1 "+ ang);
        Debug.Log("Velocity:" + vel + "Angulo vel:2 " + ang2);
    }
    private float ModAngle(float angle)
    {
        while (angle < 0)
        {
            //pongo 1080 para ahorrar calculos innecesarios
            // 1080= 360*3, 1080%360 = 0, lo que pasa es que no se hace 
            //el módulo de los números negativos
            angle += 1080;
        }
        return angle % 360;
    }

    private void ResetBooleans()
    {
        left = false;
        right = false;
        front = false;
        back = false;
    }
    private bool CheckMaxVelocity()
    {
        return !(Mathf.Abs(rigidb.velocity.x) >= max_velocity_x & direccion_giro * rigidb.velocity.x > 0f);
        //return true;
    }
    private float Sigmoide(float x)
    {
        //He tuneado la sigmoide para que de valores suaves entre el 0 y el 1,
        // de manera que en el 0 da valores pequeños pero no nulos.
        x -= 0.5f;
        x *= 6f;
        return 1 - 1 / (1 + Mathf.Exp(x));
    }
}
