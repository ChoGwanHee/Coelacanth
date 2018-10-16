using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerModule;

/// <summary>
/// 사용자의 입력을 받아 플레이어 캐릭터를 조종, 관리하는 클래스
/// </summary>
public class PlayerController : Photon.PunBehaviour
{
    /// <summary>
    /// 플레이어의 현재 애니메이션 상태
    /// </summary>
    private PlayerAniState state = PlayerAniState.Idle;

    public PlayerAniState State
    {
        get { return state; }
    }

    /// <summary>
    /// 플레이어가 현재 가지고 있는 유틸 아이템
    /// </summary>
    public ItemBoxUtil utilItem;

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

    /// <summary>
    /// 플레이어가 맵 밖으로 떨어질 때 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string fallingSound;


    /// <summary>
    /// 캐릭터 목소리 에셋
    /// </summary>
    public CharacterVoicePack characterVoice;

    

    // Move
    [Header("Move")]
    /// <summary>
    /// 포톤으로 움직일 때 True, False 일 때 씬에서 바로 재생 후 제어 가능
    /// </summary>
    public bool photonMove = true;

    /// <summary>
    /// 캐릭터의 조종 가능 여부
    /// </summary>
    public bool isControlable = false;

    /// <summary>
    /// 캐릭터의 기절 여부
    /// </summary>
    public bool isStun = false;

    /// <summary>
    /// 캐릭터의 무적 여부
    /// </summary>
    public bool isUnbeatable = false;

    /// <summary>
    /// 가속도
    /// </summary>
    public float accSpeed = 15.0f;

    /// <summary>
    /// 최대 속도
    /// </summary>
    public float maxSpeed = 10.0f;

    /// <summary>
    /// 최대 속도 계수
    /// </summary>
    public float maxSpeedFactor = 1.0f;

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
    /// 떨어지고 있는지 여부
    /// </summary>
    private bool isFalling = false;

    /// <summary>
    /// 캐릭터가 떨어지기 시작하는 높이
    /// </summary>
    private float fallingHeight;
    
    [Space()]
    /// <summary>
    /// 플레이어 재생성 대기 시간
    /// </summary>
    public float respawnTime = 6.0f;

    /// <summary>
    /// 플레이어 기절 시간
    /// </summary>
    public float stunTime = 2.0f;

    /// <summary>
    /// 넉백 시간
    /// </summary>
    public float knockbackTime = 0.2f;

    /// <summary>
    /// 넉백 시작 후 지난 시간
    /// </summary>
    private float knockbackElapsedTime = 0.0f;

    /// <summary>
    /// 상호작용 가능한지 체크할 때 사용하는 반경
    /// </summary>
    private float interactionCheckRadius = 1.0f;

    /// <summary>
    /// 상호작용이 가능하다는 UI를 나타내기 위해 체크하는 시간 간격
    /// </summary>
    private float interactionCheckTime = 0.2f;
    

    /// <summary>
    /// 아이템을 들고 있는지 여부
    /// </summary>
    private bool isGrab = false;


    // Turn
    private Ray mouseRay;
    private RaycastHit mouseHit;


    /// <summary>
    /// 폭죽을 들 수 있는 손안의 본
    /// </summary>
    public Transform weaponPoint;

    /// <summary>
    /// 아이템을 들 수 있는 본
    /// </summary>
    public Transform itemPoint;

    /// <summary>
    /// 손에 붙일 수 있는 폭죽 프리팹 모음
    /// </summary>
    public FireworkHandObjectSet handObjectSet;

    /// <summary>
    /// 손에 붙어 있는 오브젝트
    /// </summary>
    private GameObject attachedHandObject;


    /// <summary>
    /// 플레이어 발 밑에 하얀색 원 (자신만 활성화)
    /// </summary>
    public GameObject ring;

    /// <summary>
    /// 기절 이펙트
    /// </summary>
    public GameObject stunEfx;

    /// <summary>
    /// 사망 이펙트
    /// </summary>
    public GameObject deadEfx_ref;


    // 내부 컴포넌트
    private PlayerStat stat;
    public PlayerStat Stat
    {
        get { return stat; }
    }
    private BuffController bc;
    public BuffController BC
    {
        get { return bc; }
    }
    private FireworkExecuter executer;
    public FireworkExecuter Executer
    {
        get { return executer; }
    }
    private Rigidbody rb;
    private CapsuleCollider col;
    private Animator anim;

