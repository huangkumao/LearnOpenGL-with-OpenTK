using OpenTK;
using System;

namespace Common
{
    public enum Camera_Movement
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    public class Camera
    {
        //相机属性
        public Vector3 m_Position = Vector3.Zero;
        public Vector3 m_Front = -Vector3.UnitZ;
        public Vector3 m_Up = Vector3.UnitY;
        public Vector3 m_Right;
        //欧拉角
        float m_Yaw = -90.0f;  //偏航角
        float m_Pitch = 0.0f;  //俯仰角
        //相机设置
        float MovementSpeed = 2.5f;
        float MouseSensitivity = 0.1f;
        float Zoom = 45.0f;

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(m_Position, m_Position + m_Front, m_Up);
        }

        //处理键盘输入
        public void ProcessKeyboard(Camera_Movement direction, float deltaTime)
        {
            float velocity = MovementSpeed * deltaTime;
            if (direction == Camera_Movement.FORWARD)
                m_Position += m_Front * velocity;
            if (direction == Camera_Movement.BACKWARD)
                m_Position -= m_Front * velocity;
            if (direction == Camera_Movement.LEFT)
                m_Position -= m_Right * velocity;
            if (direction == Camera_Movement.RIGHT)
                m_Position += m_Right * velocity;
            if (direction == Camera_Movement.UP)
                m_Position += m_Up * velocity;
            if (direction == Camera_Movement.DOWN)
                m_Position -= m_Up * velocity;
        }

        //处理鼠标输入
        public void ProcessMouseMovement(float xoffset, float yoffset, bool constrainPitch = true)
        {
            xoffset *= MouseSensitivity;
            yoffset *= MouseSensitivity;

            m_Yaw += xoffset;
            m_Pitch += yoffset;

            // Make sure that when pitch is out of bounds, screen doesn't get flipped
            // 确保当俯仰角度不会超出范围
            if (constrainPitch)
            {
                if (m_Pitch > 89.0f)
                    m_Pitch = 89.0f;
                if (m_Pitch < -89.0f)
                    m_Pitch = -89.0f;
            }

            // Update Front, Right and Up Vectors using the updated Euler angles
            UpdateCameraVectors();
        }

        //重置到初始状态
        public void Reset()
        {
            m_Position = Vector3.Zero;
            m_Front = -Vector3.UnitZ;
            m_Yaw = -90.0f;
            m_Pitch = 0.0f;

            UpdateCameraVectors();
        }

        // Calculates the front vector from the Camera's (updated) Euler Angles
        private void UpdateCameraVectors()
        {
            Vector3 front = Vector3.Zero;
            front.X = (float) Math.Cos(MathHelper.DegreesToRadians(m_Yaw)) *
                      (float) Math.Cos(MathHelper.DegreesToRadians(m_Pitch));

            front.Y = (float) Math.Sin(MathHelper.DegreesToRadians(m_Pitch));

            front.Z = (float)Math.Sin(MathHelper.DegreesToRadians(m_Yaw)) *
                      (float)Math.Cos(MathHelper.DegreesToRadians(m_Pitch));

            m_Front = front.Normalized();
            m_Right = Vector3.Cross(m_Front, m_Up);
        }
    }
}
