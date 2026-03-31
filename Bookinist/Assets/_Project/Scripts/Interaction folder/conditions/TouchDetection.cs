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

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _hitMask))
        {
            int hitlayer = hit.collider.GetComponent<SpriteRenderer>().sortingOrder;
            int camLayer = _cam.GetComponent<CameraMovement>().currentIndexLayer;
            InteractionRunner interactionRunner = hit.collider.GetComponent<InteractionRunner>();


            print(hitlayer + " : " + camLayer);
            if (hitlayer == camLayer)
            {

                print("touched");
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
}
