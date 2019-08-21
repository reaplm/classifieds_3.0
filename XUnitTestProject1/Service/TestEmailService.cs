using Classifieds.Service;
using Classifieds.Service.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Classifieds.XUnitTest.Service
{
    public class TestEmailService
    {
        private IOptions<EmailSettings> options;
        private ServiceProvider provider;

        public TestEmailService()
        {
            Initialize();
            options = provider.GetService<IOptions<EmailSettings>>();
        }
        /// <summary>
        /// Test for {public async Task SendEmailAsync(string email, string subject, string message)}
        /// Test when there is no exception
        /// Tested using Papercut fake SMTP server
        /// </summary>
        [Fact]
        public void SendEmailAsync_NoExceptionAsync()
        {
            var emailService = new EmailService(options);

            var result = emailService.SendEmailAsync("pdm.molefe@gmail.com", "Testing Email", "This is just a test");
            result.GetAwaiter().GetResult();

            Assert.True(result.IsCompleted);
        }
        /// <summary>
        /// Test for {public async Task SendEmailAsync(string email, string subject, string message)}
        /// Test when there is an exception. 
        /// Note: Setting EnableSsl to true in EmailSettings will cause an exception 
        /// Tested using Papercut fake SMTP server
        /// </summary>
        [Fact]
        public void SendEmailAsync_ExceptionThrown() 
        {

                var emailService = new EmailService(options);

            Assert.ThrowsAsync<InvalidOperationException>(() =>
                emailService.SendEmailAsync("pdm.molefe@gmail.com",
                "Testing Email", "This is just a test"));

        }
        /// <summary>
        /// Initialize Email Settings defined in appsettings.json
        /// "EmailSettings": {
        ///     "MailServer": "localhost",
        ///     "MailPort": 25,
        ///     "SenderName": "Classifieds Registration",
        ///     "Sender": "pdm.molefe@gmail.com",
        /// }
        /// </summary>
        private void Initialize()
        {

            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            provider = services.BuildServiceProvider();
            

            
        }
    }
}
