using UnityEngine;

/// <summary>
/// 사용자의 입력을 받아 플레이어 캐릭터를 조종, 관리하는 클래스
/// </summary>
public class PlayerController : Photon.PunBehaviour
{
    /// <summary>
    /// 플레이어의 현재 애니메이션 상태
    /// </summary>
    private PlayerAniState state = PlayerAniState.Idle;

    /// <summary>
    /// 플레이어가 스턴에 걸렸을 때 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string stunSound;

    /// <summary>
    /// 플레이어의 발소리 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string walkingSound;

    FMOD.Studio.EventInstance walkingEvent;
    FMOD.Studio.PLAYBACK_STATE walkingSoundState;

    // Move
    /// <summary>
    /// 포톤으로 움직일 때 True, False 일 때 씬에서 바로 재생 후 제어 가능
    /// </summary>
    public bool photonMove = true;

    /// <summary>
    /// 캐릭터의 조종 가능 여부
    /// </summary>
    public bool isControlable = true;

    /// <summary>
    /// 캐릭터의 기절 여부
    /// </summary>
    public bool isStun = false;

    /// <summary>
    /// 가속도
    /// </summary>
    public float accSpeed = 15.0f;

    /// <summary>
    /// 최대 속도
    /// </summary>
    public float maxSpeed = 10.0f;

