using Microsoft.Extensions.Configuration;
using System.IO;

namespace MP.Infrastructure.Helper
{
    public static class AppSettingsReadHelper
    {
        public static string ReadByValue(string mainNode, string innerNode = "")
        {
            string directory = @"C:\\Users\\husey\\Documents\\GitHub\\NetCoreMicroServiceProject\\MicroServiceProject\\Source\\UserInterface\\MP.UserInterface.CoreUI";
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(directory).AddJsonFile("appsettings.json").Build();
            
            if (innerNode != "") return configurationRoot.GetSection(mainNode).GetSection(innerNode).Value;
            return configurationRoot.GetSection(mainNode).Value;
        }
    }
}