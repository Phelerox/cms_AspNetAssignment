using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace cms.Authorization
{
	public static class PersonOperations
	{
		public static OperationAuthorizationRequirement Create =
		  new OperationAuthorizationRequirement {Name=Constants.CreateOperationName};
		public static OperationAuthorizationRequirement Read =
		  new OperationAuthorizationRequirement {Name=Constants.ReadOperationName};
		public static OperationAuthorizationRequirement Update =
		  new OperationAuthorizationRequirement {Name=Constants.UpdateOperationName};
		public static OperationAuthorizationRequirement Delete =
		  new OperationAuthorizationRequirement {Name=Constants.DeleteOperationName};
		public static OperationAuthorizationRequirement Status =
		  new OperationAuthorizationRequirement {Name=Constants.StatusOperationName};
	}

	public class Constants
	{
		public static readonly string CreateOperationName = "Create";
		public static readonly string ReadOperationName = "Read";
		public static readonly string UpdateOperationName = "Update";
		public static readonly string DeleteOperationName = "Delete";
		public static readonly string StatusOperationName = "Status";

		public static readonly string AdministratorsRole = "Administrator";
	}
}