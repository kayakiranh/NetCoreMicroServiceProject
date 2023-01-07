using Microsoft.Extensions.Configuration;

namespace MP.UserInterface.CoreUI.Models
{
    public class GatewayClientHelper
    {
        private readonly IConfiguration _configuration;

        public GatewayClientHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public enum Actions
        {
            CreditCardInsert,
            CreditCardUpdate,
            CreditCardRemove,
            CreditCardList,
            CreditCardListByFinancialScore,
            CreditCardListByType,
            CreditCardListByBank,
            CustomerInsert,
            CustomerUpdate,
            CustomerRemove,
            CustomerList,
            CustomerLogin,
            CustomerByToken,
            CustomerByIdentity,
            FinancialRatingCalculate,
            AuthLogin,
            AuthCheck,
            AuthRefresh,
            AuthTestUser
        }

        public string SetUrl(Actions action)
        {
            string environment = _configuration.GetSection("Environment").Value;
            string baseUrl = string.Empty;
            switch (environment)
            {
                case "Dev":
                    baseUrl = "http://localhost:51214";
                    break;

                case "Test":
                    baseUrl = "https://test.microservisproject.com";
                    break;

                case "PreProd":
                    baseUrl = "https://preprod.microservisproject.com";
                    break;

                case "Prod":
                    baseUrl = "https://microservisproject.com";
                    break;

                default:
                    break;
            }

            string afterUrl = string.Empty;
            switch (action)
            {
                case Actions.CreditCardInsert:
                    afterUrl = "/creditcard/insert";
                    break;

                case Actions.CreditCardUpdate:
                    afterUrl = "/creditcard/update";
                    break;

                case Actions.CreditCardRemove:
                    afterUrl = "/creditcard/remove";
                    break;

                case Actions.CreditCardList:
                    afterUrl = "/creditcard/list";
                    break;

                case Actions.CreditCardListByFinancialScore:
                    afterUrl = "/creditcard/by-financial-score";
                    break;

                case Actions.CreditCardListByType:
                    afterUrl = "/creditcard/by-type";
                    break;

                case Actions.CreditCardListByBank:
                    afterUrl = "/creditcard/by-bank";
                    break;

                case Actions.CustomerInsert:
                    afterUrl = "/customer/insert";
                    break;

                case Actions.CustomerUpdate:
                    afterUrl = "/customer/update";
                    break;

                case Actions.CustomerRemove:
                    afterUrl = "/customer/remove";
                    break;

                case Actions.CustomerList:
                    afterUrl = "/customer/list";
                    break;

                case Actions.CustomerLogin:
                    afterUrl = "/customer/login";
                    break;

                case Actions.CustomerByToken:
                    afterUrl = "/customer/by-token";
                    break;

                case Actions.CustomerByIdentity:
                    afterUrl = "/customer/by-identity";
                    break;

                case Actions.FinancialRatingCalculate:
                    afterUrl = "/financialrating/calculate";
                    break;

                case Actions.AuthLogin:
                    afterUrl = "/auth/login";
                    break;

                case Actions.AuthCheck:
                    afterUrl = "/auth/check";
                    break;

                case Actions.AuthRefresh:
                    afterUrl = "/auth/refresh";
                    break;

                case Actions.AuthTestUser:
                    afterUrl = "/auth/test-user";
                    break;

                default:
                    break;
            }

            return baseUrl + afterUrl;
        }
    }
}