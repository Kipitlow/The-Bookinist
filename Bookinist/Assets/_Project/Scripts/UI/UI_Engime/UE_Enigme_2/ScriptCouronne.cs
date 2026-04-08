using UnityEngine;

public class VRRR
{
    public bool fleurRouge;
    public Transform tr;
}

public class ScriptCouronne : MonoBehaviour
{
    public GameObject prefableFlowers;
    private bool _typeFleure;

    public Sprite RougeFleur;
    public Sprite VioletFleur;
 
    public GameObject couronne_rouge;
    public GameObject couronne_fusion;
    public GameObject couronne_violet;
    int redFlowers=0, purpleFlowers = 0, placeFlowers=0;

    public void spawnFlowers(GameObject selfObject)
    {
        //GameObject RR = Instantiate(prefableFlowers, selfObject.transform.position, selfObject.transform.rotation);
        GameObject RR = Instantiate(prefableFlowers, selfObject.transform.position, selfObject.transform.rotation);
        RR.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
        if(_typeFleure)
        {
            RR.GetComponent<SpriteRenderer>().sprite = RougeFleur;
        }
        else { RR.GetComponent<SpriteRenderer>().sprite = VioletFleur; }
        gameObject.GetComponent<InteractionRunner>().enabled = false;
    }

    public void addFlower(bool redFleur)
    {
        if(redFleur)
        {
            _typeFleure = true;
            redFlowers += 1; 
        }
        else 
        {
            _typeFleure = false;
            purpleFlowers += 1; 
        }
        placeFlowers += 1;
        if(placeFlowers>=5)
        {
            _visiblingCouronne();
        }
    }

    private void _visiblingCouronne()
    {
        if(redFlowers>0 && purpleFlowers == 0) { couronne_rouge.SetActive(true); }
        if(redFlowers>0 && purpleFlowers>0) { couronne_fusion.SetActive(true); }
        if(redFlowers==0 && purpleFlowers > 0) { couronne_violet.SetActive(true); }
    }
}
