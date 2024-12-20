using NUnit.Framework;

namespace TestProject2
{
    public class ClientCars
    {
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public int Year { get; set; }
        public int ClientID { get; set; }
    }

    [TestFixture]
    public class EditCarWindowTests
    {
        private ClientCars _car;

        [SetUp]
        public void Setup()
        {
            _car = new ClientCars
            {
                LicensePlate = "123ABC",
                Brand = "Toyota",
                Model = "Corolla",
                VIN = "1HGCM82633A123456",
                Year = 2010
            };
        }

        [Test]
        public void TestInitialization()
        {
            Assert.AreEqual("123ABC", _car.LicensePlate);
            Assert.AreEqual("Toyota", _car.Brand);
            Assert.AreEqual("Corolla", _car.Model);
            Assert.AreEqual("1HGCM82633A123456", _car.VIN);
            Assert.AreEqual(2010, _car.Year);
        }

        [Test]
        public void TestUpdateCarFields()
        {
            _car.LicensePlate = "456DEF";
            _car.Brand = "Honda";
            _car.Model = "Civic";
            _car.VIN = "2T1BURHE5JC123456";
            _car.Year = 2020;

            Assert.AreEqual("456DEF", _car.LicensePlate);
            Assert.AreEqual("Honda", _car.Brand);
            Assert.AreEqual("Civic", _car.Model);
            Assert.AreEqual("2T1BURHE5JC123456", _car.VIN);
            Assert.AreEqual(2020, _car.Year);
        }

        [Test]
        public void TestValidYear()
        {
            _car.Year = 2022;
            Assert.AreEqual(2022, _car.Year);
        }

        [Test]
        public void TestInvalidYearThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _car.Year = 1900; // Некорректный год
            });
        }

        [Test]
        public void TestEmptyFields()
        {
            _car.LicensePlate = string.Empty;
            _car.Brand = string.Empty;
            _car.Model = string.Empty;
            _car.VIN = string.Empty;

            Assert.IsEmpty(_car.LicensePlate);
            Assert.IsEmpty(_car.Brand);
            Assert.IsEmpty(_car.Model);
            Assert.IsEmpty(_car.VIN);
        }

        [Test]
        public void TestNullFields()
        {
            _car.LicensePlate = null;
            _car.Brand = null;
            _car.Model = null;
            _car.VIN = null;

            Assert.IsNull(_car.LicensePlate);
            Assert.IsNull(_car.Brand);
            Assert.IsNull(_car.Model);
            Assert.IsNull(_car.VIN);
        }

        [Test]
        public void TestBoundaryYear()
        {
            _car.Year = DateTime.Now.Year; // Текущий год
            Assert.AreEqual(DateTime.Now.Year, _car.Year);

            _car.Year = 1951; // Минимально допустимый год
            Assert.AreEqual(1951, _car.Year);
        }

        [Test]
        public void TestVINValidation()
        {
            string validVIN = "1HGCM82633A123456";
            string invalidVIN = "INVALID";

            _car.VIN = validVIN;
            Assert.AreEqual(validVIN, _car.VIN);

            _car.VIN = invalidVIN;
            Assert.AreEqual(invalidVIN, _car.VIN);
        }
    }
}
