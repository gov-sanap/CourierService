using CourierService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Contracts
{
    public interface IOfferStore
    {
        Offer GetOffer(string offerCode);
    }
}
