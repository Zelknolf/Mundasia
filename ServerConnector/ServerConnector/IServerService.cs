using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Web;

namespace Mundasia.Server.Communication
{
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string Ping();
    }
}
