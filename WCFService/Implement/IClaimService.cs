using System;
using System.ServiceModel;

namespace WCFService.Implement
{
    [ServiceContract]
    public interface IClaimService
    {
        [FaultContract(typeof(ArgumentNullException))]
        [OperationContract]
        string GetAllClaims();
    }
}