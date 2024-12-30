using UnityEngine;

namespace Tests.State
{
    public abstract class State : MonoBehaviour
    {
        public PlayerEssentials Essentials { get; set; }

        public virtual void OnBlockedTop() { }

        public virtual void OnNotBlockedTop() { }

        public virtual void OnBlockedRight() { }

        public virtual void OnNotBlockedRight() { }

        public virtual void OnGrounded() { }

        public virtual void OnNotGrounded() { }

        public virtual void OnBlockedLeft() { }

        public virtual void OnNotBlockedLeft() { }
    }
}
