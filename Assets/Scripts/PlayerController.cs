using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Photon.PunBehaviour {

    private PlayerAniState state = PlayerAniState.Idle;

    [FMODUnity.EventRef]
    public string hitSound;
    [FMODUnity.EventRef]
    public string stunSound;

    // Move
    public bool photonMove = true;
    public bool isMine = false;
    public bool isControlable = true;
    public bool isStun = false;
    public float accSpeed = 15.0f;
    public float maxSpeed = 10.0f;
    private Vector3 Velocity {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }
    private Vector2 inputAxis;
    private Vector3 targetDirection;
    //private Vector2 smoothAxis = Vector2.zero;
    private Vector2 axisVelocity;

    // 내부 컴포넌트
    private PlayerStat stat;
    private FireworkExecuter executer;
    private Rigidbody rb;
    private Animator anim;

    // 외부 컴포넌트
    private Transform cameraT;
    private Camera cam;

    // Turn
    private Ray mouseRay;
    private RaycastHit mouseHit;

    // Respawn
    public float respawnTime = 6.0f;

    // Stun
    public float stunTime = 2.0f;


    public GameObject ring;

    int groundMask;

    // 임시


    void Start () {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        stat = GetComponent<PlayerStat>();
        executer = GetComponent<FireworkExecuter>();
        anim = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");

        GameManagerPhoton._instance.cameraController.FindPlayers();


        if (photonView.isMine)
        {
            ring.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        GameManagerPhoton._instance.cameraController.FindPlayers();
    }

    private void Update()
    {
        if (!stat.onStage) return;

        if (!photonMove || photonView.isMine)
        {
            if (isControlable && !isStun)
            {
                if (Input.GetMouseButtonDown(0) && state == PlayerAniState.Idle)
                {
                    executer.Trigger();
                }
            }

            CheckFall();
            ApplyAnimatorParams();
        }
        
    }

    void FixedUpdate () {
        if (!stat.onStage) return;

        if (!photonMove || photonView.isMine)
        {
            if (isControlable && !isStun)
            {
                GetInput();
                StateUpdate();
                ApplyAnimatorParams();
            }
        }
        
	}

    void GetInput()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        //targetDirection = (inputAxis.y * Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized + inputAxis.x * cam.transform.right).normalized;
        targetDirection = (inputAxis.y * Vector3.forward + inputAxis.x * Vector3.right).normalized;
    }

    void ApplyAnimatorParams()
    {
        anim.SetInteger("AniNum", (int)state);
        anim.SetFloat("MoveX", inputAxis.x, 0.1f, Time.fixedDeltaTime);
        anim.SetFloat("MoveY", inputAxis.y, 0.1f, Time.fixedDeltaTime);
    }

    public void SetAnimParam(string param, int val)
    {
        anim.SetInteger(param, val);
    }

    void Move(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }

    void AddVelocity()
    {
        // 입력값이 있다
        if (inputAxis.sqrMagnitude > 0)
        {
            Vector3 addVelocity = targetDirection * accSpeed * Time.fixedDeltaTime;
            float resultMag = (Velocity + addVelocity).magnitude;

            if (resultMag < maxSpeed || resultMag < Velocity.magnitude)
            {
                Velocity += addVelocity;
            }
        }
    }

    void TurnToMouse()
    {
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f, groundMask))
        {
            Vector3 toDir = Vector3.Scale( mouseHit.point - transform.position, new Vector3(1,0,1)).normalized;

            Quaternion targetRoation = Quaternion.LookRotation(toDir);
            transform.rotation = targetRoation;
        }
    }

    [PunRPC]
    public void Pushed(Vector3 force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);

        FMODUnity.RuntimeManager.PlayOneShot(hitSound);
    }

    void CheckFall()
    {
        if(transform.position.y < 3f)
        {
            photonView.RPC("Fall", PhotonTargets.All);
        }
    }

    // 떨어졌을 때
    [PunRPC]
    public void Fall()
    {
        stat.onStage = false;
        stat.LifeLoss();
        if(stat.life > 0)
        {
            Invoke("Respawn", respawnTime);
        }
        else
        {

        }
        Invoke("StandBy", 2.0f);
        photonView.RPC("ChangeFirework", PhotonTargets.All, 0, 0);
    }

    [PunRPC]
    public void Teleport(Vector3 movePos)
    {
        transform.position = movePos;
    }


    // 리스폰 대기
    private void StandBy()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        transform.position = Vector3.zero;
        
    }

    // 기절했을 때
    public void Stun()
    {
        isStun = true;
        ChangeState(PlayerAniState.Stun);

        Invoke("StunRecovery", stunTime);

        FMODUnity.RuntimeManager.PlayOneShot(stunSound);
    }

    // 스턴 회복
    public void StunRecovery()
    {
        isStun = false;
        //stat.HPReset();
        stat.curHP = 60;
        ChangeState(PlayerAniState.Idle);
    }

    // 리스폰
    public void Respawn()
    {
        stat.HPReset();
        GameManagerPhoton._instance.RespawnPlayer(transform);
        stat.onStage = true;
        rb.WakeUp();
        rb.useGravity = true;
    }

    public void ChangeState(PlayerAniState newState)
    {
        StateExit(state);
        state = newState;
        StateEnter(state);
    }

    private void StateEnter(PlayerAniState newState)
    {
        switch (newState)
        {
            case PlayerAniState.Idle:
                break;
            case PlayerAniState.Stun:
                break;
        }
    }

    private void StateExit(PlayerAniState preState)
    {
        switch (preState)
        {
            case PlayerAniState.Idle:
                break;
            case PlayerAniState.Stun:
                break;
        }
    }

    private void StateUpdate()
    {
        switch (state)
        {
            case PlayerAniState.Idle:
                
                AddVelocity();
                TurnToMouse();
                break;
            case PlayerAniState.Stun:
                break;
        }
    }

    public void OnAnimEvent(string eventName)
    {
        switch (eventName)
        {
            case "Attack Execute":
                if(!isStun)
                    executer.Execute();
                
                break;
            case "Attack End":
                if(!isStun)
                    ChangeState(PlayerAniState.Idle);
                executer.CheckRunOutAmmo();
                break;
        }
    }
}
