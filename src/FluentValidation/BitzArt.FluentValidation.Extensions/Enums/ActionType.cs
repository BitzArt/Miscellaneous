using System.Runtime.Serialization;

namespace FluentValidation;

public enum ActionType : byte
{
    [EnumMember(Value = ActionTypes.Get)]
    Get = 1,

    [EnumMember(Value = ActionTypes.Create)]
    Create = 2,

    [EnumMember(Value = ActionTypes.Update)]
    Update = 3,

    [EnumMember(Value = ActionTypes.Patch)]
    Patch = 4,

    [EnumMember(Value = ActionTypes.Options)]
    Options = 5,

    [EnumMember(Value = ActionTypes.Delete)]
    Delete = 6
}
