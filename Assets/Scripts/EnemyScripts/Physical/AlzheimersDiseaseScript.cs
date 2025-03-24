using Pathfinding;
using UnityEngine;

public class AlzheimersDiseaseScript : EnemyScript
{
    [SerializeField] private Transform ForgotTarget;
    [SerializeField] private AIDestinationSetter AIDSetter;

    private bool forgot = false;

    protected override void Start()
    {
        FormSwitch();
    }
    private void FormSwitch()
    {
        if(forgot)
        {
            forgot = false;
            RememberForm();
        }
        else
        {
            forgot = true;
            ForgotForm();
        }
    }
    private void RememberForm()
    {
        AIDSetter.target = PlayerMovement.PM.transform;
        Invoke(nameof(FormSwitch), (Random.Range(30f, 50f) / 10f));
    }
    private void ForgotForm()
    {
        ForgotTarget.localPosition = (PlayerMovement.PM.transform.position - transform.position).normalized;
        AIDSetter.target = ForgotTarget;
        Invoke(nameof(FormSwitch), (Random.Range(10f, 20f) / 10f));
    }
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        FormSwitch();
    }
}
