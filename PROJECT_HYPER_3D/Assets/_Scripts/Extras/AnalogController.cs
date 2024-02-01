using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnalogController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
    
    [SerializeField] CharacterMovement rootChar;
    [SerializeField] CharacterRotator rootRotator;
    public CanvasScaler scale;
    public Transform analogParent;
    public List<Image> decorativeImages;
    public Image bgImg;
	public Image joyStickImg;
    public Camera tapCamera;
    public CanvasGroup magnitudeScaler;
    public Image magnitudeStretcher;
    public Transform analogRootParent;
    public Vector3 defaultRootPosition;
    public float distanceFromCenter;
    public List<Image> toChangeColor;
    public Color cancelColor;
    public Color okayColor;
    public Color cc;

    public Vector3 posDebug;

	public Vector3 InputDirection;
    public Vector3 LastDirection;
    public bool shooting = false;
    public bool isControlling = false;
    public bool isAnaloging = false;
    public bool _enabled = true;
    public bool enableAnalog = true;
    public ColorAlphaPingpong guideAlpha;

    public void Redding()
    {
        cc = cancelColor;
    }

    public void BlueIng()
    {
        cc = okayColor;
    }

	void Awake()
	{
		bgImg = GetComponent<Image> ();
        analogParent = bgImg.transform.parent;
        //tapCamera = GameObject.FindGameObjectWithTag("touchCamera").GetComponent<Camera>();
        defaultRootPosition = analogRootParent.localPosition;
        //joyStickImg = transform.GetChild (0).GetComponent<Image> ();
        //DissapearJoystick();
    }

    void Update()
    {
        if (!enableAnalog)
            return;

        posDebug = new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), 0);

        //if (!isAnaloging)
        //{
        //    //if(analogParent.localPosition.y > 220.0f || analogParent.localPosition.y < -220.0f)
        //    //{
        //    //    //return;
        //    //}
        //    if (posDebug.y > 115.0f || posDebug.y < -220.0f || posDebug.x < -350.0f || posDebug.x > 350.0f)
        //    {
        //        AnalogManager.instance.cg.interactable = false;
        //        AnalogManager.instance.cg.blocksRaycasts = false;
        //        return;
        //    }
        //    else
        //    {
        //        analogParent.position = Input.mousePosition;
        //        AnalogManager.instance.cg.interactable = true;
        //        AnalogManager.instance.cg.blocksRaycasts = true;
        //    }
            
            
        //}

#if UNITY_EDITOR


        if (Input.GetMouseButton(0))
        {
            

            // if (GameplayController.Instance.subState != GameplaySubState.preparing)
            // {
            //     //Loger.Ler("ANALOG NOT MOVING : " + GameplayController.Instance.subState);
            //     return;
            // }

            Vector3 mousePos = tapCamera.ScreenToWorldPoint(Input.mousePosition);

            //if (mousePos.y <= -3.5f)
            //    return;

            //if (mousePos.y >= 2.5f)
            //    return;

            if (!enableAnalog)
            {
                DissapearJoystick();
                return;
            }
            else if(!isAnaloging)
            {
                return;
            }
            else
            {
                bgImg.color = new Color(cc.r, cc.g, cc.b, .5f);
                joyStickImg.color = new Color(cc.r, cc.g, cc.b, .5f);
                foreach (Image m in decorativeImages)
                {
                    m.color = new Color(cc.r, cc.g, cc.b, .5f);
                }
            }

            Vector2 pos = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (bgImg.rectTransform,
                    Input.mousePosition,
                    null,
                    out pos))
            {
                pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
                pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

                float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
                float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

                InputDirection = new Vector3(x, y, 0);
                LastDirection = InputDirection;
                InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

                joyStickImg.rectTransform.anchoredPosition =
                    new Vector3(InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3)
                        , InputDirection.y * (bgImg.rectTransform.sizeDelta.y / 3));

                distanceFromCenter = Vector2.Distance((Vector2)joyStickImg.transform.position, (Vector2)bgImg.transform.position);
                rootChar.SetMovement(InputDirection);                
                rootRotator.ReceiveAnalogInput(InputDirection);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //if (!shooting)
            //    return;

//            if (GameplayController.Instance.subState != GameplaySubState.preparing)
//                return;

//            Vector3 mousePos = tapCamera.ScreenToWorldPoint(Input.mousePosition);

//            if (mousePos.y <= -3.5f)
//                return;

//            if (mousePos.y >= 2.5f)
//                return;

//            if (GameplayController.Instance.panning)
//                return;

            
//#if UNITY_EDITOR
//            StartCoroutine(MovingJoystick(posDebug));
//#else
//            StartCoroutine(MovingJoystick(mousePos));
//#endif
            //analogParent.position = Input.mousePosition;


            //if (posDebug.x < -50)
            //{
            //    MoveJoystick(posDebug);
            //}
        }

        if (Input.GetMouseButtonUp(0))
        {
            //if (!shooting)
            //    return;

            //if (isAnaloging)
            //    DissapearJoystick();

            DissapearJoystick();
            rootChar.SetMovement(Vector3.zero);                
            //rootRotator.ReceiveAnalogInput(InputDirection);
        }



