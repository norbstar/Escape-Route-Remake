using System;
using System.Collections.Generic;

using UnityEngine;

public class Property : MonoBehaviour
{
    // [SerializeField] Texture btnTexture;

    public enum SignEnum
    {
        Positive,
        Negative
    }

    [Serializable]
    public class MoveAxisValue
    {
        public bool include;
        public bool isZero;
        public SignEnum nonZeroSign;
    }

    [Serializable]
    public class MoveValue
    {
        [ReadOnly]
        public bool include;
        public MoveAxisValue xAxis;
        public MoveAxisValue yAxis;
    }

    [SerializeField]
    public class BooleanValue
    {
        public bool include;
        public bool shouldBeTrue;
    }

    public enum FooEnum
    {
        Alpha,
        Beta,
        Delta
    }

    private FooEnum foo;

    public FooEnum Foo { get => foo; set => foo = value; }

    // public Texture BtnTexture => btnTexture;

    // public bool isCrouching;
    public BooleanValue isCrouching = new BooleanValue();

    // public bool isGrabbable;
    public BooleanValue isGrabbable = new BooleanValue();

    // public bool isTraversable;
    public BooleanValue isTraversable = new BooleanValue();

    // public bool isHolding;
    public BooleanValue isHolding = new BooleanValue();

    // public bool isDashing;
    public BooleanValue isDashing = new BooleanValue();

    // public bool isSliding;
    public BooleanValue isSliding = new BooleanValue();

    // public bool isInputSuspended;
    public BooleanValue isInputSuspended = new BooleanValue();

    // public bool isBlockedTop;
    public BooleanValue isBlockedTop = new BooleanValue();
    
    // public bool isBlockedRight;
    public BooleanValue isBlockedRight = new BooleanValue();

    // public bool isGrounded;
    public BooleanValue isGrounded = new BooleanValue();

    // public bool isBlockedLeft;
    public BooleanValue isBlockedLeft = new BooleanValue();

    public MoveValue moveValue;

    public List<BooleanValue> conditions;
}
