using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovingItem : MonoBehaviour
{
    private Image img;
    

    public void Setup(Sprite sprite, Vector3 targetPos)
    {
        img = GetComponent<Image>();
        img.sprite = sprite;

        // Lance la séquence : Attendre 1s, puis bouger en 1s
        StartCoroutine(MoveSequence(targetPos));
    }

    IEnumerator MoveSequence(Vector3 target)
    {
        // 1. Attente initiale après apparition
        yield return new WaitForSeconds(1f);

        // 2. Déplacement vers la cible
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target, elapsed / duration);
            yield return null;
        }

        transform.position = target;
        Destroy(gameObject, 0.1f); // Détruit l'objet une fois arrivé
    }
}