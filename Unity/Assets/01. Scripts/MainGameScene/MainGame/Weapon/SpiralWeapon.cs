using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralWeapon
{
    Character _owner;

    float _shotSpeed = 0.1f;
    float _shotDuration = 0.0f;
    float _shotAngle = 0.0f;
    float _shotAngleRate = 0.0f;

    int _shotCount = 4;

    float _bulletSpeedRate = 0.0f;
    float _bulletAngleRate = 0.0f;

    public void SetOwner(Character owner)
    {
        _owner = owner;
    }

    public void SetAngleRate(float angleRate)
    {
        _shotAngleRate = angleRate;
    }

    public void SetBulletSpeedRate(float speedRate)
    {
        _bulletSpeedRate = speedRate;
    }

    public void SetBulletAngleRate(float angleRate)
    {
        _bulletAngleRate = angleRate;
    }

    public void Fire(GameObject bulletPrefab)
    {
        if (_shotSpeed <= _shotDuration)
        {
            _shotDuration = 0.0f;

            for (int i = 0; i < _shotCount; i++)
            {
                float shotAngle = _shotAngle + (360.0f * (float)i / (float)_shotCount);
                CreateBullet(bulletPrefab, shotAngle);
            }

            _shotAngle += _shotAngleRate;
        }
        _shotDuration += Time.deltaTime;
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
}
