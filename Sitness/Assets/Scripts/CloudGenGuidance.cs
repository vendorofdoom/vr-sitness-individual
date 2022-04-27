using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudGenGuidance : MonoBehaviour
{
    [SerializeField]
    private MovementState movementState;

    public float PullApartTime;
    public float SwooshTime;

    public Transform GuideParentLeft;
    public Transform GuideParentRight;
    public Transform MinRotation;
    public Transform MaxRotationLeft;
    public Transform MaxRotationRight;

    private float TimeInMovement = 0f;

    public Image LGImage;
    public Image RGImage;
    public Sprite LG;
    public Sprite RG;
    public Sprite LGPressed;
    public Sprite RGPressed;
    
    private bool guidanceEnabled;

    private void Awake()
    {
        guidanceEnabled = PlayerPrefs.GetInt("guidanceEnabled", 1) == 1 ? true : false;
        if (!guidanceEnabled)
        {
            this.enabled = false;
        }
    }


    private enum MovementState
    {
        Pause,
        PullApart,
        Release,
        Swoosh
    }

    private void OnEnable()
    {
        if (guidanceEnabled)
        {
            GuideParentLeft.gameObject.SetActive(true);
            GuideParentRight.gameObject.SetActive(true);
        }
}

    private void OnDisable()
    {
        if (GuideParentLeft != null)
            GuideParentLeft.gameObject.SetActive(false);
        if (GuideParentRight != null)
            GuideParentRight.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        HideGripIcons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (movementState)
        {
            case MovementState.Pause:
                HideGripIcons();
                break;
            case MovementState.PullApart:
                DisplayGripIcons(true);
                MoveGuidesApart();
                break;
            case MovementState.Release:
                DisplayGripIcons(false);
                break;
            case MovementState.Swoosh:
                HideGripIcons();
                MoveGuidesTogether();
                break;
        }

        TimeInMovement += Time.deltaTime;
    }

    private void HideGripIcons()
    {
        LGImage.enabled = false;
        RGImage.enabled = false;
    }

    private void DisplayGripIcons(bool pressed)
    {
        if (pressed)
        {
            RGImage.sprite = RGPressed;
            LGImage.sprite = LGPressed;
            SetImageAlpha(RGImage, 0.5f);
            SetImageAlpha(LGImage, 0.5f);
        }
        else
        {
            RGImage.sprite = RG;
            LGImage.sprite = LG;
            SetImageAlpha(RGImage, 0.1f);
            SetImageAlpha(LGImage, 0.1f);
        }

        LGImage.enabled = true;
        RGImage.enabled = true;
    }

    private void MoveGuidesApart()
    {
        GuideParentLeft.rotation = Quaternion.Lerp(MinRotation.rotation, MaxRotationLeft.rotation, TimeInMovement / PullApartTime);
        GuideParentRight.rotation = Quaternion.Lerp(MinRotation.rotation, MaxRotationRight.rotation, TimeInMovement / PullApartTime);
    }

    private void MoveGuidesTogether()
    {
        GuideParentLeft.rotation = Quaternion.Lerp(MaxRotationLeft.rotation, MinRotation.rotation, TimeInMovement / SwooshTime);
        GuideParentRight.rotation = Quaternion.Lerp(MaxRotationRight.rotation, MinRotation.rotation, TimeInMovement / SwooshTime);
    }

    public void Pause()
    {
        movementState = MovementState.Pause;
        TimeInMovement = 0f;
    }

    public void PullApart()
    {
        movementState = MovementState.PullApart;
        TimeInMovement = 0f;
    }

    public void Release()
    {
        movementState = MovementState.Release;
        TimeInMovement = 0f;
    }

    public void Swoosh()
    {
        movementState = MovementState.Swoosh;
        TimeInMovement = 0f;
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }
    
}
