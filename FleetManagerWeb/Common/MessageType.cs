namespace FleetManagerWeb.Common
{
    public enum MessageType
    {
        Success = 1,
        Fail = 2,
        DeleteSucess = 3,
        DeleteFail = 4,
        DeletePartial = 5,
        AlreadyExist = 6,
        InputRequired = 7,
        SelectRequired = 8,
        RecordInGridRequired = 9,
        CanNotUpdate = 10
    }
}