#else
          if(Input.touchCount > 0)
            {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //if (GameplayController.Instance.subState != GameplaySubState.preparing)
                //    return;

                //Vector3 mousePos = tapCamera.ScreenToWorldPoint(t.position);

                //if (mousePos.y <= -3.5f)
                //    return;

                //if (mousePos.y >= 2.5f)
                //    return;

                //if (GameplayController.Instance.panning)
                //    return;

                //isAnaloging = true;
                //StartCoroutine(MovingJoystick(mousePos));
            }

            if(t.phase == TouchPhase.Moved)
            {

            Vector3 mousePos = tapCamera.ScreenToWorldPoint(t.position);

            if (mousePos.y <= -3.5f)
                return;

            if (mousePos.y >= 2.5f)
                return;

            if (GameplayController.Instance.subState != GameplaySubState.preparing)
            {
                //Loger.Ler("ANALOG NOT MOVING : " + GameplayController.Instance.subState);
                return;
            }



            if (!enableAnalog)
            {
                DissapearJoystick();
                return;
            }
            else
            {
                bgImg.color = new Color(cc.r, cc.g, cc.b, .5f);
                joyStickImg.color = new Color(cc.r, cc.g, cc.b, .5f);
                foreach (Image m in decorativeImages)
                {
                    m.color = new Color(cc.r, cc.g, cc.b, .5f);
                }
            }

            Vector2 pos = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (bgImg.rectTransform,
                    Input.mousePosition,
                    null,
                    out pos))
            {
                pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
                pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

                float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
                float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

                InputDirection = new Vector3(x, y, 0);
                LastDirection = InputDirection;
                InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

                joyStickImg.rectTransform.anchoredPosition =
                    new Vector3(InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3)
                        , InputDirection.y * (bgImg.rectTransform.sizeDelta.y / 3));

                magnitudeStretcher.transform.localScale = new Vector3(GameplayController.Instance.GetCurrentTurnPlayerSolo().shootController.AnchorPoint.magnitude * 5, .80f, 1.0f);
                magnitudeScaler.alpha = 1.0f;
            }
        }

            if (t.phase == TouchPhase.Ended)
            {
                //if (!shooting)
                //    return;

                //if (isAnaloging)
                //    DissapearJoystick();

                DissapearJoystick();
            }
        }
#endif

#if UNITY_EDITOR
        //if (Input.GetKey(KeyCode.D))
        //{
        //    InputDirection = new Vector3(1.0f, InputDirection.y, InputDirection.z);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    InputDirection = new Vector3(-1.0f, InputDirection.y, InputDirection.z);
        //}

        //if (Input.GetKey(KeyCode.W))
        //{
        //    InputDirection = new Vector3(InputDirection.x, 1.0f, InputDirection.z);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    InputDirection = new Vector3(InputDirection.x, -1.0f, InputDirection.z);
        //}

        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    InputDirection = new Vector3(0, InputDirection.y, InputDirection.z);
        //}

        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    InputDirection = new Vector3(0, InputDirection.y, InputDirection.z);
        //}

        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    InputDirection = new Vector3(InputDirection.x, 0, InputDirection.z);
        //}

        //if (Input.GetKeyUp(KeyCode.S))
        //{
        //    InputDirection = new Vector3(InputDirection.x, 0, InputDirection.z);
        //}
#elif UNITY_ANDROID

