using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Core.Enums;
using SamtryggBrfPortal.Infrastructure.Identity;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;
using SamtryggBrfPortal.Infrastructure.Services;
using SamtryggBrfPortal.Infrastructure.ViewModels;
using Xunit;

namespace SamtryggBrfPortal.Tests.Infrastructure
{
    public class ResidentServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<ResidentService>> _mockLogger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ResidentService _residentService;

        public ResidentServiceTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<ResidentService>>();
            _userManager = fixture.UserManager;

            _residentService = new ResidentService(
                _mockUnitOfWork.Object,
                _userManager,
                _mockLogger.Object);
        }

        [Fact]
        public async Task GetUserProfileAsync_UserExists_ReturnsUserProfile()
        {
            // Arrange
            var testUser = await CreateTestUser();
            
            // Act
            var result = await _residentService.GetUserProfileAsync(testUser.Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testUser.Id, result.Id);
            Assert.Equal(testUser.FirstName, result.FirstName);
            Assert.Equal(testUser.LastName, result.LastName);
            Assert.Equal(testUser.Email, result.Email);
        }

        [Fact]
        public async Task GetUserProfileAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var nonExistentUserId = "non-existent-user-id";
            
            // Act
            var result = await _residentService.GetUserProfileAsync(nonExistentUserId);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var testUser = await CreateTestUser();
            var updatedProfile = new UserProfileViewModel
            {
                Id = testUser.Id,
                FirstName = "Updated",
                LastName = "User",
                Email = testUser.Email,
                PhoneNumber = "0709876543",
                PersonalIdentityNumber = "198001010000"
            };
            
            // Act
            var result = await _residentService.UpdateUserProfileAsync(testUser.Id, updatedProfile);
            
            // Assert
            Assert.True(result);
            
            // Verify user was updated
            var updatedUser = await _userManager.FindByIdAsync(testUser.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("Updated", updatedUser.FirstName);
            Assert.Equal("User", updatedUser.LastName);
            Assert.Equal("0709876543", updatedUser.PhoneNumber);
        }

        [Fact]
        public async Task UploadDocumentAsync_EmptyUserId_ThrowsArgumentNullException()
        {
            // Arrange
            string userId = string.Empty; // Using empty string instead of null
            var model = new DocumentUploadViewModel
            {
                Name = "Test Document",
                Description = "Test Description",
                RelatedEntityType = "RentalApplication",
                RelatedEntityId = Guid.NewGuid(),
                Type = DocumentType.RentalContract
            };
            var fileContent = new byte[] { 1, 2, 3 };
            var fileName = "test.pdf";
            var contentType = "application/pdf";
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _residentService.UploadDocumentAsync(userId, model, fileContent, fileName, contentType));
        }

        [Fact]
        public async Task UploadDocumentAsync_NullModel_ThrowsArgumentNullException()
        {
            // Arrange
            var userId = "test-user-id";
            DocumentUploadViewModel? model = null; // Using nullable reference type
            var fileContent = new byte[] { 1, 2, 3 };
            var fileName = "test.pdf";
            var contentType = "application/pdf";
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _residentService.UploadDocumentAsync(userId, null!, fileContent, fileName, contentType));
        }

        [Fact]
        public async Task UploadDocumentAsync_EmptyFileContent_ThrowsArgumentException()
        {
            // Arrange
            var userId = "test-user-id";
            var model = new DocumentUploadViewModel
            {
                Name = "Test Document",
                Description = "Test Description",
                RelatedEntityType = "RentalApplication",
                RelatedEntityId = Guid.NewGuid(),
                Type = DocumentType.RentalContract
            };
            var fileContent = new byte[0];
            var fileName = "test.pdf";
            var contentType = "application/pdf";
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _residentService.UploadDocumentAsync(userId, model, fileContent, fileName, contentType));
        }

        [Fact]
        public async Task UploadDocumentAsync_FileTooLarge_ThrowsArgumentException()
        {
            // Arrange
            var userId = "test-user-id";
            var model = new DocumentUploadViewModel
            {
                Name = "Large Test Document",
                Description = "Test Description for Large File",
                RelatedEntityId = Guid.NewGuid(),
                RelatedEntityType = "RentalApplication",
                Type = DocumentType.RentalContract
            };
            
            // Create a file larger than 10MB (10 * 1024 * 1024 bytes)
            var fileContent = new byte[11 * 1024 * 1024];
            var fileName = "large_file.pdf";
            var contentType = "application/pdf";
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _residentService.UploadDocumentAsync(userId, model, fileContent, fileName, contentType));
            
            Assert.Contains("exceeds the maximum allowed size", exception.Message);
        }

        [Fact]
        public async Task UploadDocumentAsync_DisallowedFileType_ThrowsArgumentException()
        {
            // Arrange
            var userId = "test-user-id";
            var model = new DocumentUploadViewModel
            {
                Name = "Executable Test Document",
                Description = "Test Description for Executable File",
                RelatedEntityId = Guid.NewGuid(),
                RelatedEntityType = "RentalApplication",
                Type = DocumentType.RentalContract
            };
            var fileContent = new byte[] { 1, 2, 3 };
            var fileName = "test.exe";
            var contentType = "application/x-msdownload";
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _residentService.UploadDocumentAsync(userId, model, fileContent, fileName, contentType));
            
            Assert.Contains("not allowed", exception.Message);
        }

        [Fact]
        public async Task SendMessageAsync_EmptyUserId_ThrowsArgumentNullException()
        {
            // Arrange
            string userId = string.Empty; // Using empty string instead of null
            var recipientUserId = "recipient-id";
            var content = "Test message";
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _residentService.SendMessageAsync(userId, recipientUserId, content));
        }

        [Fact]
        public async Task SendMessageAsync_EmptyRecipientId_ThrowsArgumentNullException()
        {
            // Arrange
            var userId = "user-id";
            string recipientUserId = string.Empty; // Using empty string instead of null
            var content = "Test message";
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _residentService.SendMessageAsync(userId, recipientUserId, content));
        }

        [Fact]
        public async Task SendMessageAsync_EmptyContent_ThrowsArgumentException()
        {
            // Arrange
            var userId = "user-id";
            var recipientUserId = "recipient-id";
            var content = "";
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _residentService.SendMessageAsync(userId, recipientUserId, content));
        }

        [Fact]
        public async Task SendMessageAsync_ContentTooLong_ThrowsArgumentException()
        {
            // Arrange
            var userId = "user-id";
            var recipientUserId = "recipient-id";
            var content = new string('a', 5001); // Create a string longer than 5000 characters
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _residentService.SendMessageAsync(userId, recipientUserId, content));
            
            Assert.Contains("exceeds maximum length", exception.Message);
        }

        private async Task<ApplicationUser> CreateTestUser()
        {
            var testUser = new ApplicationUser
            {
                UserName = $"test-{Guid.NewGuid()}@example.com",
                Email = $"test-{Guid.NewGuid()}@example.com",
                EmailConfirmed = true,
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "0701234567",
                PhoneNumberConfirmed = true,
                CreatedAt = DateTime.Now,
                IsActive = true,
                HasCompletedOnboarding = true
            };

            var result = await _userManager.CreateAsync(testUser, "Password123!");
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return testUser;
        }
    }
}
