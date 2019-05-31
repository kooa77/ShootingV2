using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasherSpiralWeapon : SpiralWeapon
{
	//float _changeInterval = 4.0f;
	float _changeDuration = 0.0f;

	//float _orginalShotAngleRate;
	//float _originalBulletAngleRate;

	//float _updateShotAngleRate;
	//float _updateBulletAngleRate;

	List<float> _stepTime = new List<float>();
	float _transitionOffset = 0.5f;

	override public void Init(Character owner, WeaponData weaponData)
	{
		base.Init(owner, weaponData);

		float stepValue = _weaponData.changeInterval / 4.0f;
		_stepTime.Clear();
		for (int i=0; i<6; i++)
		{
			_stepTime.Add(stepValue * (i+1));
		}

		_changeDuration = stepValue;
	}

	/*
	override public void SetAngleRate(float angleRate)
	{
		_shotAngleRate = angleRate;
		_orginalShotAngleRate = angleRate;
		_updateShotAngleRate = angleRate;
	}

	override public void SetBulletAngleRate(float rate)
	{
		_bulletAngleRate = rate;
		_originalBulletAngleRate = rate;
		_updateBulletAngleRate = rate;
	}
	*/

	override public void Update()
	{
		//if(_changeDuration < 1.0f)
		if (_changeDuration < _stepTime[0])
		{
			_shotAngleRate = _weaponData.angleRate;
			_bulletAngleRate = _weaponData.bulletAngleRate;
		}
		else if (_changeDuration < _stepTime[1])
		{
			_shotAngleRate = _weaponData.angleRate;
			_bulletAngleRate = _weaponData.bulletAngleRate;
		}
		//else if(_changeDuration < 2.0f)
		else if (_changeDuration < _stepTime[2])
		{
			/*
			_updateShotAngleRate = _weaponData.angleRate * (1.5f - _changeDuration);
			_updateBulletAngleRate = _weaponData.bulletAngleRate * (1.5f - _changeDuration);
			_shotAngleRate = _updateShotAngleRate;
			_bulletAngleRate = _updateBulletAngleRate;
			*/
			_shotAngleRate = _weaponData.angleRate * (_stepTime[1] + _transitionOffset - _changeDuration);
			_bulletAngleRate = _weaponData.bulletAngleRate * (_stepTime[1] + _transitionOffset - _changeDuration);
		}
		//else if(_changeDuration < 3.0f)
		else if (_changeDuration < _stepTime[3])
		{
			/*
			_updateShotAngleRate = -_weaponData.angleRate * (3.5f - _changeDuration);
			_updateBulletAngleRate = -_weaponData.bulletAngleRate * (3.5f - _changeDuration);
			_shotAngleRate = _updateShotAngleRate;
			_bulletAngleRate = _updateBulletAngleRate;
			*/
			_shotAngleRate = -_weaponData.angleRate;
			_bulletAngleRate = -_weaponData.bulletAngleRate;
		}
		else if (_changeDuration < _stepTime[4])
		{
			/*
			_updateShotAngleRate = -_weaponData.angleRate * (3.5f - _changeDuration);
			_updateBulletAngleRate = -_weaponData.bulletAngleRate * (3.5f - _changeDuration);
			_shotAngleRate = _updateShotAngleRate;
			_bulletAngleRate = _updateBulletAngleRate;
			*/
			_shotAngleRate = -_weaponData.angleRate;
			_bulletAngleRate = -_weaponData.bulletAngleRate;
		}
		//else if(_changeDuration < 4.0f)
		else if (_changeDuration < _stepTime[5])
		{
			_shotAngleRate = -_weaponData.angleRate * (_stepTime[4] + _transitionOffset - _changeDuration);
			_bulletAngleRate = -_weaponData.bulletAngleRate * (_stepTime[4] + _transitionOffset - _changeDuration);
		}
		else
		{
			_changeDuration = 0.0f;
		}
		_changeDuration += Time.deltaTime;
	}
}
