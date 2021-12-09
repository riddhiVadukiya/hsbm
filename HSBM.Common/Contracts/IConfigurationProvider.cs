namespace HSBM.Common.Contracts
{
    public interface IConfigurationProvider
    {
        string GetConfigurationValue(string key);
    }
}
