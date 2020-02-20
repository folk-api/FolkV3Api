using System;
using System.ServiceModel;
using Eu.Xroad.UsFolkV3.Producer;

namespace Us.FolkV3.Api.Client
{
    public static class FolkClient
    {
        private static string PortAddressTpl { get; } = $"http{{0}}://{{1}}{Const.ConsumerProxyPath}";

        internal static UsFolkPortTypeClient WebService(string consumerHost, bool secure)
        {
            Util.RequireNonNull(consumerHost, "consumerHost");
            try
            {
                return new UsFolkPortTypeClient(
                    new BasicHttpBinding(secure ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None),
                    EndpointAddress(Util.RequireNonNull(consumerHost, "consumerHost"), secure)
                    );
            }
            catch (Exception e)
            {
                throw new FolkApiException("Could not create instance", e);
            }
        }

        public static PersonSmallClient PersonSmall(string consumerHost, string consumerName)
        {
            return new PersonSmallClient(consumerHost, consumerName);
        }

        public static PersonMediumClient PersonMedium(string consumerHost, string consumerName)
        {
            return new PersonMediumClient(consumerHost, consumerName);
        }

        public static PrivateCommunityClient PrivateCommunity(string consumerHost, string consumerName)
        {
            return new PrivateCommunityClient(consumerHost, consumerName);
        }

        public static PublicCommunityClient PublicCommunity(string consumerHost, string consumerName)
        {
            return new PublicCommunityClient(consumerHost, consumerName);
        }

        private static EndpointAddress EndpointAddress(string consumerHost, bool secure)
        {
            return new EndpointAddress(string.Format(PortAddressTpl, secure ? "s" : "", consumerHost));
        }

    }
}
