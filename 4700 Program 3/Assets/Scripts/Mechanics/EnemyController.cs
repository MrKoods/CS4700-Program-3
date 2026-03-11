using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        [Header("Movement")]
        public bool useHopping = false;
        public float patrolSpeedMultiplier = 0.5f;
        public float hopInterval = 1.0f;
        public float hopStrength = 7.0f;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        float hopTimer;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            hopTimer = hopInterval;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                PlayerEnemyCollision ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (path != null)
            {
                if (mover == null)
                {
                    mover = path.CreateMover(control.maxSpeed * patrolSpeedMultiplier);
                }

                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1f, 1f);
            }
            else
            {
                control.move.x = 0f;
            }

            if (useHopping)
            {
                hopTimer -= Time.deltaTime;

                if (hopTimer <= 0f && control.IsGrounded)
                {
                    control.Bounce(new Vector2(control.move.x, hopStrength));
                    hopTimer = hopInterval;
                }
            }
        }
    }
}