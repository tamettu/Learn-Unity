using UnityEngine;
using FSM;
using StateMachine = FSM.StateMachine;
public class PlayerMove : MonoBehaviour, IEntity
{
    #region Different State  
    #region Idle State
    public class IdleState : BaseState<PlayerMove>
    {
        public IdleState(PlayerMove owner) : base(owner) {}
        public override void Enter() {}
        public override void Update(float dt)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 
            || Input.GetAxisRaw("Vertical") != 0
            )
            {
                owner.Fsm.ChangeState(owner.moveState);
            }
        }
        public override void Exit() {}
    }
    #endregion
    #region Move State
    public class MoveState : BaseState<PlayerMove>
    {
        public MoveState(PlayerMove owner) : base(owner) {}
        public override void Enter() => owner.Aim.SetBool("IsMoving",true);
        public override void Update(float dt)
        {
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");
            if (hor == 0 && ver == 0)
            {
                owner.Fsm.ChangeState(owner.idleState);
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && Time.time - owner._lastRollTime >= owner._rollCD)
            {
                owner.Fsm.ChangeState(owner.rollState);
                return;
            }
            else
            {
                owner._direction = new Vector2 (hor, ver).normalized;
                owner.Aim.SetFloat("X", hor);
                owner.Aim.SetFloat("Y", ver);
            }
        }
        public override void FixedUpdate(float dt)
        {
            owner._rb.velocity = owner._direction * owner._moveSpeed;
        }
        public override void Exit(){
            owner.Aim.SetBool("IsMoving",false);
            owner._rb.velocity = Vector2.zero;
        }
    }
    #endregion
    #region Roll State
    public class RollState : BaseState<PlayerMove>
    {
        private float rollingTime;
        public RollState(PlayerMove owner) : base(owner) {}
        public override void Enter()
        {
            owner.Aim.SetBool("IsRolling",true);
            rollingTime = 0f;
            owner._rb.AddForce(owner._direction * owner._rollSpeed, ForceMode2D.Impulse);
            owner._lastRollTime = Time.time;
        }
        public override void FixedUpdate(float dt)
        {
            if (owner._rb.velocity.magnitude < owner._moveSpeed)
            {
                owner._rb.velocity = owner._direction * owner._moveSpeed;
            }
            rollingTime += dt;
            if (rollingTime >= owner._totalRollTime)
            {
                owner.Fsm.ChangeState(owner.idleState);
                return;
            }
        }
        public override void Exit(){
            owner.Aim.SetBool("IsRolling",false);
        }
    }
    #endregion
    #endregion
    #region Initialization
    #region Tools
    public StateMachine Fsm {get; private set;}
    Animator Aim;
    Rigidbody2D _rb;
    #endregion
    #region Init All State
    IdleState idleState;
    MoveState moveState;
    RollState rollState;
    #endregion
    #region Setting
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _rollSpeed = 2f;
    [SerializeField] float _totalRollTime= 0.43f;
    [SerializeField] float _rollCD = 0.5f;
    #endregion
    #region Data
    Vector2 _direction {get; set;}
    float _lastRollTime;
    #endregion
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        _direction = new Vector2();
        _rb = GetComponent<Rigidbody2D>();
        Aim = GetComponent<Animator>();
        Fsm = new StateMachine();
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        rollState = new RollState(this);
    }
    void Start()
    {
        Fsm.ChangeState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        Fsm.Update(Time.deltaTime);
    }
    void FixedUpdate()
    {
        Fsm.FixedUpdate(Time.fixedDeltaTime);
    }

}
