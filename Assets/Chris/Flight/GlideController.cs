using UnityEngine;

namespace cst.Flight
{
    public class GlideController : ControllerBase, IControllerBase
    {
        private const float LANDING_DISTANCE = 32.0f;
        private const float MAX_ROLL_ANGLE = 37.5f;
        private const float MAX_PITCH_ANGLE = 65.0f;
        private const float TURN_TIGHTNESS = 2.0f;
        private const float INCREMENT_TURN_SPEED = 35.0f;
        private const float RETURN_TURN_SPEED = 30.0f;
        private const float INCREMENT_PITCH_SPEED = 45.0f;
        private const float INCREMENT_VELOCITY = 40.0f;
        private const float DECREMENT_VELOCITY = 25.0f;
        private const float RESTING_VELOCITY = 125.0f;
        private const float MAX_VELOCITY = RESTING_VELOCITY * 2.0f;
        private const float MIN_VELOCITY = 0.0f;

        private Vector3 m_position;
        private Vector3 m_rotation;

        private float m_forwardSpeed;

        private bool m_glideSoundPlaying = false;
        private readonly AudioSource m_glideSound;

        public GlideController(SeraphController controller)
            : base(controller)
        {
            m_glideSound      = (AudioSource)GameObject.Find("Camera").AddComponent("AudioSource");
            m_glideSound.clip = (AudioClip)Resources.Load("Gliding");
        }

        public void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);

            m_forwardSpeed = data.velocity < RESTING_VELOCITY 
                ? RESTING_VELOCITY
                : data.velocity;
        }

        public void update()
        {
            m_rotation = transform.eulerAngles;
            m_position = transform.position;

            handleOrientationChange(Time.deltaTime);
            handleMoveForward(Time.deltaTime);
            handleAudio();
        }

        public void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public void triggerExit(Collider other)
        {
            Debug.Log(GetType().Name + " triggerExit()");
        }

        public void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");

            // Check if we've collided with something below us.
            if (Physics.Raycast(transform.position,
                new Vector3(0.0f, -1.0f, 0.0f), LANDING_DISTANCE))
            {
                stopAudio();
                controller.setState(SeraphState.LANDING);
            }
        }

        public void collisionExit(Collision other)
        {
            Debug.Log(GetType().Name + " collisionExit()");
        }

        public TransitionData transitionData()
        {
            return new TransitionData
            {
                direction = transform.forward,
                velocity = m_forwardSpeed
            };
        }

        private void handleAudio()
        {
            if (!m_glideSoundPlaying)
            {
                m_glideSound.loop = true;
                m_glideSound.playOnAwake = false;
                m_glideSound.Play();
                m_glideSoundPlaying = true;
            }
        }

        private void stopAudio()
        {
            m_glideSound.Stop();
            m_glideSoundPlaying = false;
        }

        private void handleOrientationChange(float delta)
        {
            handlePitchChange(delta);
            handleYawChange(delta);
            handleRollChange(delta);

            transform.eulerAngles = m_rotation;
        }

        // Handles the change in pitch based on the user input
        private void handlePitchChange(float delta)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetAxis("ControllerVertical") < 0)
            {
                turnVerticalDown(delta);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetAxis("ControllerVertical") > 0)
            {
                turnVerticalUp(delta);
            }
            else
            {
                turnVerticalNone(delta);
            }
        }

        // Handles the change in yaw based on the current roll
        private void handleYawChange(float delta)
        {
            float angle = m_rotation.z;

            if (angle > 180.0f)
                angle -= 360.0f;

            m_rotation.y = Helpers.wrapAngle(m_rotation.y
                - (delta * angle * TURN_TIGHTNESS));
        }

        // Handles the change in roll based on the user input
        private void handleRollChange(float delta)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("ControllerHorizontal") < 0)
            {
                if (m_rotation.z < 0.0f)
                {
                    turnHorizontalLeft(delta * 1.25f);
                }
                else
                {
                    turnHorizontalLeft(delta);
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetAxis("ControllerHorizontal") > 0)
            {
                if (m_rotation.z > 0.0f)
                {
                    turnHorizontalRight(delta * 1.25f);
                }
                else
                {
                    turnHorizontalRight(delta);
                }
            }
            else
            {
                turnHorizontalNone(delta);
            }
        }

        // Changes pitch based on up user input
        private void turnVerticalUp(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle > -MAX_PITCH_ANGLE)
            {
                m_rotation.x = Helpers.wrapAngle(m_rotation.x
                    - (delta * INCREMENT_PITCH_SPEED));
            }
        }

        // Changes pitch based on down user input
        private void turnVerticalDown(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle < MAX_PITCH_ANGLE)
            {
                m_rotation.x = Helpers.wrapAngle(m_rotation.x
                    + (delta * INCREMENT_PITCH_SPEED));
            }
        }

        // Returns pitch to neutral on no user input
        private void turnVerticalNone(float delta)
        {
            // Intentionally left empty - no return pitch behaviour
        }

        // Changes roll based on left user input
        private void turnHorizontalLeft(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle < MAX_ROLL_ANGLE)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z
                    + (delta * INCREMENT_TURN_SPEED));
            }
        }

        // Changes roll based on right user input
        private void turnHorizontalRight(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle > -MAX_ROLL_ANGLE)
            {
                m_rotation.z = Helpers.wrapAngle(
                    m_rotation.z - (delta * INCREMENT_TURN_SPEED));
            }
        }

        // Returns roll to neutral on no user input
        private void turnHorizontalNone(float delta)
        {
            float angle = m_rotation.z - 180.0f;

            if (angle > 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z
                    + delta * RETURN_TURN_SPEED);

                if (m_rotation.z - 180.0f < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else if (angle < 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z
                    - delta * RETURN_TURN_SPEED);

                if (m_rotation.z - 180.0f > 0.0f)
                    m_rotation.z = 0.0f;
            }
        }

        // Handles moving forward
        private void handleMoveForward(float delta)
        {
            // Get the normalized pitch
            float pitch = Helpers.getNormalizedAngle(m_rotation.x);

            // Get rest speed based on pitch
            float restSpeed = RESTING_VELOCITY + (pitch *
                ((MAX_VELOCITY - RESTING_VELOCITY) / MAX_PITCH_ANGLE));

            // Convert pitch to a unit vector
            pitch /= MAX_PITCH_ANGLE;

            // Calculate the step each frame
            float step = pitch * delta;

            // Don't go above the calculated rest speed
            if (m_forwardSpeed > restSpeed && pitch > 0.0f)
                step = -step;

            if (step > 0.0f)
                step *= INCREMENT_VELOCITY;
            else
                step *= DECREMENT_VELOCITY;

            m_forwardSpeed += step;

            // Drop out of flight if we stall
            if (m_forwardSpeed < MIN_VELOCITY)
            {
                stopAudio();
                controller.setState(SeraphState.GROUNDED);
            }

            m_position += transform.forward * m_forwardSpeed
                * delta;

            transform.position = m_position;
        }
    }
}