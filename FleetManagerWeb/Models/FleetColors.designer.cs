﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FleetManagerWeb.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SVLL_ETS")]
	public partial class FleetColorsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertFleetColors(FleetColors instance);
    partial void UpdateFleetColors(FleetColors instance);
    partial void DeleteFleetColors(FleetColors instance);
        #endregion

        public FleetColorsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FleetColorsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FleetColorsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FleetColorsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<FleetColors> FleetColors
		{
			get
			{
				return this.GetTable<FleetColors>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InsertOrUpdateFleetColors")]
		public ISingleResult<InsertOrUpdateFleetColorsResult> InsertOrUpdateFleetColors([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="FleetColorsName", DbType="NVarChar(100)")] string FleetColorsname, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="BigInt")] System.Nullable<long> userid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="BigInt")] System.Nullable<long> pageid)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, FleetColorsname, userid, pageid);
			return ((ISingleResult<InsertOrUpdateFleetColorsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetFleetColorsAll")]
		public ISingleResult<GetFleetColorsAllResult> GetFleetColorsAll()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetFleetColorsAllResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetFleetColorsById")]
		public ISingleResult<GetFleetColorsByIdResult> GetFleetColorsById([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<GetFleetColorsByIdResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.DeleteFleetColors")]
		public ISingleResult<DeleteFleetColorsResult> DeleteFleetColors([global::System.Data.Linq.Mapping.ParameterAttribute(Name="IdList", DbType="NVarChar(MAX)")] string idlist, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DeletedBy", DbType="BigInt")] System.Nullable<long> deletedby, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="BigInt")] System.Nullable<long> pageid)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), idlist, deletedby, pageid);
			return ((ISingleResult<DeleteFleetColorsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CountFleetColors")]
		public ISingleResult<CountFleetColorsResult> CountFleetColors()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<CountFleetColorsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchFleetColors")]
		public ISingleResult<SearchFleetColorsResult> SearchFleetColors([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Rows", DbType="Int")] System.Nullable<int> rows, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Page", DbType="Int")] System.Nullable<int> page, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Search", DbType="NVarChar(500)")] string search, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sort", DbType="NVarChar(50)")] string sort)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), rows, page, search, sort);
			return ((ISingleResult<SearchFleetColorsResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.FleetColors")]
	public partial class FleetColors : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _FleetColorsName;
		
		private System.DateTime _CreatedOn;
		
		private long _CreatedBy;
		
		private System.Nullable<System.DateTime> _UpdatedOn;
		
		private System.Nullable<long> _UpdatedBy;
		
		private System.Nullable<System.DateTime> _DeletedOn;
		
		private System.Nullable<long> _DeletedBy;
		
		private bool _IsDeleted;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnFleetColorsNameChanging(string value);
    partial void OnFleetColorsNameChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    partial void OnCreatedByChanging(long value);
    partial void OnCreatedByChanged();
    partial void OnUpdatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnUpdatedOnChanged();
    partial void OnUpdatedByChanging(System.Nullable<long> value);
    partial void OnUpdatedByChanged();
    partial void OnDeletedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnDeletedOnChanged();
    partial void OnDeletedByChanging(System.Nullable<long> value);
    partial void OnDeletedByChanged();
    partial void OnIsDeletedChanging(bool value);
    partial void OnIsDeletedChanged();
    #endregion
		
		public FleetColors()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FleetColorsName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string FleetColorsName
		{
			get
			{
				return this._FleetColorsName;
			}
			set
			{
				if ((this._FleetColorsName != value))
				{
					this.OnFleetColorsNameChanging(value);
					this.SendPropertyChanging();
					this._FleetColorsName = value;
					this.SendPropertyChanged("FleetColorsName");
					this.OnFleetColorsNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedBy", DbType="BigInt NOT NULL")]
		public long CreatedBy
		{
			get
			{
				return this._CreatedBy;
			}
			set
			{
				if ((this._CreatedBy != value))
				{
					this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> UpdatedOn
		{
			get
			{
				return this._UpdatedOn;
			}
			set
			{
				if ((this._UpdatedOn != value))
				{
					this.OnUpdatedOnChanging(value);
					this.SendPropertyChanging();
					this._UpdatedOn = value;
					this.SendPropertyChanged("UpdatedOn");
					this.OnUpdatedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedBy", DbType="BigInt")]
		public System.Nullable<long> UpdatedBy
		{
			get
			{
				return this._UpdatedBy;
			}
			set
			{
				if ((this._UpdatedBy != value))
				{
					this.OnUpdatedByChanging(value);
					this.SendPropertyChanging();
					this._UpdatedBy = value;
					this.SendPropertyChanged("UpdatedBy");
					this.OnUpdatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeletedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> DeletedOn
		{
			get
			{
				return this._DeletedOn;
			}
			set
			{
				if ((this._DeletedOn != value))
				{
					this.OnDeletedOnChanging(value);
					this.SendPropertyChanging();
					this._DeletedOn = value;
					this.SendPropertyChanged("DeletedOn");
					this.OnDeletedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeletedBy", DbType="BigInt")]
		public System.Nullable<long> DeletedBy
		{
			get
			{
				return this._DeletedBy;
			}
			set
			{
				if ((this._DeletedBy != value))
				{
					this.OnDeletedByChanging(value);
					this.SendPropertyChanging();
					this._DeletedBy = value;
					this.SendPropertyChanged("DeletedBy");
					this.OnDeletedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDeleted", DbType="Bit NOT NULL")]
		public bool IsDeleted
		{
			get
			{
				return this._IsDeleted;
			}
			set
			{
				if ((this._IsDeleted != value))
				{
					this.OnIsDeletedChanging(value);
					this.SendPropertyChanging();
					this._IsDeleted = value;
					this.SendPropertyChanged("IsDeleted");
					this.OnIsDeletedChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class InsertOrUpdateFleetColorsResult
	{
		
		private long _InsertedId;
		
		public InsertOrUpdateFleetColorsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InsertedId", DbType="BigInt NOT NULL")]
		public long InsertedId
		{
			get
			{
				return this._InsertedId;
			}
			set
			{
				if ((this._InsertedId != value))
				{
					this._InsertedId = value;
				}
			}
		}
	}
	
	public partial class GetFleetColorsAllResult
	{
		
		private long _Id;
		
		private string _FleetColorsName;
		
		public GetFleetColorsAllResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FleetColorsName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string FleetColorsName
		{
			get
			{
				return this._FleetColorsName;
			}
			set
			{
				if ((this._FleetColorsName != value))
				{
					this._FleetColorsName = value;
				}
			}
		}
	}
	
	public partial class GetFleetColorsByIdResult
	{
		
		private long _Id;
		
		private string _FleetColorsName;
		
		public GetFleetColorsByIdResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FleetColorsName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string FleetColorsName
		{
			get
			{
				return this._FleetColorsName;
			}
			set
			{
				if ((this._FleetColorsName != value))
				{
					this._FleetColorsName = value;
				}
			}
		}
	}
	
	public partial class DeleteFleetColorsResult
	{
		
		private int _TotalReference;
		
		private string _Name;
		
		public DeleteFleetColorsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalReference", DbType="Int NOT NULL")]
		public int TotalReference
		{
			get
			{
				return this._TotalReference;
			}
			set
			{
				if ((this._TotalReference != value))
				{
					this._TotalReference = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
	
	public partial class CountFleetColorsResult
	{
		
		private int _Result;
		
		public CountFleetColorsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Result", DbType="Int NOT NULL")]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				if ((this._Result != value))
				{
					this._Result = value;
				}
			}
		}
	}
	
	public partial class SearchFleetColorsResult
	{
		
		private System.Nullable<int> _RowNum;
		
		private int _Total;
		
		private long _Id;
		
		private string _FleetColorsName;
		
		public SearchFleetColorsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RowNum", DbType="Int")]
		public System.Nullable<int> RowNum
		{
			get
			{
				return this._RowNum;
			}
			set
			{
				if ((this._RowNum != value))
				{
					this._RowNum = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Total", DbType="Int NOT NULL")]
		public int Total
		{
			get
			{
				return this._Total;
			}
			set
			{
				if ((this._Total != value))
				{
					this._Total = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FleetColorsName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string FleetColorsName
		{
			get
			{
				return this._FleetColorsName;
			}
			set
			{
				if ((this._FleetColorsName != value))
				{
					this._FleetColorsName = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
