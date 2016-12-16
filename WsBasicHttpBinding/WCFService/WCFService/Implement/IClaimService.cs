using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;

namespace WCFService.Implement
{
    [ServiceContract]
    public interface IClaimService
    {
        [FaultContract(typeof(ArgumentNullException))]
        [OperationContract]
        IEnumerable<Claim> GetAllClaims();
    }
}