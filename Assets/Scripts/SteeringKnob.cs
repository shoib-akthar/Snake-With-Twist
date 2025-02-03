using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringKnob : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool IsKnobHeld = false;
    public RectTransform KnobTransform;
    private float CurrentKnobAngle = 0f;
    private float PreviousKnobAngle = 0f;
    private Vector2 KnobCenter;
    public float MaxTurnAngle = 200f;
    public float AutoReturnSpeed = 300f;
    public float SteeringValue;

    void Update()
    {
        if (!IsKnobHeld && CurrentKnobAngle != 0f)
        {
            float AngleAdjustment = AutoReturnSpeed * Time.deltaTime;
            if (Mathf.Abs(AngleAdjustment) > Mathf.Abs(CurrentKnobAngle))
                CurrentKnobAngle = 0f;
            else if (CurrentKnobAngle > 0f)
                CurrentKnobAngle -= AngleAdjustment;
            else
                CurrentKnobAngle += AngleAdjustment;
        }

        KnobTransform.localEulerAngles = new Vector3(0, 0, -MaxTurnAngle * SteeringValue);
        SteeringValue = CurrentKnobAngle / MaxTurnAngle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsKnobHeld = true;
        KnobCenter = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, KnobTransform.position);
        PreviousKnobAngle = Vector2.Angle(Vector2.up, eventData.position - KnobCenter);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float NewKnobAngle = Vector2.Angle(Vector2.up, eventData.position - KnobCenter);
        if ((eventData.position - KnobCenter).sqrMagnitude >= 400)
        {
            if (eventData.position.x > KnobCenter.x)
                CurrentKnobAngle += NewKnobAngle - PreviousKnobAngle;
            else
                CurrentKnobAngle -= NewKnobAngle - PreviousKnobAngle;
        }

        CurrentKnobAngle = Mathf.Clamp(CurrentKnobAngle, -MaxTurnAngle, MaxTurnAngle);
        PreviousKnobAngle = NewKnobAngle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsKnobHeld = false;
    }
}
