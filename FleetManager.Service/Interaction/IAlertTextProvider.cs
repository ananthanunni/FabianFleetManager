using FleetManager.Model.Interaction;

namespace FleetManager.Service.Interaction
{
    public interface IAlertTextProvider
    {
	  string AlertMessage(string tableName, MessageType msgType, string message = null);
    }
}
