using System;
using System.Linq;
using Xunit;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Tests.Infrastructure
{
    public class ApplicationDbContextTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public ApplicationDbContextTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ApplicationDbContext_CanAddAndRetrieveBrfAssociation()
        {
            // Arrange
            var brfAssociation = new BrfAssociation
            {
                Name = "Test BRF Association",
                OrganizationNumber = "123456-7890",
                Address = "Test Street 123",
                PostalCode = "12345",
                City = "Test City",
                ContactEmail = "test@brf.com",
                ContactPhone = "0701234567",
                CreatedAt = DateTime.Now
            };

            // Act
            _fixture.DbContext.BrfAssociations.Add(brfAssociation);
            _fixture.DbContext.SaveChanges();

            // Assert
            var retrievedBrf = _fixture.DbContext.BrfAssociations
                .FirstOrDefault(b => b.Name == "Test BRF Association");
            
            Assert.NotNull(retrievedBrf);
            Assert.Equal("Test BRF Association", retrievedBrf.Name);
            Assert.Equal("123456-7890", retrievedBrf.OrganizationNumber);
        }

        [Fact]
        public void ApplicationDbContext_CanAddAndRetrievePropertyWithBrfAssociation()
        {
            // Arrange
            var brfAssociation = new BrfAssociation
            {
                Name = "Property Test BRF",
                OrganizationNumber = "987654-3210"
            };

            var property = new Property
            {
                Address = "Property Test Street 123",
                PostalCode = "54321",
                City = "Property Test City",
                Size = 75.5m,
                NumberOfRooms = 3,
                Floor = 2,
                HasBalcony = true,
                HasElevator = true,
                Description = "A nice test property",
                BrfAssociation = brfAssociation,
                CreatedAt = DateTime.Now
            };

            // Act
            _fixture.DbContext.Properties.Add(property);
            _fixture.DbContext.SaveChanges();

            // Assert
            var retrievedProperty = _fixture.DbContext.Properties
                .Include(p => p.BrfAssociation)
                .FirstOrDefault(p => p.Address == "Property Test Street 123");
            
            Assert.NotNull(retrievedProperty);
            Assert.Equal("Property Test Street 123", retrievedProperty.Address);
            Assert.Equal(75.5m, retrievedProperty.Size);
            Assert.Equal(3, retrievedProperty.NumberOfRooms);
            Assert.True(retrievedProperty.HasBalcony);
            Assert.NotNull(retrievedProperty.BrfAssociation);
            Assert.Equal("Property Test BRF", retrievedProperty.BrfAssociation.Name);
        }

        [Fact]
        public void ApplicationDbContext_CanAddAndRetrieveRentalApplication()
        {
            // Arrange
            var brfAssociation = new BrfAssociation
            {
                Name = "Rental Test BRF",
                OrganizationNumber = "555555-5555"
            };

            var property = new Property
            {
                Address = "Rental Test Street 123",
                PostalCode = "55555",
                City = "Rental Test City",
                BrfAssociation = brfAssociation
            };

            var rentalApplication = new RentalApplication
            {
                Property = property,
                TenantFirstName = "John",
                TenantLastName = "Doe",
                TenantEmail = "john.doe@example.com",
                TenantPhone = "0701234567",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(365),
                MonthlyRent = 8000,
                Status = RentalStatus.Pending,
                CreatedAt = DateTime.Now
            };

            // Act
            _fixture.DbContext.RentalApplications.Add(rentalApplication);
            _fixture.DbContext.SaveChanges();

            // Assert
            var retrievedApplication = _fixture.DbContext.RentalApplications
                .Include(r => r.Property)
                .ThenInclude(p => p.BrfAssociation)
                .FirstOrDefault(r => r.TenantEmail == "john.doe@example.com");
            
            Assert.NotNull(retrievedApplication);
            Assert.Equal("John", retrievedApplication.TenantFirstName);
            Assert.Equal("Doe", retrievedApplication.TenantLastName);
            Assert.Equal(8000, retrievedApplication.MonthlyRent);
            Assert.Equal(RentalStatus.Pending, retrievedApplication.Status);
            Assert.NotNull(retrievedApplication.Property);
            Assert.Equal("Rental Test Street 123", retrievedApplication.Property.Address);
            Assert.NotNull(retrievedApplication.Property.BrfAssociation);
            Assert.Equal("Rental Test BRF", retrievedApplication.Property.BrfAssociation.Name);
        }

        [Fact]
        public void ApplicationDbContext_AutomaticallySetsDates()
        {
            // Arrange
            var brfAssociation = new BrfAssociation
            {
                Name = "Date Test BRF",
                OrganizationNumber = "111111-1111"
            };

            // Act
            _fixture.DbContext.BrfAssociations.Add(brfAssociation);
            _fixture.DbContext.SaveChanges();

            // Modify the entity
            brfAssociation.ContactEmail = "updated@brf.com";
            _fixture.DbContext.SaveChanges();

            // Assert
            var retrievedBrf = _fixture.DbContext.BrfAssociations
                .FirstOrDefault(b => b.Name == "Date Test BRF");
            
            Assert.NotNull(retrievedBrf);
            Assert.NotEqual(default, retrievedBrf.CreatedAt);
            Assert.NotNull(retrievedBrf.ModifiedAt);
        }
    }
}
