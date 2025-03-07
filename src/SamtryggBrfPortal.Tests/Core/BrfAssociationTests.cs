using System;
using Xunit;
using SamtryggBrfPortal.Core.Entities;

namespace SamtryggBrfPortal.Tests.Core
{
    public class BrfAssociationTests
    {
        [Fact]
        public void BrfAssociation_WithValidData_ShouldCreateInstance()
        {
            // Arrange
            var name = "Test BRF";
            var organizationNumber = "123456-7890";
            var address = "Test Street 123";
            var postalCode = "12345";
            var city = "Test City";
            var contactEmail = "test@brf.com";
            var contactPhone = "0701234567";

            // Act
            var brfAssociation = new BrfAssociation
            {
                Name = name,
                OrganizationNumber = organizationNumber,
                Address = address,
                PostalCode = postalCode,
                City = city,
                ContactEmail = contactEmail,
                ContactPhone = contactPhone,
                CreatedAt = DateTime.Now
            };

            // Assert
            Assert.Equal(name, brfAssociation.Name);
            Assert.Equal(organizationNumber, brfAssociation.OrganizationNumber);
            Assert.Equal(address, brfAssociation.Address);
            Assert.Equal(postalCode, brfAssociation.PostalCode);
            Assert.Equal(city, brfAssociation.City);
            Assert.Equal(contactEmail, brfAssociation.ContactEmail);
            Assert.Equal(contactPhone, brfAssociation.ContactPhone);
            Assert.NotEqual(default, brfAssociation.CreatedAt);
        }

        [Fact]
        public void BrfAssociation_WithProperties_ShouldHaveCorrectRelationship()
        {
            // Arrange
            var brfAssociation = new BrfAssociation
            {
                Name = "Test BRF",
                OrganizationNumber = "123456-7890"
            };

            var property1 = new Property
            {
                Address = "Property 1",
                BrfAssociation = brfAssociation
            };

            var property2 = new Property
            {
                Address = "Property 2",
                BrfAssociation = brfAssociation
            };

            // Act
            brfAssociation.Properties = new List<Property> { property1, property2 };

            // Assert
            Assert.Equal(2, brfAssociation.Properties.Count);
            Assert.Contains(property1, brfAssociation.Properties);
            Assert.Contains(property2, brfAssociation.Properties);
            Assert.Same(brfAssociation, property1.BrfAssociation);
            Assert.Same(brfAssociation, property2.BrfAssociation);
        }

        [Fact]
        public void BrfAssociation_WithBoardMembers_ShouldHaveCorrectRelationship()
        {
            // Arrange
            var brfAssociation = new BrfAssociation
            {
                Name = "Test BRF",
                OrganizationNumber = "123456-7890"
            };

            var boardMember1 = new BrfBoardMember
            {
                FirstName = "John",
                LastName = "Doe",
                Role = "Chairman",
                BrfAssociation = brfAssociation
            };

            var boardMember2 = new BrfBoardMember
            {
                FirstName = "Jane",
                LastName = "Smith",
                Role = "Treasurer",
                BrfAssociation = brfAssociation
            };

            // Act
            brfAssociation.BoardMembers = new List<BrfBoardMember> { boardMember1, boardMember2 };

            // Assert
            Assert.Equal(2, brfAssociation.BoardMembers.Count);
            Assert.Contains(boardMember1, brfAssociation.BoardMembers);
            Assert.Contains(boardMember2, brfAssociation.BoardMembers);
            Assert.Same(brfAssociation, boardMember1.BrfAssociation);
            Assert.Same(brfAssociation, boardMember2.BrfAssociation);
        }
    }
}
