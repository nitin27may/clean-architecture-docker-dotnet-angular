namespace Contact.Api.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ActivityLogAttribute : Attribute
{
    public string ActivityDescription { get; }

    public ActivityLogAttribute(string activityDescription)
    {
        ActivityDescription = activityDescription;
    }
}
