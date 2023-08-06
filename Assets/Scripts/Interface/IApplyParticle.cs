using UnityEngine;

public interface IApplyParticle
{
    void OnApplyParticle(GameObject _gameObject, Vector3 _position, Quaternion _rotation, Sprite _spriteImage);
}