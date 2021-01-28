using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayButton : MonoBehaviour
{
    //private bool mFaded = false;
    //public float Duration = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /*public void Fade()
    {
      
       var canvasGroup = GetComponent<CanvasGroup>();

       StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, mFaded ? 1 : 0));

       mFaded = !mFaded;

        
    }
    
    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);

            yield return null;
        }
    }*/ 
}
