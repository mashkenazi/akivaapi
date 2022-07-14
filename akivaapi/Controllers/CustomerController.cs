using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using System.Threading.Tasks;


namespace akivaapi.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        public static SocketsHttpHandler handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        //public static handler = new HttpClientHandler(),
        //handler.Credentials = new System.Net.NetworkCredential("admin@client", "admin");
        //private static readonly HttpClient httpClient = new HttpClient(handler);


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
 
    };

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }
        /*
        [HttpGet("GetWeatherForecast")]
        public IEnumerable<akivaapi.CustomerController> GetWeather()
        {
            return Enumerable.Range(1, 5).Select(index => new CustomerController
            {
                Date = DateTime.Now.AddDays(index),
                //TemperatureC = Random.Shared.Next(-20, 55),
                TemperatureC = 22,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        } 
        */

        [HttpPost("CreateNewCustomer")]
        public Customer CreateNewCustomer(string CustomerType, Company CompanyData, Person PersonData, string Country,
            string ManagingPartnerId, string VerticalCode, Contact[] Contacts)
        {
            //want to make the "Contacts" argument optional
            // input -> customer 
            Customer customer = new Customer
            {
                customerType = CustomerType,
                company = CompanyData,
                person = PersonData,
                country = Country,
                managingPartnerId = ManagingPartnerId,
                verticalCode = VerticalCode,
                contacts = Contacts
            };
            // insert logic 
            // save to external server 
            insertCustomerToExternalServer(customer);
            // save to db 
            insertCustomerToDb(customer);
            return new Customer
            {
                customerType = CustomerType,
                company = new Company
                {
                    companyRegistrationNumber = CompanyData.companyRegistrationNumber,
                    companyName = CompanyData.companyName,
                    country = CompanyData.country,
                    companyVATNumber = CompanyData.companyVATNumber
                },
                person = new Person
                {
                    personRegistrationEmail = PersonData.personRegistrationEmail,
                    personName = PersonData.personName
                },
                country = Country,
                managingPartnerId = ManagingPartnerId,
                verticalCode = VerticalCode,
                contacts = Enumerable.Range(1, Contacts.Length).Select(index => new Contact
                {
                    //PLACEHOLDER
                    firstName = "Test",
                    lastName = "Test",
                    email = "Test",
                    phone = "Test",
                    language = "Test",
                    description = "Test",
                    partnerId = "Test"
                }).ToArray()

        };
        }

        [HttpGet("GetCustomer")]
        public async Task<Customer> GetCustomerAsync(string ManagingPartnerId)
        {
            await connect();
            //search for Customer
            Customer myCustomer = getCustomerFromExternalServer(ManagingPartnerId);
            if (myCustomer != null)
            {
                return myCustomer;
            }
            else
            {
                return getCustomerFromDb(ManagingPartnerId);
            }
        }


        private bool insertCustomerToExternalServer(Customer customer)
        {
            //connect to Dexter Create Customer Post API at https://dx-test.eset.com/api/customer-catalog/v1/customers and post customer
            return false; //todo
        }


        private Customer getCustomerFromExternalServer(String name)
        {
            //connect to Dexter Get Customer Get API at https://dx-test.eset.com/api/customer-catalog/v1/customers/{customerId}
            //return customer
            return null; //todo
        }


        private bool insertCustomerToDb(Customer customer)
        {
            //Connect to DB
            //Insert customer to DB
            return false; //todo
        }


        private Customer getCustomerFromDb(String ManagingPartnerId)
        {
            //Connect to DB
            //Search DB for customer by ManagingPartnerId
            //return customer if exists, else null
            return null; //todo
        }

        static async Task connect()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            
            HttpClient client = new HttpClient(handler);
            var headerVal = Convert.ToBase64String(Encoding.UTF8.GetBytes("Dor@eset.co.il:SurXruxh2022"));
            var header = new AuthenticationHeaderValue("Basic", headerVal);
            //client.DefaultRequestHeaders.Authorization = header;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", headerVal);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + headerVal);
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://dx-test.eset.com/api/customer-catalog/v1/customers/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            } 

        }
    }
}