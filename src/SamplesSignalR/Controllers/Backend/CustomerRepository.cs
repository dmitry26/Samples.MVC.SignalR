using System;
using System.Collections.Generic;
using System.Linq;

namespace SamplesSignalR.Backend
{
    public class CustomerRepository
    {
        private static Lazy<IList<Customer>> _customers = new Lazy<IList<Customer>>(() => FindAllInternal());

        public IList<Customer> FindAll()
        {			
			return _customers.Value;
        }

        public void Delete(int id)
        {
            var customers = FindAll();
			var customer = customers.Where(c => c.Id == id).FirstOrDefault();

			if (customer != null)
				customers.Remove(customer);
        }

        private static IList<Customer> FindAllInternal()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var list = new List<Customer>();

			for (var i = 0; i < 5; i++)
            {
                var fakeId = rnd.Next(100, 1000);
                var fakeStreetAddressNo = rnd.Next(1, 50);
                var fakeAddressLen = rnd.Next(8, 20);

				list.Add(new Customer
                {
                    Id = fakeId,
                    Name = String.Format("Customer-{0}", fakeId),
                    Address = String.Format("{0} {1}", fakeStreetAddressNo, RandomString(fakeAddressLen))
                });
            }

            return list.OrderBy(c => c.Name).ToList();
        }

        private static string RandomString(int size)
        {
            var rng = new Random();
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = alphabet[rng.Next(alphabet.Length)];
            }

            return new string(buffer);
        }
    }
}