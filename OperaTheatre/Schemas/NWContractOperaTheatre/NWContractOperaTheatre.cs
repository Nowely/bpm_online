namespace Terrasoft.Configuration.ContractOperaTheatre
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using Terrasoft.Core;
    using Terrasoft.Web.Common;
    using Terrasoft.Core.Entities;
    using System.Runtime.Serialization;
    using GetNumberOfPerfomance; 

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractOperaTheatre : BaseService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public int GetNumberOfPerfomance(int CodeOfProgramm)
        {
        	
            return new ContractToGetNumber(UserConnection).GetNumber(CodeOfProgramm);
        }
    }
}