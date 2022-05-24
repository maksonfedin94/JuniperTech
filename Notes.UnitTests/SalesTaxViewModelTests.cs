using Moq;
using Notes.Models;
using Notes.Services;
using Notes.ViewModels;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Notes.UnitTests
{
    [TestFixture]
    public class SalesTaxViewModelTests
    {
        [SetUp]
        public void SetUp()
        {
            var platformServicesFake = new Mock<IPlatformServices>();
            Device.PlatformServices = platformServicesFake.Object;
        }

        [TestCase("32835")]
        public void GetTaxForLocationSuccessTest(string zipCode)
        {
            var mockTaxService = new Mock<ITaxService>();
            mockTaxService.Setup(x => x.GetTaxForLocation(zipCode, "US")).ReturnsAsync(new TaxRate
            {
                Rate = new Rate
                {
                    StateRate = "0.60",
                    CityRate = "0.05",
                    CombinedRate = "0.65"
                }
            });

            DependencyService.Register<ITaxService>();

            var mockDialogService = new Mock<IDialogService>();
            mockDialogService.Setup(x => x.ShowBaseDialog(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var viewModel = new SalesTaxViewModel(mockTaxService.Object, mockDialogService.Object);
            Assert.IsNull(viewModel.ZipCode);
            viewModel.ZipCode = zipCode;
            Assert.IsNotNull(viewModel.ZipCode);
            Assert.AreEqual(viewModel.ZipCode, zipCode);
            Assert.IsNotNull(viewModel.TaxRateCommand);
            Assert.IsTrue(viewModel.TaxRateCommand.CanExecute(null));
            viewModel.TaxRateCommand.Execute(null);
            mockDialogService.Verify(x => x.ShowBaseDialog(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
