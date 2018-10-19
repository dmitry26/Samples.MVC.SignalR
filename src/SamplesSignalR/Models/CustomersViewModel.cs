using System.Collections.Generic;
using SamplesSignalR.Backend;

namespace SamplesSignalR.Models
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(IList<Customer> customers)
        {
            Customers = customers;
        }

        public IList<Customer> Customers { get; private set; }
    }
}