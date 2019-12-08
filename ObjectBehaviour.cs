using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGame.Behaviour
{
    public class ObjectBehaviour
    {
        public ObjectBehaviour()
        {
            Screen.objectBehaviours.Add(this);
            ElementInList = Screen.objectBehaviours.Count - 1;
            Awake();
            Start();
        }

        public void Destroy()
        {
            Screen.objectBehaviours.RemoveAt(ElementInList);
            OnDelete();
        }

        int ElementInList = -1;
        List<dynamic> Components = new List<dynamic>();
        public GameObject gameObject = new GameObject();
        public bool WasStartCalled = false;
        public bool Visible = true;

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void OnDelete()
        {

        }

        public dynamic GetComponent<T>()
        {
            Type componentType = typeof(T);
            foreach (dynamic component in Components)
            {
                if (((ObjectHandle)component).Unwrap().GetType() == componentType)
                {
                    return component;
                }
            }
            dynamic newComponent = Activator.CreateInstance(componentType);
            Components.Add(newComponent);
            return newComponent;
        }

        void ObjectBehaviourCallUpdateThread()
        {
            Stopwatch sw = new Stopwatch();
            bool SetReset = false;
            while (true)
            {
                while(Screen.FrameUpdate) {
                    if (!SetReset)
                    {
                        sw.Restart();
                        Update();
                        sw.Stop();
                    }
                    SetReset = true;
                }
                SetReset = false;
            }
        }

        void ObjectBehaviourCallLateUpdateThread()
        {
            Stopwatch sw = new Stopwatch();
            bool SetReset = false;
            while (true)
            {
                while (Screen.FrameLateUpdate)
                {
                    if (!SetReset)
                    {
                        sw.Restart();
                        Update();
                        sw.Stop();
                    }
                    SetReset = true;
                }
                SetReset = false;
            }
        }
    }

    public struct Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public static Vector2 zero
        {
            get
            {
                Vector2 vector2 = new Vector2(0f, 0f);
                return vector2;
            }
        }

        public static Vector2 down
        {
            get
            {
                Vector2 vector2 = new Vector2(0f, 1f);
                return vector2;
            }
        }

        public static Vector2 up
        {
            get
            {
                Vector2 vector2 = new Vector2(0f, -1f);
                return vector2;
            }
        }

        public static Vector2 left
        {
            get
            {
                Vector2 vector2 = new Vector2(-1f, 0f);
                return vector2;
            }
        }

        public static Vector2 right
        {
            get
            {
                Vector2 vector2 = new Vector2(1f, 0f);
                return vector2;
            }
        }

        public static Vector2 MoveTowards(Vector2 current, Vector2 target, int maxDistanceDelta)
        {
            Vector2 vector2 = target - current;
            float magnitude = vector2.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0.0)
                return target;
            return current + vector2 / magnitude * maxDistanceDelta;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Mathf.Clamp(t, 0f, 1f);
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }

        public static float SqrMagnitude(Vector2 a)
        {
            return (float)((double)a.x * (double)a.x + (double)a.y * (double)a.y);
        }

        public float SqrMagnitude()
        {
            return (float)((double)this.x * (double)this.x + (double)this.y * (double)this.y);
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            return (float)Math.Acos(Mathf.Clamp(Vector2.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
        }

        public static int Dot(Vector2 lhs, Vector2 rhs)
        {
            return (int)((double)lhs.x * (double)rhs.x + (double)lhs.y * (double)rhs.y);
        }

        public Vector2 normalized
        {
            get
            {
                Vector2 vector2 = new Vector2(x, y);
                vector2.Normalize();
                return vector2;
            }
        }

        public void Normalize()
        {
            float magnitude = this.magnitude;
            if ((double)magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = Vector2.zero;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2;
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector2))
                return false;
            Vector2 vector2 = (Vector2)other;
            if (x.Equals(vector2.x))
                return y.Equals(vector2.y);
            return false;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }

        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2((a.x * d), (a.y * d));
        }

        public static Vector2 operator *(float d, Vector2 a)
        {
            return new Vector2((a.x * d), (a.y * d));
        }

        public static Vector2 operator /(Vector2 a, float d)
        {
            return new Vector2((a.x / d), (a.y / d));
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return (double)SqrMagnitude(lhs - rhs) < 9.99999943962493E-11;
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return (double)SqrMagnitude(lhs - rhs) >= 9.99999943962493E-11;
        }
    }

    public static class Mathf
    {
        public static float Clamp(float value, float min, float max)
        {
            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }

        public static int RoundToInt(float value)
        {
            return (int)Math.Round(value);
        }
    }

    public static class Time
    {
        public static float deltaTime = 0;
        public static readonly double fixedTime = Settings.FramesPerSecond / 1000;
    }

    public static class Input
    {
        private static ConsoleKey consoleKey;

        /// <summary>
        /// Don't use it outside making separate thread in Init at GameLogic
        /// </summary>
        public static void RegisterKeys()
        {
            while (true)
            {
                consoleKey = Console.ReadKey(true).Key;
            }
        }

        public static bool GetKeyDown(ConsoleKey key)
        {
            bool toReturn = consoleKey == key;
            if (toReturn) consoleKey = ConsoleKey.Process;
            return toReturn;
        }
    }

}
