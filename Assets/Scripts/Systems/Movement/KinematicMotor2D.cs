using UnityEngine;

namespace Systems.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KinematicMotor2D : MonoBehaviour, IRigidbody2DAdapter
    {
        private const float minMoveDistance = 0.001f;
        private const float shellRadius = 0.01f;
        private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        private Rigidbody2D body;
        private ContactFilter2D contactFilter;
        public float gravityModifier = 1f;

        private Vector2 groundNormal = Vector2.up;
        public float minGroundNormalY = 0.65f;
        public Vector2 velocity;
        public bool IsGrounded { get; private set; }

        public Vector3 Position => body.position;

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public void AddImpulse(Vector2 force)
        {
            velocity += force;
        }

        public void Teleport(Vector3 position)
        {
            body.position = position;
            velocity = Vector2.zero;
            body.velocity = Vector2.zero;
        }

        public void Stop()
        {
            body.velocity = Vector2.zero;
        }

        public void Bounce(float value)
        {
            velocity.y = value;
        }

        public void Bounce(Vector2 dir)
        {
            velocity = dir;
        }

        private void OnEnable()
        {
            if (!body) body = GetComponent<Rigidbody2D>();
            body.isKinematic = true;
        }

        private void OnDisable()
        {
            if (body) body.isKinematic = false;
        }

        private void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        private void FixedUpdate()
        {
            if (velocity.y < 0) velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            else velocity += Physics2D.gravity * Time.deltaTime;

            IsGrounded = false;

            var delta = velocity * Time.deltaTime;

            var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            var moveX = moveAlongGround * delta.x;
            PerformMovement(moveX, false);

            var moveY = Vector2.up * delta.y;
            PerformMovement(moveY, true);
        }

        private void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;
            if (distance > minMoveDistance)
            {
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;

                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    if (IsGrounded)
                    {
                        var projection = Vector2.Dot(velocity, currentNormal);
                        if (projection < 0) velocity -= projection * currentNormal;
                    }
                    else
                    {
                        velocity.x = 0;
                        velocity.y = Mathf.Min(velocity.y, 0);
                    }

                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    if (modifiedDistance < distance) distance = modifiedDistance;
                }
            }

            body.position = body.position + move.normalized * distance;
        }
    }
}