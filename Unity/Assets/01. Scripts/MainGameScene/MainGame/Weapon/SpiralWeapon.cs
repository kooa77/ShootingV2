using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
보스 5인연속.
보스를 처치하면 다음 보스가 등장
보스의 탄막이 점점 어려워진다.
5인을 이기면 승리
*/

public struct WeaponData
{
	public float shotSpeed;
	public int shotCount;

	public float angleRate;
	public float bulletSpeedRate;
	public float bulletAngleRate;

	public float changeInterval;
}

public class SpiralWeapon
{
	Character _owner;

	//float _shotSpeed = 0.1f;
	float _shotDurataion = 0.0f;

	float _shotAngle = 0.0f;
	protected float _shotAngleRate = 20.0f;

	float _bulletSpeedRate = 0.0f;
	protected float _bulletAngleRate = 0.0f;

	//int _shotCount = 4;

	protected WeaponData _weaponData;

	virtual public void Init(Character owner, WeaponData weaponData)
	{
		_owner = owner;

		_weaponData = weaponData;
		_shotAngleRate = _weaponData.angleRate;
		_bulletSpeedRate = _weaponData.bulletSpeedRate;
		_bulletAngleRate = _weaponData.bulletAngleRate;
	}

	/*
	public void SetOwner(Character owner)
	{
		_owner = owner;
	}

	virtual public void SetAngleRate(float angleRate)
	{
		_shotAngleRate = angleRate;
	}

	public void SetBulletSpeedRate(float rate)
	{
		_bulletSpeedRate = rate;
	}

	virtual public void SetBulletAngleRate(float rate)
	{
		_bulletAngleRate = rate;
	}
	*/

public void Fire(GameObject bulletPrefab)
	{
		if (_weaponData.shotSpeed < _shotDurataion)
		{
			_shotDurataion = 0.0f;
			for (int i = 0; i < _weaponData.shotCount; i++)
			{
				float shotAngle = _shotAngle +
					(360.0f * ((float)i / (float)_weaponData.shotCount));
				CreateBullet(bulletPrefab, shotAngle);
			}
			_shotAngle += _shotAngleRate;
		}
		_shotDurataion += Time.deltaTime;
	}

	void CreateBullet(GameObject bulletPrefab, float shotAngle)
	{
		// 총알을 생성
		GameObject bulletObject = GameObject.Instantiate<GameObject>(bulletPrefab);
		Vector3 bulletPos = _owner.transform.position + (_owner.transform.up * 1.0f);
		bulletObject.transform.position = bulletPos;
		bulletObject.transform.rotation = _owner.transform.rotation;
		bulletObject.transform.Rotate(Vector3.up, shotAngle);

		Bullet bullet = bulletObject.GetComponent<Bullet>();
		bullet.SetShotCharacterType(_owner.GetCharacterType());
		bullet.SetSpeedRate(_bulletSpeedRate);
		bullet.SetAngleRate(_bulletAngleRate);
	}

	virtual public void Update()
	{
	}
}
