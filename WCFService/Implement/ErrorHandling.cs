using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WCFService.Implement
{
    public class ErrorServiceBehavior : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //throw new NotImplementedException();
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                cd.ErrorHandlers.Add(new ErrorHandling());
            }
        }

        //outros métodos
    }

    public class ErrorHandling : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            try
            {
                Log(error);
            }
            catch (Exception) { }

            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is ArgumentNullException)
            {
                FaultException ex =
                    new FaultException(
                        new FaultReason("O arquivo informado não foi encontrado"),
                        new FaultCode("FileNotFound"));

                fault = Message.CreateMessage(version, ex.CreateMessageFault(), ex.Action);
            }
        }

        private static void Log(Exception error)
        {
            //efetuar o log
        }
    }
}