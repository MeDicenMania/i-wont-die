using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData1", menuName = "Player Data")]
public class PlayerData1 : ScriptableObject
{
    [Header("Movimiento")]


    [Header("Run")]
    public float maxVelocity = 20f;
    public float runAcceleration;
    public float runAccelAmount=3f;
    public float runDecceleration;
    public float runDecclAmount =20f;
    public float accelAair = 5f;
    public float deccAir = 2f;
    public float initialSpeed = 13;

    [Header("Jump")]
    public float jumpForce;
    public float fallmultiplier = 2f;
    public float lowjumpmultiplier = 9f;

    [Header("WallJump")]
    public Vector2 wallJumpForce = new Vector2(10, 10);
    public float WallJumpmult = 1.4f;

    [Header("WallSlide")]
    public float walllSlideSpeed = 2.5f;

    [Header("Assists")]
    public float coyote = 0.12f;
    public float jumpBuffer = 0.12f;


    [Header("Special Moves")]


    [Header("Slide")]
    public float slideSize;
    public float normalSize;
    public float slideForce =20f;
    public float slideSpeed = 0f;
    public Vector2 slideJumpForce = new Vector2(5, 10);
    public float slideJumpmult = 1.5f;

    [Header("Grapple")]
    public float maxGrappleDistance;
    

}
