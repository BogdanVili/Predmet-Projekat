using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace PhishingApp.Config
{
    public class CorsBehavior : IEndpointBehavior, IDispatchMessageInspector
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime) { }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
        }
        public void Validate(ServiceEndpoint endpoint) { }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            var httpHeader = reply.Properties["httpResponse"] as System.ServiceModel.Channels.HttpResponseMessageProperty;
            if (httpHeader != null)
            {
                httpHeader.Headers.Add("Access-Control-Allow-Origin", "*");
                httpHeader.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
                httpHeader.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            }
        }
    }

}
