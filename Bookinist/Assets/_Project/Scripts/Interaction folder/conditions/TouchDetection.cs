using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    Camera _cam;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _hitMask = ~0;



    private void Awake()
    {
        _cam = Camera.main;
    }


    public void OnTouch(Vector2 screenPosition)
    {
        Ray ray = _cam.ScreenPointToRay(screenPosition);
        print("touched");

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _hitMask))
        {
            string hitlayer = hit.collider.GetComponent<SpriteRenderer>().sortingLayerName;
            int camLayer = _cam.GetComponent<CameraMovement>().currentIndexLayer;
            InteractionRunner interactionRunner = hit.collider.GetComponent<InteractionRunner>();

            InteractionContext context = new InteractionContext
            {
                instigator = null,
                target = hit.collider.gameObject
            };  

            if (interactionRunner != null)
            {
                interactionRunner.TryExecuteAll(context);
            }
        }
    }
}
