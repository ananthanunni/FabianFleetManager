namespace FleetManager.Model.Interaction
{
    public enum MessageType
    {
        Success = 1,
        Fail = 2,
        DeleteSuccess = 3,
        DeleteFail = 4,
        DeletePartial = 5,
        AlreadyExists = 6,
        InputRequired = 7,
        SelectRequired = 8,
        RecordInGridRequired = 9,
        CanNotUpdate = 10
    }
}