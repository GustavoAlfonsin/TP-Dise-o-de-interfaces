using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SimpleUIAnimator :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public enum AnimationMode
    {
        None,
        ScalePunch,
        SquashStretch,
        FadeAlpha,
        ColorBlend
    }

    [Header("Mode")]
    public AnimationMode mode;

    [Header("Targets")]
    public RectTransform target;
    public Graphic graphic;

    [Header("General")]
    public float speed = 8f;

    [Header("Scale Punch")]
    public float hoverScale = 1.1f;

    [Header("Squash Stretch")]
    public Vector2 squashScale =
        new Vector2(1.2f, 0.8f);

    [Header("Fade")]
    [Range(0f, 1f)]
    public float fadedAlpha = 0.2f;

    [Header("Color Blend")]
    public Color startColor = Color.green;
    public Color endColor = Color.red;

    [Range(0f, 100f)]
    public float currentValue = 0f;

    public float maxValue = 100f;

    [Header("Click Bounce")]
    public bool clickBounce = true;

    public float clickScale = 0.9f;

    float targetAmount = 0f;
    float currentAmount = 0f;

    Vector3 originalScale;

    bool isHovering;
    bool isPressing;

    void Awake()
    {
        if (target == null)
            target = transform as RectTransform;

        if (graphic == null)
            graphic = GetComponent<Graphic>();

        originalScale = target.localScale;
    }

    void Update()
    {
        float goal =
            isHovering ? maxValue : 0f;

        targetAmount = Mathf.Lerp(
            targetAmount,
            goal,
            Time.deltaTime * speed
        );

        currentAmount =
            Mathf.Lerp(
                currentAmount,
                targetAmount,
                Time.deltaTime * speed
            );

        float t =
            Mathf.Clamp01(
                currentAmount / maxValue
            );

        switch (mode)
        {
            case AnimationMode.ScalePunch:

                float pressT =
                    isPressing ? clickScale : 1f;

                Vector3 finalScale =
                    Vector3.Lerp(
                        originalScale,
                        originalScale * hoverScale,
                        t
                    );

                finalScale *= pressT;

                target.localScale =
                    Vector3.Lerp(
                        target.localScale,
                        finalScale,
                        Time.deltaTime * speed
                    );

                break;

            case AnimationMode.SquashStretch:

                Vector3 squash =
                    new Vector3(
                        squashScale.x,
                        squashScale.y,
                        1f
                    );

                target.localScale =
                    Vector3.Lerp(
                        originalScale,
                        Vector3.Scale(
                            originalScale,
                            squash
                        ),
                        t
                    );

                break;

            case AnimationMode.FadeAlpha:

                if (graphic != null)
                {
                    Color c = graphic.color;

                    c.a =
                        Mathf.Lerp(
                            1f,
                            fadedAlpha,
                            t
                        );

                    graphic.color = c;
                }

                break;

            case AnimationMode.ColorBlend:

                if (graphic != null)
                {
                    graphic.color =
                        Color.Lerp(
                            startColor,
                            endColor,
                            t
                        );
                }

                break;
        }
    }

    public void OnPointerEnter(
        PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(
        PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerDown(
    PointerEventData eventData)
    {
        isPressing = true;
    }

    public void OnPointerUp(
        PointerEventData eventData)
    {
        isPressing = false;
    }
}