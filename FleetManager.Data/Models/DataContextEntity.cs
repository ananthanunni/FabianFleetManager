using FleetManager.Core.Common;
using FleetManager.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web.Mvc;

namespace FleetManager.Data.Models
{
    public abstract class DataContextEntity<TDataContext>
	  where TDataContext : DataContext
    {
	  protected TDataContext objDataContext = null;
	  protected ILogger _logger;
	  protected IMySession _mySession;

	  public DataContextEntity(TDataContext context = null)
	  {
		_logger = DependencyResolver.Current.GetService<ILogger>();
		_mySession = DependencyResolver.Current.GetService<IMySession>();
	  }

	  protected TDataContext GetDataContext()
	  {
		var context= DependencyResolver.Current.GetService<TDataContext>();
		
		return null;
	  }

	  protected virtual TOut Convert<TIn,TOut>(TIn value)
		where TIn:class
		where TOut:class
	  {
		Type objectType = value.GetType();
		Type target = typeof(TOut);
		var x = Activator.CreateInstance(target, false);
		var z = from source in objectType.GetMembers().ToList()
			  where source.MemberType == MemberTypes.Property
			  select source;
		var d = from source in target.GetMembers().ToList()
			  where source.MemberType == MemberTypes.Property
			  select source;
		List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
		   .ToList().Contains(memberInfo.Name)).ToList();
		PropertyInfo propertyInfo;
		object result;
		foreach (var memberInfo in members)
		{
		    propertyInfo = typeof(TOut).GetProperty(memberInfo.Name);
		    result = value.GetType().GetProperty(memberInfo.Name).GetValue(value, null);

		    propertyInfo.SetValue(x, value, null);
		}
		return (TOut)x;
	  }
    }
}