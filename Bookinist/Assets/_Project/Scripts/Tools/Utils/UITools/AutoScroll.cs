using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private Transform _creditPanel;
    [SerializeField] private float _maxPosY;
    [SerializeField] private float _spawnPosY;

    private void OnEnable()
    {
        //StartCoroutine(StartAutoScroll());
        Vector3 newPos = new Vector3(_creditPanel.transform.position.x, _spawnPosY, _creditPanel.transform.position.z);
        _creditPanel.position = newPos;
    }

    private void Update()
    {
        if (_creditPanel.position.y > _maxPosY) return;

        Vector3 newPos = new Vector3(_creditPanel.transform.position.x, _creditPanel.transform.position.y + Time.deltaTime, _creditPanel.transform.position.z);
        _creditPanel.position = newPos;

    }
    IEnumerator StartAutoScroll()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            Vector3 newPos = new Vector3(_creditPanel.transform.position.x, _creditPanel.transform.position.y + Time.deltaTime, _creditPanel.transform.position.z);
            _creditPanel.position = newPos;

            if (newPos.y > _maxPosY) break;
        }
    }
}
