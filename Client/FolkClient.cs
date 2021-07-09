using System;
using System.ServiceModel;
using Eu.Xroad.UsFolkV3.Producer;

namespace Us.FolkV3.Api.Client
{
    public static class FolkClient
    {

        internal static UsFolkPortTypeClient WebService(string host, bool secure)
        {
            Util.RequireNonNull(host, "host");
            try
            {
                return new UsFolkPortTypeClient(
                    new BasicHttpBinding(secure ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None),
                    EndpointAddress(host, secure)
                    );
            }
            catch (Exception e)
            {
                throw new FolkApiException("Could not create instance", e);
            }
        }

        public static PersonSmallClient PersonSmall(HeldinConfig config)
        {
            return new PersonSmallClient(config);
        }

        public static PersonMediumClient PersonMedium(HeldinConfig config)
        {
            return new PersonMediumClient(config);
        }

        public static PrivateCommunityClient PrivateCommunity(HeldinConfig config)
        {
            return new PrivateCommunityClient(config);
        }

        public static PublicCommunityClient PublicCommunity(HeldinConfig config)
        {
            return new PublicCommunityClient(config);
        }

        private static EndpointAddress EndpointAddress(string host, bool secure)
        {
            return new EndpointAddress(string.Format("http{0}://{1}", secure ? "s" : "", host));
        }

    }
}
