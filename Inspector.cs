using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Zus
{
    public class SoapInspectionEvengArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class SoapClientInspector : IClientMessageInspector
    {
        public event EventHandler<SoapInspectionEvengArgs> SoapReceivedResponse;
        public event EventHandler<SoapInspectionEvengArgs> SoapSendingRequest;

        protected virtual void OnReceivedResponse(SoapInspectionEvengArgs e)
        {
            SoapReceivedResponse?.Invoke(this, e);
        }

        protected virtual void OnSendingRequest(SoapInspectionEvengArgs e)
        {
            SoapSendingRequest?.Invoke(this, e);
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            SoapInspectionEvengArgs args = new SoapInspectionEvengArgs();

            args.Message = reply.ToString();

            OnReceivedResponse(args);
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            SoapInspectionEvengArgs args = new SoapInspectionEvengArgs();

            args.Message = request.ToString();

            OnSendingRequest(args);

            return null;
        }
    }

    public class InspectorBehavior : IEndpointBehavior
    {
        public string SoapMessageResponse { get; private set; }
        public string SoapMessageRequest { get; private set; }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            SoapClientInspector inspector = new SoapClientInspector();

            inspector.SoapSendingRequest += (o, e) =>
            {
                SoapMessageRequest = e.Message;
                Console.WriteLine($"############ REQUEST:\n\n{SoapMessageRequest}\n");
            };
            inspector.SoapReceivedResponse += (o, e) =>
            {
                SoapMessageResponse = e.Message;
                Console.WriteLine($"############ RESPONSE:\n\n{SoapMessageResponse}\n");
            };

            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
