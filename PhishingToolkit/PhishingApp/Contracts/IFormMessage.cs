using System.ServiceModel;
using System.ServiceModel.Web;

namespace Contracts
{
    [ServiceContract]
    public interface IFormMessage
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/FormData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare
        )]
        void ReceiveData(FormData data);

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void HandleOptions();
    }
}