    // 외부 컴포넌트
    private Camera cam;


    int groundMask;

    void Awake () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        cam = Camera.main;
        stat = GetComponent<PlayerStat>();
        bc = GetComponent<BuffController>();
        executer = GetComponent<FireworkExecuter>();
        anim = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");

        if (MapManager._instance != null)
            fallingHeight = MapManager._instance.fallingHeight;
        else
            fallingHeight = -0.1f;

        if (photonView.isMine)
        {
            ring.SetActive(true);
            StartCoroutine(CheckInteractionIndicator());
        }

        TurnToScreen();
    }

    private void Update()
    {
        if (!stat.onStage) return;

        if (!photonMove || photonView.isMine)
        {
            if (isControlable && !isStun)
            {
                if (state == PlayerAniState.Idle)
                {
                    if (!isGrab)
                    {
                        if (Input.GetMouseButtonDown(0))
                            executer.Trigger();

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Interaction();
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                            StartCoroutine(ItemUsingDelay(utilItem.aniNum));

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            ChangeState(PlayerAniState.Put);
                        }
                    }


                }
            }

            ApplyAnimatorParams();
        }
        
    }

    void FixedUpdate () {
        if (!stat.onStage) return;

        if (!photonMove || photonView.isMine)
        {
            StateUpdate();

            if (!isFalling)
            {
                CheckFalling();
            }
            else
            {
                CheckFall();
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

        // 카메라의 방향이 Z축을 바라보게 고정이 되어 있어서 사용할 필요가 없음 나중에 바뀌면 이걸로 바꿔야 됨
        //targetDirection = (inputAxis.y * Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized + inputAxis.x * cam.transform.right).normalized;
        targetDirection = (inputAxis.y * Vector3.forward + inputAxis.x * Vector3.right).normalized;
    }

    /// <summary>
    /// 애니메이션 파라미터들을 업데이트 합니다.
    /// </summary>
    void ApplyAnimatorParams()
    {
        anim.SetInteger("AniNum", (int)state);
        //anim.SetFloat("MoveX", inputAxis.x, 0.1f, Time.deltaTime);
        //anim.SetFloat("MoveY", inputAxis.y, 0.1f, Time.deltaTime);
        anim.SetFloat("MoveX", inputAxis.magnitude, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// 애니메이션 파라미터를 설정합니다.
    /// </summary>
    /// <param name="param">파라미터 이름</param>
    /// <param name="val">값</param>
    public void SetAnimParam(string param, int val)
    {
        anim.SetInteger(param, val);
    }

    public void SetAnimParam(string param, bool val)
    {
        anim.SetBool(param, val);
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

            if (resultMag < maxSpeed * maxSpeedFactor || resultMag < Velocity.magnitude)
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
            Vector3 toDir = Vector3.Scale(mouseHit.point - transform.position, new Vector3(1, 0, 1)).normalized;

            Quaternion targetRoation = Quaternion.LookRotation(toDir);
            transform.rotation = targetRoation;
        }
    }

    /// <summary>
    /// 카메라 방향으로 캐릭터를 회전합니다.
    /// </summary>
    public void TurnToScreen()
    {
        if (!photonView.isMine) return;

        // 카메라의 방향이 Z축을 바라보게 고정이 되어 있어서 사용할 필요가 없음 나중에 바뀌면 이걸로 바꿔야 됨
        //Vector3 toDir = Vector3.Scale(cam.transform.position - transform.position , new Vector3(1, 0, 1)).normalized;
        Vector3 toDir = new Vector3(0, 0, -1);

        transform.rotation = Quaternion.LookRotation(toDir);
    }

    /// <summary>
    /// 캐릭터의 속도를 초기화 하고 특정 방향으로 힘을 줍니다.
    /// </summary>
    /// <param name="force">힘</param>
    [PunRPC]
    public void Pushed(Vector3 force)
    {
        if (isUnbeatable) return;

        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
        ChangeState(PlayerAniState.KnockBack);
    }

    /// <summary>
    /// 스테이지 밖으로 떨어지고 있는지 체크합니다.
    /// </summary>
    void CheckFalling()
    {
        if(transform.position.y <= fallingHeight)
        {
            ChangeState(PlayerAniState.Fall);
        }
    }

    /// <summary>
    /// 스테이지 밖으로 떨어졌는지 체크합니다.
    /// </summary>
    void CheckFall()
    {
        if(transform.position.y < fallingHeight-6.4f)
        {
            photonView.RPC("Fall", PhotonTargets.All);
        }
    }

    /// <summary>
    /// 플레이어가 떨어졌을 때 처리를 합니다.
    /// </summary>
    [PunRPC]
    public void Fall()
    {
        col.enabled = false;
        stat.onStage = false;
        stat.KillScoring();
        bc.RemoveAllBuff();

        // 보내는 정보   = 정보 : 플레이어 : 상태 : 라이프차감
        // 받는 정보      = 정보 : 플레이어 : 상태 : 보유라이프
        ServerManager.Send(string.Format("FALL:{0}:{1}:{2}", true, PlayerAniState.Fall, respawnTime));
        Invoke("Respawn", respawnTime);
        executer.ChangeFirework(0, 0);

        Vector3 genPos = transform.position;
        GameObject.Instantiate(deadEfx_ref, genPos, Quaternion.identity);
        PublicPlayVoiceSound("Falling");
        FMODUnity.RuntimeManager.PlayOneShot(fallingSound);

        if(photonView.isMine)
            GameManagerPhoton._instance.cameraController.Shake(4, 0.5f);

        StandBy();
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
        transform.position = new Vector3(0f,0f,-20f);
    }

    /// <summary>
    /// 기절 이펙트를 설정합니다.
    /// </summary>
    /// <param name="active">활성화 여부</param>
    [PunRPC]
    private void SetStunEfx(bool active)
    {
        if (active)
        {
            stunEfx.SetActive(true);
        }
        else
        {
            stunEfx.SetActive(false);
        }
    }

    /// <summary>
    /// 기절에서 회복 했을 때
    /// </summary>
    private void StunRecovery()
    {
        //stat.HPReset();
        stat.curHP = 60;
        ChangeState(PlayerAniState.Idle);
    }

    /// <summary>
    /// 캐릭터를 리스폰합니다.
    /// </summary>
    private void Respawn()
    {
        if (photonView.isMine)
        {
            GameManagerPhoton._instance.RespawnPlayer(transform);
        }
        stat.HPReset();
        stat.onStage = true;
        if(GameManagerPhoton._instance.IsPlaying)
            isControlable = true;
        rb.WakeUp();
        rb.useGravity = true;
        col.enabled = true;
        ChangeState(PlayerAniState.Idle);
    }

    /// <summary>
    /// 스포트라이트 받았을 때 처리를 합니다.
    /// </summary>
    public void Spotlight()
    {
        CancelInvoke();
        StopAllCoroutines();
        bc.RemoveAllBuff();

        if(photonView.isMine && isGrab && state != PlayerAniState.Put)
        {
            PutUtilItem();
        }

        // 발 밑에 땅이 있으면
        if (Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, 3.0f, groundMask, QueryTriggerInteraction.Ignore))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ChangeState(PlayerAniState.Idle);
        }
        // 발 밑에 땅이 없으면
        else
        {
            // 즉시 리스폰
            Respawn();
        }
        isControlable = false;
        inputAxis = Vector2.zero;
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);

        TurnToScreen();
    }

    /// <summary>
    /// 캐릭터의 목소리를 재생합니다.
    /// </summary>
    /// <param name="soundType">재생할 목소리 종류</param>
    public void PlayVoiceSound(string soundType)
    {
        if (characterVoice == null || !photonView.isMine) return;

        switch (soundType)
        {
            case "Common":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.commonFireVoice);
                break;
            case "Fountain":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.fountainVoice);
                break;
            case "Rocket":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.rocketVoice);
                break;
            case "Party":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.partyVoice);
                break;
            case "Charging":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.chargingVoice);
                break;
            case "Hit":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.hitVoice);
                break;
            case "Stun":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.stunVoice);
                break;
            case "Falling":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.fallingVoice);
                break;
            case "Victory":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.victoryVoice);
                break;
        }
    }

    /// <summary>
    /// 캐릭터 소유와 상관없이 캐릭터의 목소리를 재생합니다.
    /// </summary>
    /// <param name="soundType">재생할 목소리 종류</param>
    public void PublicPlayVoiceSound(string soundType)
    {
        if (characterVoice == null) return;

        switch(soundType)
        {
            case "Common":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.commonFireVoice);
                break;
            case "Fountain":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.fountainVoice);
                break;
            case "Rocket":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.rocketVoice);
                break;
            case "Party":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.partyVoice);
                break;
            case "Charging":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.chargingVoice);
                break;
            case "Hit":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.hitVoice);
                break;
            case "Stun":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.stunVoice);
                break;
            case "Falling":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.fallingVoice);
                break;
            case "Victory":
                FMODUnity.RuntimeManager.PlayOneShot(characterVoice.victoryVoice);
                break;
        }
    }

    /// <summary>
    /// 캐릭터의 애니메이션 상태를 교체합니다.
    /// </summary>
    /// <param name="newState">새로운 애니메이션 상태</param>
    public void ChangeState(PlayerAniState newState)
    {
        if (state == newState) return;
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
                anim.SetFloat("MoveX", 0);
                anim.SetFloat("MoveY", 0);
                inputAxis = Vector2.zero;
                break;
            case PlayerAniState.Attack:
                break;
            case PlayerAniState.KnockBack:
                inputAxis = Vector2.zero;
                knockbackElapsedTime = 0.0f;
                break;
            case PlayerAniState.Stun:
                photonView.RPC("SetStunEfx", PhotonTargets.All, true);
                isStun = true;
                FMODUnity.RuntimeManager.PlayOneShot(stunSound);
                PlayVoiceSound("Stun");
                Invoke("StunRecovery", stunTime);
                if(isGrab)
                {
                    PutUtilItem();
                }
                break;
            case PlayerAniState.Fall:
                anim.SetInteger("AniNum", (int)PlayerAniState.Fall);
                anim.SetFloat("MoveX", 0);
                anim.SetFloat("MoveY", 0);
                inputAxis = Vector2.zero;
                isFalling = true;
                if (isGrab)
                {
                    PutUtilItem(true);
                }
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

            case PlayerAniState.KnockBack:

                break;
            case PlayerAniState.Put:
                PutUtilItem();
                break;

            case PlayerAniState.Stun:
                photonView.RPC("SetStunEfx", PhotonTargets.All, false);
                isStun = false;
                break;

            case PlayerAniState.Fall:
                isFalling = false;
                break;
            case PlayerAniState.Emotion:
                anim.SetInteger("SubAniNum", 0);
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
                if (isControlable && !isStun)
                {
                    GetInput();
                    AddVelocity();
                    TurnToMouse();
                }
                break;
            case PlayerAniState.Attack:
                if (isControlable && !isStun)
                {
                    GetInput();
                    if (executer.curFirework.fwType == FireworkType.Butterfly)
                    {
                        TurnToMouse();
                    }
                }
                break;
            case PlayerAniState.KnockBack:
                knockbackElapsedTime += Time.fixedDeltaTime;
                if(knockbackElapsedTime >= knockbackTime)
                {
                    ChangeState(PlayerAniState.Idle);
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
                if(!isStun && state != PlayerAniState.KnockBack)
                    ChangeState(PlayerAniState.Idle);
                break;

            case "Move Sound":
                FMODUnity.RuntimeManager.PlayOneShot(walkingSound);
                break;

            case "Play Start Sound":
                FMODUnity.RuntimeManager.PlayOneShot(executer.curFirework.startSound);
                break;

            case "Lift End":
                ChangeState(PlayerAniState.Idle);
                break;
            case "Put End":
                ChangeState(PlayerAniState.Idle);
                break;
        }
    }

    /// <summary>
    /// 손에 들 수 있는 폭죽인지 체크
    /// </summary>
    /// <param name="newFirework"></param>
    public void CheckHandObject(Firework newFirework)
    {
        switch (newFirework.fwType)
        {
            case FireworkType.Roman:
            case FireworkType.Fountain:
            case FireworkType.Rocket:
            case FireworkType.Party:
                Destroy(attachedHandObject);
                AttachToHand(newFirework.fwType);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 손에 폭죽 붙이기
    /// </summary>
    /// <param name="num">붙일 폭죽 번호</param>
    public void AttachToHand(FireworkType type)
    {
        switch (type)
        {
            case FireworkType.Roman:
                attachedHandObject = Instantiate(handObjectSet.objects[0], weaponPoint);
                break;
            case FireworkType.Fountain:
                attachedHandObject = Instantiate(handObjectSet.objects[1], weaponPoint);
                break;
            case FireworkType.Rocket:
                attachedHandObject = Instantiate(handObjectSet.objects[2], weaponPoint);
                break;
            case FireworkType.Party:
                attachedHandObject = Instantiate(handObjectSet.objects[3], weaponPoint);
                break;
        }
    }

    /// <summary>
    /// 플레이어 주변에 상호작용 가능한 오브젝트가 있는지 확인하고 있으면 작동합니다.
    /// </summary>
    private void Interaction()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, interactionCheckRadius, LayerMask.GetMask("InteractionObject", "Item"));
        IInteractable closestObj = null;
        float closestSqrmag = 9999f;

        for(int i=0; i<cols.Length; i++)
        {
            IInteractable obj = cols[i].GetComponent<IInteractable>();

            if (obj != null && obj.IsInteractable())
            {
                float curSqrmag = (cols[i].transform.position - transform.position).sqrMagnitude;
                if (closestSqrmag > curSqrmag)
                {
                    closestObj = obj;
                    closestSqrmag = curSqrmag;
                }
            }
        }

        if(closestObj != null)
            closestObj.Interact(this);
    }

    [PunRPC]
    private void LiftUtilItem(int index1, int index2)
    {
        if(index2 == -1)
        {
            Debug.LogError("인덱스 오류");
            return;
        }
        utilItem = GameManagerPhoton._instance.itemManager.itemBoxPool[index1][index2] as ItemBoxUtil;
        isGrab = true;
        anim.SetInteger("SubAniNum", 1);
        ChangeState(PlayerAniState.Lift);
    }

    [PunRPC]
    private void PutUtilItem(bool remove = false)
    {
        isGrab = false;
        anim.SetInteger("SubAniNum", 0);

        if (utilItem == null) return;

        GameManagerPhoton._instance.photonView.RPC("RemoveItemBoxFollower", PhotonTargets.All, utilItem.poolIndex1, utilItem.poolIndex2, utilItem.transform.position);
        if (remove)
        {
            GameManagerPhoton._instance.photonView.RPC("DeactivateItemBox", PhotonTargets.All, utilItem.poolIndex1, utilItem.poolIndex2);
        }
        
        utilItem = null;
    }

    public IEnumerator ItemUsingDelay(int subAniNum)
    {
        float elapsedTime = 0.0f;
        float delayTime = (utilItem.item as UtilItem).delayTime;
        bool wait = true;

        anim.SetInteger("SubAniNum", subAniNum);
        ChangeState(PlayerAniState.Consume);
        while(wait)
        {
            if(Input.GetMouseButtonUp(0) || state != PlayerAniState.Consume)
            {
                if(isGrab)
                    anim.SetInteger("SubAniNum", 1);
                wait = false;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= delayTime)
                {
                    wait = false;
                    utilItem.Use(this);
                }
                else
                {
                    yield return null;
                }
            }
        }
        ChangeState(PlayerAniState.Idle);
    }

    public IEnumerator CheckInteractionIndicator()
    {
        IInteractable closestObj = null;
        ItemBoxUtil outlinedObj = null;

        while (true)
        {
            closestObj = null;

            if (state == PlayerAniState.Idle && !isGrab)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position, interactionCheckRadius, LayerMask.GetMask("InteractionObject", "Item"));
                float closestSqrmag = 9999f;
                int closestIndex = -1;

                for (int i = 0; i < cols.Length; i++)
                {
                    IInteractable obj = cols[i].GetComponent<IInteractable>();

                    if (obj != null && obj.IsInteractable())
                    {
                        float curSqrmag = (cols[i].transform.position - transform.position).sqrMagnitude;
                        if (closestSqrmag > curSqrmag)
                        {
                            closestObj = obj;
                            closestIndex = i;
                            closestSqrmag = curSqrmag;
                        }
                    }
                }

                if(closestObj != null)
                {
                    UIManager._instance.eButton.SetActivate(true, cols[closestIndex].transform, closestObj.GetButtonType());

                    if(outlinedObj != null)
                    {
                        outlinedObj.SetOutline(false);
                    }

                    ItemBoxUtil itemBox = cols[closestIndex].GetComponent<ItemBoxUtil>();
                    if (itemBox != null)
                    {
                        outlinedObj = itemBox;
                        outlinedObj.SetOutline(true);
                    }
                    
                }
            }
            
            if (closestObj == null)
            {
                UIManager._instance.eButton.SetActivate(false);

                if(outlinedObj != null)
                {
                    outlinedObj.SetOutline(false);
                    outlinedObj = null;
                }
            }
            yield return new WaitForSeconds(interactionCheckTime);
        }
        
    }
}
