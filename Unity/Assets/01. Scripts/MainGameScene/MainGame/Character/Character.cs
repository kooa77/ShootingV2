using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] List<GameObject> _charPrefabList;

    [SerializeField] RuntimeAnimatorController _animatorController;
    
    public enum CharType
    {
        Player,
        NPC
    }
    [SerializeField] CharType _charType = CharType.NPC;

	public CharType GetCharacterType()
	{
		return _charType;
	}

    List<CharacterModule> _charModuleList = new List<CharacterModule>();
    CharacterModule _characterModule = null;
    
    AnimationController _animationController;

    void Awake()
    {
        // 플레이어 모듈을 생성
        _charModuleList.Add(new PlayerModule(this));
        _charModuleList.Add(new NPCModule(this));

        _characterController = gameObject.GetComponent<CharacterController>();

        // 자동화
        {
            // 1.
            // 에디터에서 프리팹을 세팅
            // 세팅한 프리팹을 객체로 생성
            {
                int index = 0;
                if (CharType.Player == _charType)
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }

                GameObject obj = GameObject.Instantiate<GameObject>(_charPrefabList[index]);
                obj.transform.position = transform.position;
                obj.transform.rotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;

                obj.transform.SetParent(transform);
            }

            if ( 0 < transform.childCount)
            {
                Transform childTransform = transform.GetChild(0);
                childTransform.gameObject.AddComponent<AnimationController>();
                _animationController = childTransform.gameObject.GetComponent<AnimationController>();

                Animator animCom = childTransform.gameObject.GetComponent<Animator>();
                animCom.runtimeAnimatorController = _animatorController;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterModule = _charModuleList[(int)_charType];
        _characterModule.BuildStateList();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(eState.DEATH != _stateType)
        {
            _characterModule.UpdateAI();

            UpdateState();
            UpdateMove();
        }
	}

	float _hp = 100.0f;
	void OnTriggerEnter(Collider other)
    {
		if (eState.DEATH == _stateType)
			return;

		// 캐릭터
			// tag가 총알 태그이면
			// other.gameObject 에서 총알 스크립트 컴포넌트를 뽑아냄
			// 총알 스크립트의 쏜 객체 정보 조사
			// 그 정보가 나이면 패스
			// tag가 총알 태그이면
		
		//if (other.gameObject.Equals(gameObject))
        //    return;
		// 내가 쏜 총알일 때 패스
		if(true == other.gameObject.tag.Equals("Bullet"))
		{
			Bullet bullet = other.gameObject.GetComponent<Bullet>();
			if (_charType == bullet.ShotCharacterType())
				return;
		}

		if(_hp <= 0.0f)
		{
			ChangeState(eState.DEATH);
		}
		else
		{
			_hp-=10;
		}
    }

    void OnTriggerExit(Collider other)
    {
        // 내가 쏜 총알일 때 패스
        if (other.gameObject.Equals(gameObject))
            return;
    }


    // 유한 상태 머신 (FSM)

    public enum eState
    {
        IDLE,
        WAIT,
        KICK,
        WALK,
        RUN,
        SLIDE,
        DEATH,
    }

    eState _stateType = eState.IDLE;

    public void ChangeState(eState state)
    {
        _stateType = state;
        _state = _stateDic[state];
        _state.Start();
    }

    void UpdateState()
    {
        _state.Update();
    }

    public eState GetStateType()
    {
        return _stateType;
    }


    // State

    Dictionary<eState, State> _stateDic = new Dictionary<eState, State>();
    State _state = null;

    public Dictionary<eState, State> GetStateDic()
    {
        return _stateDic;
    }


    // Animation

    public void PlayAnimation(string trigger, System.Action endCallback)
    {
        _animationController.Play(trigger, endCallback);
    }


    // Movement

    CharacterController _characterController;
    float _moveSpeed = 0.0f;
    Vector3 _destPoint;

    void UpdateMove()
    {
        Vector3 moveDirection = GetMoveDirection();
        Vector3 moveVelocity = moveDirection * _moveSpeed;
        Vector3 gravityVelocity = Vector3.down * 9.8f;  // 중력

        Vector3 finalVelocty = (moveVelocity + gravityVelocity) * Time.deltaTime;
        _characterController.Move(finalVelocty);

        // 현재 위치와 목적지 까지의 거리를 계산해서
        // 적절한 범위 내에 들어오면 스톱.
        if(0.0f < _moveSpeed)
        {
            float distance = GetDistanceToTarget();
            if (distance < 0.5f)
            {
                _moveSpeed = 0.0f;
                ChangeState(eState.IDLE);
            }
        }
    }

    public float GetDistanceToTarget()
    {
        Vector3 charPos = transform.position;
        Vector3 curPos = new Vector3(charPos.x, 0.0f, charPos.z);
        Vector3 destPos = new Vector3(_destPoint.x, 0.0f, _destPoint.z);
        float distance = Vector3.Distance(curPos, destPos);
        return distance;
    }

    public void StartMove(float speed)
    {
        _moveSpeed = speed;
    }

    public void StopMove()
    {
        _moveSpeed = 0.0f;
    }

    public void SetDestination(Vector3 destPoint)
    {
        _destPoint = destPoint;
    }

    Vector3 GetMoveDirection()
    {
        // (목적위치 - 현재 위치) 노멀라이즈
        Vector3 charPos = transform.position;
        Vector3 curPos = new Vector3(charPos.x, 0.0f, charPos.z);
        Vector3 destPos = new Vector3(_destPoint.x, 0.0f, _destPoint.z);
        Vector3 direction = (destPos - curPos).normalized;

        /*
        Vector3 lookPos = new Vector3(_destPoint.x, charPos.y, _destPoint.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
                                    transform.rotation,
                                    targetRotation,
                                    360.0f * Time.deltaTime);
        */
        
        return direction;
    }


    // Attack

    [SerializeField] GameObject _bulletPrefab;

    float _curTestRot = 0;      // 임시 변수
	public void Fire()
    {
        // 총알을 생성
        GameObject bulletObject = GameObject.Instantiate<GameObject>(_bulletPrefab);
		Vector3 bulletPos = transform.position +
			(transform.forward * 1.5f) +
			(transform.up * 1.0f);
		bulletObject.transform.position = bulletPos;
		bulletObject.transform.rotation = transform.rotation;

		Bullet bullet = bulletObject.GetComponent<Bullet>();
		bullet.SetShotCharacterType(_charType);

		bulletObject.transform.Rotate(Vector3.up, _curTestRot);
		_curTestRot += 30;
	}
}
