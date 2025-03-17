using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(BasePlayer))]
    public class PlayerStateActivation : MonoBehaviour
    {
        [SerializeField] private bool canMove, canJump, canShift, canFall, canDash, canSneak, canSlide, canGrab, canTraverse;

        public bool CanMove { get => canMove; set => canMove = value; }
        public bool CanJump { get => canJump; set => canJump = value; }
        public bool CanShift { get => canShift; set => canShift = value; }
        public bool CanFall { get => canFall; set => canFall = value; }
        public bool CanDash { get => canDash; set => canDash = value; }
        public bool CanSneak { get => canSneak; set => canSneak = value; }
        public bool CanSlide { get => canSlide; set => canSlide = value; }
        public bool CanGrab { get => canGrab; set => canGrab = value; }
        public bool CanTraverse { get => canTraverse; set => canTraverse = value; }
    }
}
