using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManager.Model.Interaction;

namespace FleetManager.Service.Interaction
{
    public class AlertTextProvider : IAlertTextProvider
    {
	  public string AlertMessage(string tableName, MessageType msgType, string message = null)
	  {
		// TODO: Move to Localization RESX. Lowest priority
		if (msgType == MessageType.Success)
		{
		    return tableName + " Submitted Successfully.";
		}
		else if (msgType == MessageType.Fail)
		{
		    return tableName + " Not Submitted Successfully.";
		}
		else if (msgType == MessageType.DeleteSuccess)
		{
		    return tableName + "(s) Deleted Successfully.";
		}
		else if (msgType == MessageType.DeleteFail)
		{
		    return tableName + "(s) Delete Failure.";
		}
		else if (msgType == MessageType.DeletePartial)
		{
		    if (!string.IsNullOrEmpty(message))
		    {
			  return "Following " + tableName + "(s) Can Not Be Deleted Due To Reference.<br/>" + message;
		    }
		    else
		    {
			  return "Some " + tableName + "(s) Can Not Be Deleted Due To Reference.";
		    }
		}
		else if (msgType == MessageType.AlreadyExists)
		{
		    if (!string.IsNullOrEmpty(message))
		    {
			  return message + " Already Exists.";
		    }
		    else
		    {
			  return tableName + " Already Exists.";
		    }
		}
		else if (msgType == MessageType.InputRequired)
		{
		    return "Please Enter " + tableName + ".";
		}
		else if (msgType == MessageType.SelectRequired)
		{
		    return "Please Select " + tableName + ".";
		}
		else if (msgType == MessageType.CanNotUpdate)
		{
		    return tableName + " has been Approved. So Record Can not be Updated.";
		}
		else
		{
		    return message;
		}
	  }
    }
}
