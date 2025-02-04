using KursovoyProject;
using Xunit;

namespace TestProject1
{
    public class EditCarWindowTests
    {
        [Fact]
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

            Assert.Equal("A123BC", window.LicensePlateTextBox.Text);
            Assert.Equal("Toyota", window.BrandTextBox.Text);
            Assert.Equal("Corolla", window.ModelTextBox.Text);
            Assert.Equal("1234567890", window.VINTextBox.Text);
            Assert.Equal("2020", window.YearTextBox.Text);
        }
    }
}
