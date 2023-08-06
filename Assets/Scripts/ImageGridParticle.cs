using UnityEngine;

public class ImageGridParticle : MonoBehaviour, IApplyParticle, IParticleResetRigidbody, IParticleRebound, IAddForceParticle
{
    private Vector3 m_originalPosition;
    private Quaternion m_originalRotation;
    private Sprite m_spriteImage;

    public void OnApplyParticle(GameObject _gameObject, Vector3 _position, Quaternion _rotation, Sprite _spriteImage)
    {
        _gameObject.transform.localPosition = _position;
        _gameObject.transform.localRotation = _rotation;

        m_originalPosition = _gameObject.transform.position;
        m_originalRotation = _gameObject.transform.rotation;

        var sRenderer = _gameObject.GetComponent<SpriteRenderer>();
        if (sRenderer)
        {
            sRenderer.sprite = _spriteImage;
            m_spriteImage = _spriteImage;
        }
    }

    public void OnParticleResetRigidbody()
    {
        var _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _rb.drag = 0f;
        _rb.angularDrag = 0f;
    }

    public void OnParticleRebound()
    {
        transform.position = m_originalPosition;
        transform.rotation = m_originalRotation;
    }

    public void OnAddForceParticle(RaycastHit _raycastHit)
    {
        var _rb = _raycastHit.collider.attachedRigidbody;
        _rb.AddForce(Vector2.one * 400f, ForceMode.Force);
    }
}
