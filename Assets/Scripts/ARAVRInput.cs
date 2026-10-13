#define PC
// #define Oculus

using UnityEngine;

public static class ARAVRInput
{
#if PC
    private enum ButtonTarget
    {
        Fire1,
        Fire2,
        Fire3,
        Jump,
    }
#endif
    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2
#elif Oculus
        One = OVRInput.Button.One,
        Two = OVRInput.Button.Two,
        Thumbstick = OVRInput.Button.PrimaryThumbstick,
        IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
        HandTrigger = OVRInput.Button.PrimaryHandTrigger
#endif
    }

    public enum Controller
    {
#if PC
        LTouch,
        RTouch
#elif Oculus
        LTouch = OVRInput.Controller.LTouch,
        RTouch = OVRInput.Controller.RTouch
#endif
    }

#if Oculus
    static Transform rootTransform;
#endif
#if Oculus
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {
            rootTransform = GameObject.Find("TrackingSpace").transform;
        }
        return rootTransform;
    }
# endif

    private static Camera _mainCamera;

    static Camera MainCamera
    {
        get
        {
            if (!_mainCamera)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    // 왼쪽 컨트롤러
    private static Transform _lHand;
    // 씬에 등록된 왼쪽 컨트롤러를 찾아 반환
    public static Transform LHand
    {
        get
        {
            if (_lHand) return _lHand;
#if PC
            var cam = MainCamera;
            if (!cam) return null;

            var handObj = new GameObject("LHand");
            _lHand = handObj.transform;
            _lHand.SetParent(cam.transform);
#elif Oculus
            var found = GameObject.Find("LeftControllerAnchor");
            if (!found) return null;
            _lHand = found.transform;
#endif
            return _lHand;
        }
    }

    // 오른쪽 컨트롤러
    private static Transform _rHand;
    // 씬에 등록된 오른쪽 컨트롤러 찾아 반환
    public static Transform RHand
    {
        get
        {
            // 만약 rHand에 값이 없을경우
            if (_rHand) return _rHand;
#if PC
            var cam = MainCamera;
            if (!cam) return null;
            
            var handObj = new GameObject("RHand");
            _rHand = handObj.transform;
            _rHand.parent = cam.transform;
#elif Oculus
            var found = GameObject.Find("RightControllerAnchor");
            if (!found) return null;
            _rHand = found.transform;
#endif
            return _rHand;
        }
    }

    public static Vector3 RHandPosition
    {
        get
        {
#if PC
            var cam = MainCamera;
            if (!cam)
                return Vector3.zero;

            Vector3 pos = Input.mousePosition;
            pos.z = 0.7f;
            pos = cam.ScreenToWorldPoint(pos);

            if (RHand != null)
                RHand.position = pos;

            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#endif
        }
    }

    public static Vector3 RHandDirection
    {
        get
        {
#if PC
            var cam = MainCamera;
            if (!cam || !RHand) return Vector3.zero;
            
            Vector3 direction = RHandPosition - cam.transform.position;
            RHand.forward = direction;
            return direction;

#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.
            RTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);
            return direction;
#endif
        }
    }

    public static Vector3 LHandPosition
    {
        get
        {
#if PC
            var cam = MainCamera;
            if (!cam || !LHand) return Vector3.zero;
            
            // 마우스의 스크린 좌표 얻어오기
            Vector3 pos = Input.mousePosition;
            // z 값은 0.7m로 설정
            pos.z = 0.7f;
            // 스크린 좌표를 월드 좌표로 변환
            pos = cam.ScreenToWorldPoint(pos);
            if (LHand)
                LHand.position = pos;
            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#endif
        }
    }

    public static Vector3 LHandDirection
    {
        get
        {
#if PC
            var cam = MainCamera;
            if(!cam) return Vector3.zero;
            
            Vector3 direction = LHandPosition - cam.transform.position;
            LHand.forward = direction;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.
            LTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);
            return direction;
#endif
        }
    }

    // 컨트롤러의 특정 버튼을 누르고 있는 동안 true를 반환
    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        // virtualMask에 들어온 값을 ButtonTarget 타입으로 변환해 전달한다.
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // 컨트롤러의 특정 버튼을 눌렀을 때 true를 반환
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // 컨트롤러의 특정 버튼을 눌렀다 떼었을 때 true를 반환
    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // 컨트롤러의 Axis 입력을 반환
    // axis: Horizontal, Vertical 값을 갖는다.
    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#elif Oculus
        if (axis == "Horizontal" || axis == "Mouse X")
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)
            hand).x;
        }
        else
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)
            hand).y;
        }
#endif
    }

    // 컨트롤러에 진동 호출하기
    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibration(0.06f, 1, 1, hand);
#endif
    }

    // 컨트롤러에 진동 호출하기
    // duration: 지속 시간, frequency: 빈도, amplify: 진폭, hand: 왼쪽 혹은 오른쪽 컨트롤러
    public static void PlayVibration(float duration, float frequency, float amplitude,
    Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance == null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }
        // 이미 플레이중인 진동 코루틴은 정지
        CoroutineInstance.coroutineInstance.StopAllCoroutines();
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#endif
    }

    // 카메라가 바라보는 방향을 기준으로 센터를 잡는다.
    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#endif
    }

    // 원하는 방향으로 타깃의 센터를 설정
    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }

#if PC
    private static readonly Vector3 OriginScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif
    // 광선 레이가 닿는 곳에 크로스헤어를 위치시키고 싶다.
    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller hand = Controller.RTouch)
    {
        if (!crosshair) return;
        
        Ray ray;
        var cam = MainCamera;
        if (!cam) return;
        
        // 컨트롤러의 위치와 방향을 이용해 레이 제작
        if (isHand)
        {
#if PC
            ray = cam.ScreenPointToRay(Input.mousePosition);
#else
            if (hand == Controller.RTouch)
                ray = new Ray(RHandPosition, RHandDirection);
            else
                ray = new Ray(LHandPosition, LHandDirection);
#endif
        }
        else
        {
            // 카메라를 기준으로 화면의 정중앙으로 레이를 제작
            ray = new Ray(cam.transform.position, cam.transform.forward);
        }

        // 눈에 안 보이는 Plane을 만든다.
        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        // plane을 이용해 ray를 쏜다.
        if (plane.Raycast(ray, out distance))
        {
            // 레이의 GetPoint 함수를 이용해 충돌 지점의 위치를 가져온다.
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -cam.transform.forward;
            // 크로스헤어의 크기를 최소 기본 크기에서 거리에 따라 더 커지도록 한다
            crosshair.localScale = OriginScale * Mathf.Max(1, distance);
        }
        else
        {
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -cam.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = OriginScale * Mathf.Max(1, distance);
        }
    }

#if Oculus
    static IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude,
    Controller hand)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            OVRInput.SetControllerVibration(frequency, amplitude, (OVRInput.Controller)
            hand);
            yield return null;
        }
        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)hand);
    }
#endif
}


// ARAVRInput 클래스에서 사용할 코루틴 객체
class CoroutineInstance : MonoBehaviour
{
    public static CoroutineInstance coroutineInstance;
    
    private void Awake()
    {
        if (coroutineInstance && coroutineInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        coroutineInstance = this;
        DontDestroyOnLoad(gameObject);
    }
}

