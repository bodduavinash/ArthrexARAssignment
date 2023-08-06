using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageGridParticleManager : MonoBehaviour
{
    public string SpriteNamePrefix = "";
    public Sprite[] SpriteArray;
    public GameObject imageGridPrefab;
    public GameObject imageGridParentGO;

    private float m_gridWidth = 0.1f, m_gridHeight = 0.1f;
    private float m_imageWidth = 3f, m_imageHeight = 3f;

    private List<GameObject> m_spriteGOs = new List<GameObject>();

    private bool m_isRBStarted = false;

    private void Start()
    {
        LoadGridImages();
    }

    private void LoadGridImages()
    {
        int _gridImageIndex = 0;
        for (int i = 0; i < m_imageHeight / m_gridHeight; i++)
        {
            for (int j = 0; j < m_imageWidth / m_gridWidth; j++)
            {
                var spriteGO = Instantiate(imageGridPrefab);
                spriteGO.transform.parent = imageGridParentGO.transform;
                spriteGO.name = SpriteNamePrefix + _gridImageIndex;

                var updateTransform = spriteGO.GetComponent<IApplyParticle>();
                updateTransform?.OnApplyParticle(spriteGO, 
                    new Vector3(j * m_gridWidth, -i * m_gridHeight, 0f),
                    Quaternion.identity, SpriteArray[_gridImageIndex]);

                m_spriteGOs.Add(spriteGO);
                _gridImageIndex++;
            }
        }
    }

    private void Update()
    {

#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
#else
        if(Input.touchCount > 0)
#endif
        {
            if (!m_isRBStarted)
            {
                RaycastHit _raycastHit;

                Vector3 userPosition;

#if UNITY_EDITOR
                userPosition = Input.mousePosition;
#else
                userPosition = Input.GetTouch(0).position;
#endif

                var _worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(userPosition.x, userPosition.y, 1f));
                _worldPosition.z = 0f;

                var isRayCastHit2D = Physics.Raycast(_worldPosition, Vector2.down, out _raycastHit, 2f);
                if (isRayCastHit2D)
                {
                    _raycastHit.collider.GetComponent<IAddForceParticle>().OnAddForceParticle(_raycastHit);

                    m_isRBStarted = true;
                }
            }
            else
            {
                ResetAllParticleRigidbody();                
            }
        }
    }

    private void ResetAllParticleRigidbody()
    {
        foreach(var _spriteGOs in m_spriteGOs)
        {
            var _resetParticleRB = _spriteGOs.GetComponent<IParticleResetRigidbody>();
            _resetParticleRB.OnParticleResetRigidbody();

            var _reboundParticle = _spriteGOs.GetComponent<IParticleRebound>();
            _reboundParticle.OnParticleRebound();
        }

        m_isRBStarted = false;
    }
}
