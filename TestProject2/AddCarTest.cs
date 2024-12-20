using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject2
{
    public class Clients
    {
        public int ClientID { get; set; }
        public string FullName { get; set; }
    }

    public class AddCarWindow
    {
        private ClientCars _car;
        private Clients _customer;

        public AddCarWindow(ClientCars car, Clients customer)
        {
            _car = car;
            _customer = customer;
        }

        public string FullNameTextBoxText { get; set; }
        public string LicensePlateTextBoxText { get; set; }
        public string BrandTextBoxText { get; set; }
        public string ModelTextBoxText { get; set; }
        public string VINTextBoxText { get; set; }
        public string YearTextBoxText { get; set; }

        public void SaveButton_Click()
        {
            var clients = new List<Clients>
            {
                new Clients { ClientID = 1, FullName = "Иван Иванов" }
            };

            var client = clients.FirstOrDefault(c => c.FullName == FullNameTextBoxText);

            if (client != null)
            {
                _car.ClientID = client.ClientID;
            }
            else
            {
                throw new Exception("Клиент не найден");
            }

            _car.LicensePlate = LicensePlateTextBoxText;
            _car.Brand = BrandTextBoxText;
            _car.Model = ModelTextBoxText;

            if (VINTextBoxText.Length == 17)
            {
                _car.VIN = VINTextBoxText;
            }
            else
            {
                throw new Exception("Некорректный VIN");
            }

            if (int.TryParse(YearTextBoxText, out int year) && year > 1950 && year <= DateTime.Now.Year)
            {
                _car.Year = year;
            }
            else
            {
                throw new Exception("Некорректный год выпуска");
            }
        }
    }

    [TestFixture]
    public class AddCarWindowTests
    {
        private ClientCars _car;
        private Clients _customer;

        [SetUp]
        public void Setup()
        {
            _car = new ClientCars();
            _customer = new Clients { ClientID = 1, FullName = "Иван Иванов" };
        }

        [Test]
        public void TestSaveButtonWithValidData()
        {
            var window = new AddCarWindow(_car, _customer)
            {
                FullNameTextBoxText = "Иван Иванов",
                LicensePlateTextBoxText = "123ABC",
                BrandTextBoxText = "Toyota",
                ModelTextBoxText = "Corolla",
                VINTextBoxText = "1HGCM82633A123456",
                YearTextBoxText = "2015"
            };

            window.SaveButton_Click();

            Assert.AreEqual(1, _car.ClientID);
            Assert.AreEqual("123ABC", _car.LicensePlate);
            Assert.AreEqual("Toyota", _car.Brand);
            Assert.AreEqual("Corolla", _car.Model);
            Assert.AreEqual("1HGCM82633A123456", _car.VIN);
            Assert.AreEqual(2015, _car.Year);
        }

        [Test]
        public void TestSaveButtonWithInvalidVIN()
        {
            var window = new AddCarWindow(_car, _customer)
            {
                FullNameTextBoxText = "Иван Иванов",
                LicensePlateTextBoxText = "123ABC",
                BrandTextBoxText = "Toyota",
                ModelTextBoxText = "Corolla",
                VINTextBoxText = "INVALIDVIN",
                YearTextBoxText = "2015"
            };

            Assert.Throws<Exception>(() => window.SaveButton_Click());
        }

        [Test]
        public void TestSaveButtonWithInvalidYear()
        {
            var window = new AddCarWindow(_car, _customer)
            {
                FullNameTextBoxText = "Иван Иванов",
                LicensePlateTextBoxText = "123ABC",
                BrandTextBoxText = "Toyota",
                ModelTextBoxText = "Corolla",
                VINTextBoxText = "1HGCM82633A123456",
                YearTextBoxText = "1900"
            };

            Assert.Throws<Exception>(() => window.SaveButton_Click());
        }

        [Test]
        public void TestSaveButtonWithNonExistingClient()
        {
            var window = new AddCarWindow(_car, _customer)
            {
                FullNameTextBoxText = "Неизвестный Клиент",
                LicensePlateTextBoxText = "123ABC",
                BrandTextBoxText = "Toyota",
                ModelTextBoxText = "Corolla",
                VINTextBoxText = "1HGCM82633A123456",
                YearTextBoxText = "2015"
            };

            Assert.Throws<Exception>(() => window.SaveButton_Click());
        }
    }
}
