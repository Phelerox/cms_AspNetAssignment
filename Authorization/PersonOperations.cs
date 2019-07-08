using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace cms.Authorization
{
	public static class ContactOperations
	{
		public static OperationAuthorizationRequirement Create =
		  new OperationAuthorizationRequirement {Name=Constants.CreateOperationName};
		public static OperationAuthorizationRequirement Read =
		  new OperationAuthorizationRequirement {Name=Constants.ReadOperationName};
		public static OperationAuthorizationRequirement Update =
		  new OperationAuthorizationRequirement {Name=Constants.UpdateOperationName};
		public static OperationAuthorizationRequirement Delete =
		  new OperationAuthorizationRequirement {Name=Constants.DeleteOperationName};
		public static OperationAuthorizationRequirement Promote =
		  new OperationAuthorizationRequirement {Name=Constants.PromoteOperationName};
        public static OperationAuthorizationRequirement Demote =
		  new OperationAuthorizationRequirement {Name=Constants.DemoteOperationName};
		public static OperationAuthorizationRequirement Censor =
		  new OperationAuthorizationRequirement {Name=Constants.CensorOperationName};
	}

	public class Constants
	{
		public static readonly string CreateOperationName = "Create";
		public static readonly string ReadOperationName = "Read";
		public static readonly string UpdateOperationName = "Update";
		public static readonly string DeleteOperationName = "Delete";
		public static readonly string PromoteOperationName = "Promote";
        public static readonly string DemoteOperationName = "Demote";
		public static readonly string CensorOperationName = "Censor";

		public static readonly string AdministratorsRole = "ContactAdministrators";
	}
}