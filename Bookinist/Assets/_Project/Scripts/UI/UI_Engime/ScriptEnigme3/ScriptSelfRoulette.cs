using UnityEngine;

public class ScriptSelfRoulette : MonoBehaviour
{
    public bool DetectCollision=false;
    GameObject T_T;
    private void Start()
    {
        T_T = GameObject.Find("TraitCarrer");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == T_T.name) 
        {
            DetectCollision = true;
        } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == T_T.name)
        {
            DetectCollision = false;
        }
    }
}
