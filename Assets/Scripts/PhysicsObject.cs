using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float MinGroundNormalY = 0.65f;
    public float GravityModifier = 1.0f;

    protected bool Grounded;
    protected Vector2 TargetVelocity;
    protected Vector2 GroundNormal;
    protected Vector2 Velocity;
    protected Rigidbody2D Rb2d;
    protected ContactFilter2D ContactFilter;
    protected RaycastHit2D[] HitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> HitBufferList = new List<RaycastHit2D>(16);

    protected const float MinDistance = 0.001f;
    protected const float ShellRadius = 0.01f;

    private void OnEnable()
    {
        Rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ContactFilter.useTriggers = false;
        ContactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        ContactFilter.useLayerMask = true;
    }

    void Update()
    {
        TargetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    private void FixedUpdate()
    {
        Velocity += GravityModifier * Physics2D.gravity * Time.deltaTime;
        Velocity.x = TargetVelocity.x;

        Grounded = false;

        Vector2 deltaPosition = Velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    protected virtual void ComputeVelocity() { }

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinDistance)
        {
            int count =  Rb2d.Cast(move, ContactFilter, HitBuffer, distance + ShellRadius);

            HitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                HitBufferList.Add(HitBuffer[i]);
            }

            for (int i = 0;i < HitBufferList.Count; i++)
            {
                Vector2 currentNormal = HitBufferList[i].normal;

                if (currentNormal.y > MinGroundNormalY)
                {
                    Grounded = true;

                    if (yMovement) 
                    {
                        GroundNormal = currentNormal;
                        currentNormal.x = 0;
                    } 
                }

                float projection =  Vector2.Dot(Velocity, currentNormal);

                if (projection < 0)
                {
                    Velocity = Velocity - projection * currentNormal;
                }

                float modifiedDistance = HitBufferList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        Rb2d.position = Rb2d.position + move.normalized * distance;
    }
}