#endif

    }

    public void InitAnalog()
    {
        Vector3 mousePos;
#if !UNITY_EDITOR
         if(Input.touchCount > 0)
         {
             Touch t = Input.GetTouch(0);
             mousePos = tapCamera.ScreenToWorldPoint(t.position);
             StartCoroutine(MovingJoystick(mousePos));
         }
   
#else
        mousePos = tapCamera.ScreenToWorldPoint(Input.mousePosition);
        StartCoroutine(MovingJoystick(mousePos));
#endif


    }

    public IEnumerator StartTouching()
    {
        isAnaloging = true;
        yield return new WaitForEndOfFrame();

    }

    public IEnumerator MovingJoystick(Vector3 posDebug)
    {
        AnalogManager.instance.UnGuide();
        yield return new WaitForEndOfFrame();
        
#if UNITY_EDITOR
        analogParent.position = Input.mousePosition;
#else
        if(Input.touchCount > 0)
        {
            analogParent.position = Input.GetTouch(0).position;
        }
#endif
        
        MoveJoystick(posDebug);
        yield return new WaitForEndOfFrame();
        isAnaloging = true;
        magnitudeScaler.alpha = 1.0f;
        yield return new WaitForEndOfFrame();
        
    }

    

    public void ResetJoystick()
    {
        enableAnalog = true;
    }

    public void MoveJoystick(Vector3 position)
    {
        // if (GameplayController.Instance.subState != GameplaySubState.preparing)
        //     return;

        isAnaloging = true;
        //bgImg.enabled = true;
        //joyStickImg.enabled = true;
        LeanTween.cancel(bgImg.gameObject);
        LeanTween.cancel(joyStickImg.gameObject);
        LeanTween.alpha(bgImg.GetComponent<RectTransform>(), 1.0f, .15f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.alpha(joyStickImg.GetComponent<RectTransform>(), 1.0f, .15f).setEase(LeanTweenType.easeOutQuad);
        AnalogManager.instance.UnGuide();
        enableAnalog = true;
    }

    public void DissapearJoystick()
    {
        //bgImg.enabled = false;
        //joyStickImg.enabled = false;
        //enableAnalog = false;
        LeanTween.cancel(bgImg.gameObject);
        LeanTween.cancel(joyStickImg.gameObject);
        bgImg.color = new Color(1, 1, 1, 0);
        joyStickImg.color = new Color(1, 1, 1, 0);
        //isAnaloging = false;
        //magnitudeScaler.enabled = false;
        foreach(Image m in decorativeImages)
        {
            m.color = new Color(1, 1, 1, 0);
        }
        magnitudeScaler.alpha = .0f;
    }

	public virtual void OnDrag(PointerEventData ped)
	{
        //Debug.Log (ped.position);

        //if (!shooting)
        //    return;

        // if (GameplayController.Instance.subState != GameplaySubState.preparing)
        //     return;

        //Loger.Ler(enableAnalog);

        if (!enableAnalog)
        {
            DissapearJoystick();
            return;
        }
        else
        {
            bgImg.color = new Color(cc.r, cc.g, cc.b, .5f);
            joyStickImg.color = new Color(cc.r, cc.g, cc.b, .5f);
            foreach (Image m in decorativeImages)
            {
                m.color = new Color(cc.r, cc.g, cc.b, .5f);
            }
        }

       

        isControlling = true;
        //Vector2 pos = Vector2.zero;
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle
        //    (bgImg.rectTransform,
        //        ped.position,
        //        ped.pressEventCamera,
        //        out pos))
        //{
            
        //    pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
        //    pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

        //    float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
        //    float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

        //    InputDirection = new Vector3(x, y, 0);
        //    LastDirection = InputDirection;
        //    InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //    joyStickImg.rectTransform.anchoredPosition =
        //        new Vector3(InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3)
        //            , InputDirection.y * (bgImg.rectTransform.sizeDelta.y / 3));
        //}

        //Debug.Log(InputDirection);
        //Debug.Log(Camera.main.point(joyStickImg.transform.localPosition));
        //Debug.Log(joyStickImg.transform.localPosition);
        //Debug.Log(Camera.main.ScreenToWorldPoint(joyStickImg.transform.localPosition));
    }

	public virtual void OnPointerDown(PointerEventData ped)
	{
        //if (!shooting)
        //    return;

        // if (GameplayController.Instance.subState != GameplaySubState.preparing)
        //     return;


        if (!enableAnalog)
            return;
        //isAnaloging = false;

        

        AnalogManager.instance.cg.interactable = true;
        AnalogManager.instance.cg.blocksRaycasts = true;

        bgImg.color = new Color(cc.r, cc.g, cc.b, .5f);
        joyStickImg.color = new Color(cc.r, cc.g, cc.b, .5f);

        joyStickImg.rectTransform.anchoredPosition = Vector3.zero;
		isControlling = true;
		OnDrag (ped);
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
        //if (!shooting)
        //    return;

        // if (GameplayController.Instance.subState != GameplaySubState.preparing)
        //     return;

        //if (!enabled)
        //    return;
        //isAnaloging = true;
        isControlling = false;
		InputDirection = Vector3.zero;
		joyStickImg.rectTransform.anchoredPosition = Vector3.zero;
        //enableAnalog = false;
        DissapearJoystick();
        AnalogManager.instance.UnGuide();
        analogRootParent.localPosition = defaultRootPosition;
        rootChar.SetMovement(InputDirection);
	}

    public virtual void InitAnalogPosition()
    {
        // Player p = GameplayController.Instance.GetCurrentTurnPlayerSolo();
        // Vector3 cPos = new Vector3(0, defaultRootPosition.y, defaultRootPosition.z);
        // if(p.transform.position.x < 0)
        // {
        //     cPos.x = -360;
        // }
        // else
        // {
        //     cPos.x = 360;
        // }
        // analogRootParent.localPosition = cPos;
    }

    // public Vector3 GetPingPongPosition()
    // {
    //     Player p = GameplayController.Instance.GetCurrentTurnPlayerSolo();
    //     Vector3 cPos = new Vector3(0, defaultRootPosition.y, defaultRootPosition.z);
    //     if (p.transform.position.x < 0)
    //     {
    //         cPos.x = -360;
    //     }
    //     else
    //     {
    //         cPos.x = 360;
    //     }
    //     return cPos;
    // }
}
