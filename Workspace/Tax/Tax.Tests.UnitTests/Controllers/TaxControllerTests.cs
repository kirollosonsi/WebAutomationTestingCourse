using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tax.Core;
using Tax.Data;
using Tax.Repository;
using Tax.Web.Controllers;
using Tax.Web.Models;
using Xunit;

namespace Tax.Tests.UnitTests.Controllers
{
    public class TaxControllerTests
    {
        [Fact]
        public async Task AddTax_InvalidInput_ReturnsToSameActionWithValidation()
        {
            // why all these nulls !!
            var userManagerStub = Substitute.For<UserManager<ApplicationUser>>(Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var logger = Substitute.For<ILogger<TaxController>>();
            var userTaxRepo = Substitute.For<IUserTaxRepository>();
            var taxService = Substitute.For<ITaxService>();

            var taxController = new TaxController(userManagerStub, logger, userTaxRepo, taxService);

            taxController.ModelState.AddModelError("error", "error");

            var result = await taxController.AddTax(new TaxViewModel());

            Assert.True(false);
            Assert.True(taxController.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
            Assert.IsNotType<LocalRedirectResult>(result);
        }

        [Fact]
        public async Task GetTax_ValidData_ReturnsViewResults()
        {
           int year = 2000;
            string userId = "userid";

            var userManagerStub = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            var logger = new Mock<ILogger<TaxController>>();
            var userTaxRepo = new Mock<IUserTaxRepository>();
            var taxService = new Mock<ITaxService>();

            userManagerStub.Setup(x => x.GetUserAsync(null)).ReturnsAsync(new ApplicationUser { Id = userId });

            userTaxRepo.Setup(x => x.GetUserTax(userId, year)).Returns(Task.FromResult(new UserTax { UserId = userId, Year = year, NumberOfChildren = 1, TaxDueAmount = 10, TotalIncome = 6000, CharityPaidAmount = 10 }));

            TaxController taxController = new TaxController(userManagerStub.Object, logger.Object, userTaxRepo.Object, taxService.Object);

            var result = await taxController.GetTax(year);
            
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
