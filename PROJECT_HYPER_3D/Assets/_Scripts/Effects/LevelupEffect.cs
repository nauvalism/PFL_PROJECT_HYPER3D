using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelupEffect : MonoBehaviour
{
   [SerializeField] TextMeshPro levelupTxt;
   [SerializeField] SpriteRenderer circleCore;
   [SerializeField] ParticleSystem particle;
   [SerializeField] List<ParticleSystem> particles;
   [SerializeField] List<SoundManager> levelupSounds;

   public void DoLvlUp()
   {
        LeanTween.cancel(levelupTxt.gameObject);
        levelupTxt.alpha = 0;
        levelupTxt.transform.localPosition = Vector3.zero;
        
        LeanTween.cancel(circleCore.gameObject);
        circleCore.transform.localScale = Vector3.zero;
        circleCore.color = new Color(circleCore.color.r, circleCore.color.g, circleCore.color.b, 1);
        LeanTween.scale(circleCore.gameObject, Vector3.one, 2.0f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.alpha(circleCore.gameObject, .0f, 1.75f).setEase(LeanTweenType.easeOutQuad);

        LeanTween.moveLocalY(levelupTxt.gameObject, 1.50f, 2.0f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.value(levelupTxt.gameObject, .0f, 1.0f, 0.75f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            levelupTxt.alpha = f;
        }).setLoopPingPong(1);

        particle.Play();
        for(int i = 0 ; i < particles.Count ; i++)
        {
            particles[i].Play();
        }

        for(int i = 0 ; i < levelupSounds.Count ; i++)
        {
            levelupSounds[i].Play(i);
        }
   }
}