    /// <summary>
    /// 리지드바디의 속도
    /// </summary>
    private Vector3 Velocity {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    /// <summary>
    /// 사용자가 입력한 방향을 담은 벡터
    /// </summary>
    private Vector2 inputAxis;

    /// <summary>
    /// 사용자가 가고자 하는 방향 벡터
    /// </summary>
    private Vector3 targetDirection;

    private Vector2 axisVelocity;
    

    /// <summary>
    /// 플레이어 재생성 대기 시간
    /// </summary>
    public float respawnTime = 6.0f;

    /// <summary>
    /// 플레이어 기절 시간
    /// </summary>
    public float stunTime = 2.0f;


    // Turn
    private Ray mouseRay;
    private RaycastHit mouseHit;

    /// <summary>
    /// 플레이어 발 밑에 하얀색 원 (자신만 활성화)
    /// </summary>
    public GameObject ring;

    /// <summary>
    /// 기절 이펙트
    /// </summary>
    public GameObject stunEfx;


    // 내부 컴포넌트
    private PlayerStat stat;
    private FireworkExecuter executer;
    private Rigidbody rb;
    private Animator anim;

    // 외부 컴포넌트
    private Transform cameraT;
    private Camera cam;


    int groundMask;


    // 임시


    void Start () {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        stat = GetComponent<PlayerStat>();
        executer = GetComponent<FireworkExecuter>();
        anim = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");
        //walkingEvent = FMODUnity.RuntimeManager.CreateInstance(walkingSound);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingEvent, transform, rb);

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
            }
        }
        
	}

    /// <summary>
    /// 사용자의 방향 입력을 받아 Vector2에 저장하고 방향 벡터를 만들어 저장합니다.
    /// </summary>
    void GetInput()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        //targetDirection = (inputAxis.y * Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized + inputAxis.x * cam.transform.right).normalized;
        targetDirection = (inputAxis.y * Vector3.forward + inputAxis.x * Vector3.right).normalized;
    }

    /// <summary>
    /// 애니메이션 파라미터들을 업데이트 합니다.
    /// </summary>
    void ApplyAnimatorParams()
    {
        anim.SetInteger("AniNum", (int)state);
        anim.SetFloat("MoveX", inputAxis.x, 0.1f, Time.fixedDeltaTime);
        anim.SetFloat("MoveY", inputAxis.y, 0.1f, Time.fixedDeltaTime);
    }

    /// <summary>
    /// Integer형 애니메이션 파라미터를 설정합니다.
    /// </summary>
    /// <param name="param">파라미터 이름</param>
    /// <param name="val">값</param>
    public void SetAnimParam(string param, int val)
    {
        anim.SetInteger(param, val);
    }

    /// <summary>
    /// 캐릭터를 지정한 위치로 이동시킵니다.
    /// </summary>
    /// <param name="targetPosition">이동할 위치</param>
    void Move(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }

    /// <summary>
    /// 사용자가 입력한 방향으로 속도를 더합니다.
    /// </summary>
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

    /// <summary>
    /// 마우스 방향으로 캐릭터를 회전합니다.
    /// </summary>
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

    /// <summary>
    /// 캐릭터의 속도를 초기화 하고 특정 방향으로 힘을 줍니다.
    /// </summary>
    /// <param name="force">힘</param>
    [PunRPC]
    public void Pushed(Vector3 force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
    }

    /// <summary>
    /// 떨어지는지 체크합니다.
    /// </summary>
    void CheckFall()
    {
        if(transform.position.y < 3f)
        {
            photonView.RPC("Fall", PhotonTargets.All);
        }
    }

    /// <summary>
    /// 플레이어의 생명을 하나깎고 리스폰 대기를 합니다.
    /// </summary>
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

        ChangeState(PlayerAniState.Fall);
        Invoke("StandBy", 2.0f);
        //photonView.RPC("ChangeFirework", PhotonTargets.All, 0, 0);
        executer.ChangeFirework(0, 0);
    }

    /// <summary>
    /// 지정한 위치로 캐릭터를 순간 이동시킵니다.
    /// </summary>
    /// <param name="movePos">이동할 위치</param>
    [PunRPC]
    public void Teleport(Vector3 movePos)
    {
        transform.position = movePos;
    }

    /// <summary>
    /// 리스폰 대기
    /// </summary>
    private void StandBy()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        transform.position = Vector3.zero;
        
    }

    /// <summary>
    /// 기절했을 때
    /// </summary>
    public void Stun()
    {
        isStun = true;
        ChangeState(PlayerAniState.Stun);

        Invoke("StunRecovery", stunTime);

        FMODUnity.RuntimeManager.PlayOneShot(stunSound);
    }

    /// <summary>
    /// 기절에서 회복 했을 때
    /// </summary>
    private void StunRecovery()
    {
        isStun = false;
        //stat.HPReset();
        stat.curHP = 60;
        ChangeState(PlayerAniState.Idle);
    }

    /// <summary>
    /// 캐릭터를 리스폰합니다.
    /// </summary>
    private void Respawn()
    {
        stat.HPReset();
        GameManagerPhoton._instance.RespawnPlayer(transform);
        stat.onStage = true;
        isControlable = true;
        rb.WakeUp();
        rb.useGravity = true;
        ChangeState(PlayerAniState.Idle);
    }

    private void MoveSound()
    {

        walkingEvent.getPlaybackState(out walkingSoundState);

        if (state == PlayerAniState.Idle)
        {
            if (Velocity.sqrMagnitude > 0.01f)
            {
                print(Velocity.sqrMagnitude);
                if (walkingSoundState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
                {
                    walkingEvent.start();
                }
            }
        }
        else
        {
            if(walkingSoundState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                walkingEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    /// <summary>
    /// 캐릭터의 애니메이션 상태를 교체합니다.
    /// </summary>
    /// <param name="newState">새로운 애니메이션 상태</param>
    public void ChangeState(PlayerAniState newState)
    {
        StateExit(state);
        state = newState;
        StateEnter(state);
    }

    /// <summary>
    /// 특정 애니메이션 상태로 들어갈 때 처리를 합니다.
    /// </summary>
    /// <param name="newState">새 애니메이션 상태</param>
    private void StateEnter(PlayerAniState newState)
    {
        switch (newState)
        {
            case PlayerAniState.Idle:
                break;
            case PlayerAniState.Attack:
                break;
            case PlayerAniState.Stun:
                stunEfx.SetActive(true);
                break;
            case PlayerAniState.Fall:
                anim.SetFloat("MoveX", 0);
                anim.SetFloat("MoveY", 0);
                break;
        }
    }

    /// <summary>
    /// 특정 애니메이션 상태에서 나올 때 처리를 합니다.
    /// </summary>
    /// <param name="preState">이전 애니메이션 상태</param>
    private void StateExit(PlayerAniState preState)
    {
        switch (preState)
        {
            case PlayerAniState.Idle:
                break;
            case PlayerAniState.Attack:
                if (executer.curFirework.fwType == FireworkType.Butterfly)
                {
                    executer.charging = false;
                    anim.SetBool("Charging", false);
                    UIManager._instance.chargingUI.SetActivate(false);
                }

                executer.replaceable = true;
                if (executer.newFirework == null)
                    executer.CheckRunOutAmmo();
                else
                    executer.CheckFireworkChanged();
                break;
            case PlayerAniState.Stun:
                stunEfx.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 특정 애니메이션 상태일 때 지속적으로 실행 합니다.
    /// </summary>
    private void StateUpdate()
    {
        switch (state)
        {
            case PlayerAniState.Idle:
                AddVelocity();
                TurnToMouse();
                break;
            case PlayerAniState.Attack:
                if (executer.curFirework.fwType == FireworkType.Butterfly)
                {
                    if(executer.charging && !Input.GetMouseButton(0))
                    {
                        executer.charging = false;
                        anim.SetBool("Charging", false);
                    }
                    else if(!executer.charging && Input.GetMouseButton(0))
                    {
                        anim.SetBool("Charging", true);
                    }

                    TurnToMouse();
                }
                break;
            case PlayerAniState.Stun:
                break;
        }
    }

    /// <summary>
    /// 애니메이션 이벤트를 실행합니다.
    /// </summary>
    /// <param name="eventName"></param>
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
                break;
            case "Move Sound":
                FMODUnity.RuntimeManager.PlayOneShot(walkingSound);
                break;
        }
    }
}
