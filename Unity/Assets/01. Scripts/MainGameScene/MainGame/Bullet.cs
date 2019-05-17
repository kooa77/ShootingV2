using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float _speed = 2.5f;
    float _speedRate = .0f;

    float _angle = 0.0f;
    float _angleRate = 0.0f;

    void Start ()
    {
        Destroy(gameObject, 10.0f);
	}

    //float _curTestRot = 0;	 // 임시 변수
    void Update ()
    {
		Vector3 curPos = transform.position;
		Vector3 nextPos = curPos + (transform.forward * _speed * Time.deltaTime);
		transform.position = nextPos;

        _speed += (_speedRate * Time.deltaTime);

		transform.Rotate(Vector3.up, _angle);
        _angle += (_angleRate * Time.deltaTime);
	}

	[SerializeField] GameObject _effectPrefab;
    void OnTriggerEnter(Collider other)
    {
		// 내 총알의 주인이 부딪쳤으면 패스
		if (true == other.gameObject.tag.Equals("Character"))
		{
			Character character = other.gameObject.GetComponent<Character>();
			if (_shotCharType == character.GetCharacterType())
				return;
		}


		// 폭발 효과 실행 (스프라이트)
		GameObject effObject = GameObject.Instantiate(_effectPrefab);
		effObject.transform.position = transform.position;
		effObject.transform.localScale = Vector3.one;
		Destroy(effObject, 1.5f);

		Destroy(gameObject);
	}

	Character.CharType _shotCharType;

	public Character.CharType ShotCharacterType()
	{
		return _shotCharType;
	}

	public void SetShotCharacterType(Character.CharType charType)
	{
		_shotCharType = charType;
	}

    public void SetSpeedRate(float rate)
    {
        _speedRate = rate;
    }

    public void SetAngleRate(float rate)
    {
        
        _angleRate = rate;
    }
}
