using CourierService.Contracts;
using CourierService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryCostEstimatorCLI
{
    public class FileOfferStore : IOfferStore
    {
        IConfigurationRoot _configuration;

        public FileOfferStore(IConfigurationRoot configurationRoot)
        {
            _configuration = configurationRoot;
        }

        public Offer GetOffer(string offerCode)
        {
            var offers = _configuration.GetSection(Constants.ConfigurationSections.Offers);
            return offers.GetSection(offerCode).Get<Offer>();
        }
    }
}
