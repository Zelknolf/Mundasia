using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Web;
using System.Security.Cryptography;

namespace Mundasia.Server.Communication
{
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string Ping();

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string GetPublicKey();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate="CreateAccount?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        bool CreateAccount(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Login?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        int Login(string message);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CreateCharacter?message={message}", ResponseFormat = WebMessageFormat.Xml)]
        string CreateCharacter(string message);
    }
}
