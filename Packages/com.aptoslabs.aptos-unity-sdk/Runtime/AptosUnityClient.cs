namespace Aptos
{
    /// <summary>
    /// This client is a wrapper around the AptosClient that uses the AptosUnityWebRequestClient instead of the default HttpClient.
    /// The AptosUnityWebRequestClient leverages UnityWebRequest to make HTTP requests.
    /// </summary>
    public class AptosUnityClient : AptosClient
    {
        public AptosUnityClient(AptosConfig config)
            : base(config) { }

        public AptosUnityClient(NetworkConfig networkConfig)
            : base(new AptosConfig(networkConfig, null, new AptosUnityWebRequestClient())) { }
    }
}
