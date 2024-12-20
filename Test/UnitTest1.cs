using NUnit.Framework;

namespace Test
{
    public class EditCarWindowTests
    {
        [Test]
        public void FieldsTest()
        {
            
            var car = new ClientCars
            {
                LicensePlate = "A123BC",
                Brand = "Toyota",
                Model = "Corolla",
                VIN = "1234567890",
                Year = 2020
            };

            var window = new EditCarWindow(car);

            
            Assert.AreEqual("A123BC", window.LicensePlateTextBox.Text);
            Assert.AreEqual("Toyota", window.BrandTextBox.Text);
            Assert.AreEqual("Corolla", window.ModelTextBox.Text);
            Assert.AreEqual("1234567890", window.VINTextBox.Text);
            Assert.AreEqual("2020", window.YearTextBox.Text);
        }
    }
}
