using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicActor : MonoBehaviour
{
    [SerializeField] Transform rootGraphic;
    [SerializeField] Renderer render;
    [SerializeField] GameObject materialPlace;
    [SerializeField] List<int> ltids;

    private void OnValidate() {
        
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //render = materialPlace.GetComponent<Renderer>();
        render = materialPlace.GetComponent<Renderer>();
        for(int i = 0 ; i < render.materials.Length; i++)
        {
            ltids.Add(-1);
        }
    }

    protected virtual void Update() {
        
    }

    public virtual void GetHit()
    {
        LeanTween.cancel(materialPlace);
        LeanTween.value(materialPlace, Color.red, Color.white, .25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((Color c)=>{
            //Debug.Log("Warna");
                render.material.color = c;
        });
        LeanTween.scale(materialPlace, new Vector3(1.25f,1.25f,1.25f), 1.0f).setEase(LeanTweenType.punch);
        GetHitAll();
    }

    public virtual void GetHitAll()
    {
        for(int i = 0 ; i < render.materials.Length ; i++)
        {
            //Debug.Log(render.materials[i].name);
        }
    
    }

    public virtual void Heal()
    {
        LeanTween.cancel(materialPlace);
        LeanTween.cancel(materialPlace);
        LeanTween.value(materialPlace, Color.green, Color.white, .25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((Color c)=>{
            //Debug.Log("Warna");
                render.material.color = c;
        });
    }

    public virtual void Blink()
    {
        LeanTween.cancel(materialPlace.gameObject);
        LeanTween.value(materialPlace, 1.0f, .0f, 0.5f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(5).setOnUpdate((float f)=>{
            render.material.color = new Color(1,1,1,f);
        });
    }

    public virtual void DisableGraphic()
    {
        rootGraphic.transform.localScale = Vector3.zero;
        
    }

    
}
