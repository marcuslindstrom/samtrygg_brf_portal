using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Core.Enums;
using SamtryggBrfPortal.Infrastructure.Identity;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;
using SamtryggBrfPortal.Infrastructure.Services.Interfaces;
using SamtryggBrfPortal.Infrastructure.ViewModels;

namespace SamtryggBrfPortal.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for resident operations
    /// </summary>
    public class ResidentService : IResidentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ResidentService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="userManager">User manager</param>
        /// <param name="logger">Logger</param>
        public ResidentService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILogger<ResidentService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<ResidentDashboardViewModel?> GetDashboardDataAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    return null;
                }

                // Get the user's property
                var properties = await _unitOfWork.Properties.GetByOwnerIdAsync(userId);
                var property = properties.FirstOrDefault();

                // Get the user's rental applications
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                var pendingApplications = applications.Where(a => a.Status == RentalStatus.Pending).ToList();

                // Create the dashboard view model
                var dashboard = new ResidentDashboardViewModel
                {
                    Property = property != null ? new PropertySummaryViewModel
                    {
                        Id = property.Id,
                        Address = property.Address,
                        Size = property.Size, // Using Size which is a string
                        NumberOfRooms = property.NumberOfRooms,
                        Floor = property.Floor, // Using Floor which is a string
                        MonthlyRent = property.MonthlyRent,
                        IsAvailableForRent = property.IsAvailableForRent,
                        LastUpdated = property.ModifiedAt ?? property.CreatedAt,
                        PrimaryImageUrl = property.PrimaryImageUrl ?? string.Empty
                    } : null,
                    RentalStatus = new RentalStatusViewModel
                    {
                        HasActiveRental = applications.Any(a => a.Status == RentalStatus.Approved),
                        PreviousRentalsCount = applications.Count(a => a.Status == RentalStatus.Completed),
                        CurrentRentalApplicationId = pendingApplications.FirstOrDefault()?.Id,
                        StatusText = pendingApplications.Any() ? "Ansökan pågår" : "Ingen aktiv",
                        StatusBadgeClass = pendingApplications.Any() ? "bg-warning" : "bg-secondary"
                    },
                    PendingApplicationsCount = pendingApplications.Count,
                    DocumentsCount = applications.Sum(a => a.Documents?.Count ?? 0),
                    UnreadMessagesCount = applications.Sum(a => a.Messages?.Count(m => !m.IsRead && m.RecipientUserId == userId) ?? 0)
                };

                // Add recent activities
                var activities = new List<ActivityViewModel>();

                // Add application activities
                foreach (var app in applications.OrderByDescending(a => a.CreatedAt).Take(3))
                {
                    activities.Add(new ActivityViewModel
                    {
                        Title = "Ansökan skapad",
                        Description = $"Du skapade en ny ansökan om andrahandsuthyrning",
                        Date = app.CreatedAt,
                        Type = "Application",
                        RelatedEntityId = app.Id
                    });
                }

                dashboard.RecentActivities = activities.OrderByDescending(a => a.Date).Take(5).ToList();

                // Add messages
                var messages = new List<MessageViewModel>();
                foreach (var app in applications)
                {
                    if (app.Messages != null)
                    {
                        foreach (var msg in app.Messages.Where(m => m.RecipientUserId == userId))
                        {
                            messages.Add(new MessageViewModel
                            {
                                Id = msg.Id,
                                SenderName = "BRF Styrelsen", // Default sender name
                                Content = msg.Content ?? string.Empty,
                                Date = msg.CreatedAt,
                                IsRead = msg.IsRead,
                                RelatedEntityId = app.Id,
                                RelatedEntityType = "RentalApplication"
                            });
                        }
                    }
                }
                dashboard.Messages = messages.OrderByDescending(m => m.Date).Take(5).ToList();

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<RentalApplicationViewModel>> GetApplicationsAsync(string userId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                
                return applications.Select(a => new RentalApplicationViewModel
                {
                    Id = a.Id,
                    PropertyId = a.PropertyId,
                    PropertyAddress = a.Property?.Address ?? string.Empty,
                    TenantFirstName = a.TenantFirstName,
                    TenantLastName = a.TenantLastName,
                    TenantEmail = a.TenantEmail,
                    TenantPhone = a.TenantPhone,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    MonthlyRent = a.MonthlyRent,
                    Status = a.Status,
                    SubmittedAt = a.CreatedAt,
                    LastUpdated = a.ModifiedAt ?? a.CreatedAt,
                    HasBackgroundCheck = a.BackgroundCheck != null,
                    BackgroundCheckStatus = a.BackgroundCheck?.Status ?? default(BackgroundCheckStatus),
                    UnreadMessagesCount = a.Messages?.Count(m => !m.IsRead && m.RecipientUserId == userId) ?? 0,
                    Documents = a.Documents?.Select(d => new DocumentViewModel
                    {
                        Id = d.Id,
                        Name = d.FileName,
                        Type = d.DocumentType,
                        FilePath = d.FileUrl,
                        FileSize = d.FileSize,
                        MimeType = d.ContentType,
                        UploadDate = d.CreatedAt,
                        UploadedByUserId = d.UploadedBy,
                        Description = string.Empty,
                        RelatedEntityType = "RentalApplication"
                    }).ToList() ?? new List<DocumentViewModel>(),
                    Messages = a.Messages?.Where(m => m.RecipientUserId == userId).Select(m => new MessageViewModel
                    {
                        Id = m.Id,
                        SenderName = "BRF Styrelsen", // Default sender name
                        Content = m.Content ?? string.Empty,
                        Date = m.CreatedAt,
                        IsRead = m.IsRead,
                        RelatedEntityId = a.Id,
                        RelatedEntityType = "RentalApplication"
                    }).ToList() ?? new List<MessageViewModel>()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting applications for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<RentalApplicationViewModel?> GetApplicationByIdAsync(Guid applicationId, string userId)
        {
            try
            {
                var application = await _unitOfWork.RentalApplications.GetCompleteAsync(applicationId);
                if (application == null || application.ApplicantUserId != userId)
                {
                    return null;
                }

                return new RentalApplicationViewModel
                {
                    Id = application.Id,
                    PropertyId = application.PropertyId,
                    PropertyAddress = application.Property?.Address ?? string.Empty,
                    TenantFirstName = application.TenantFirstName,
                    TenantLastName = application.TenantLastName,
                    TenantEmail = application.TenantEmail,
                    TenantPhone = application.TenantPhone,
                    StartDate = application.StartDate,
                    EndDate = application.EndDate,
                    MonthlyRent = application.MonthlyRent,
                    Status = application.Status,
                    SubmittedAt = application.CreatedAt,
                    LastUpdated = application.ModifiedAt ?? application.CreatedAt,
                    HasBackgroundCheck = application.BackgroundCheck != null,
                    BackgroundCheckStatus = application.BackgroundCheck?.Status ?? default(BackgroundCheckStatus),
                    UnreadMessagesCount = application.Messages?.Count(m => !m.IsRead && m.RecipientUserId == userId) ?? 0,
                    Documents = application.Documents?.Select(d => new DocumentViewModel
                    {
                        Id = d.Id,
                        Name = d.FileName,
                        Type = d.DocumentType,
                        FilePath = d.FileUrl,
                        FileSize = d.FileSize,
                        MimeType = d.ContentType,
                        UploadDate = d.CreatedAt,
                        UploadedByUserId = d.UploadedBy,
                        Description = string.Empty,
                        RelatedEntityType = "RentalApplication"
                    }).ToList() ?? new List<DocumentViewModel>(),
                    Messages = application.Messages?.Where(m => m.RecipientUserId == userId).Select(m => new MessageViewModel
                    {
                        Id = m.Id,
                        SenderName = "BRF Styrelsen", // Default sender name
                        Content = m.Content ?? string.Empty,
                        Date = m.CreatedAt,
                        IsRead = m.IsRead,
                        RelatedEntityId = application.Id,
                        RelatedEntityType = "RentalApplication"
                    }).ToList() ?? new List<MessageViewModel>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application with ID {ApplicationId} for user with ID {UserId}", applicationId, userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateApplicationAsync(string userId, RentalApplicationCreateViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"User with ID {userId} not found");
                }

                var property = await _unitOfWork.Properties.GetByIdAsync(model.PropertyId);
                if (property == null)
                {
                    throw new ArgumentException($"Property with ID {model.PropertyId} not found");
                }

                await _unitOfWork.BeginTransactionAsync();

                var application = new RentalApplication
                {
                    PropertyId = model.PropertyId,
                    OwnerId = property.OwnerId,
                    ApplicantUserId = userId,
                    ApplicantFirstName = user.FirstName,
                    ApplicantLastName = user.LastName,
                    ApplicantEmail = user.Email,
                    TenantFirstName = model.TenantFirstName,
                    TenantLastName = model.TenantLastName,
                    TenantEmail = model.TenantEmail,
                    TenantPhone = model.TenantPhone,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    MonthlyRent = model.MonthlyRent,
                    Status = RentalStatus.Pending,
                    RentalReason = model.AdditionalInformation,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                var createdApplication = await _unitOfWork.RentalApplications.AddAsync(application);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();

                return createdApplication.Id;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating application for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> CancelApplicationAsync(Guid applicationId, string userId)
        {
            try
            {
                var application = await _unitOfWork.RentalApplications.GetByIdAsync(applicationId);
                if (application == null || application.ApplicantUserId != userId)
                {
                    return false;
                }

                if (application.Status != RentalStatus.Pending)
                {
                    return false;
                }

                await _unitOfWork.BeginTransactionAsync();

                application.Status = RentalStatus.Cancelled;
                application.ModifiedAt = DateTime.UtcNow;
                application.ModifiedBy = userId;

                await _unitOfWork.RentalApplications.UpdateAsync(application);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error cancelling application with ID {ApplicationId} for user with ID {UserId}", applicationId, userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<DocumentViewModel>> GetDocumentsAsync(string userId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                var documents = new List<DocumentViewModel>();

                foreach (var app in applications)
                {
                    if (app.Documents != null)
                    {
                        documents.AddRange(app.Documents.Select(d => new DocumentViewModel
                        {
                            Id = d.Id,
                            Name = d.FileName,
                            Type = d.DocumentType,
                            FilePath = d.FileUrl,
                            FileSize = d.FileSize,
                            MimeType = d.ContentType,
                            UploadDate = d.CreatedAt,
                            UploadedByUserId = d.UploadedBy,
                            Description = string.Empty,
                            RelatedEntityId = app.Id,
                            RelatedEntityType = "RentalApplication"
                        }));
                    }
                }

                return documents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting documents for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DocumentViewModel?> GetDocumentByIdAsync(Guid documentId, string userId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                
                foreach (var app in applications)
                {
                    if (app.Documents != null)
                    {
                        var document = app.Documents.FirstOrDefault(d => d.Id == documentId);
                        if (document != null)
                        {
                            return new DocumentViewModel
                            {
                                Id = document.Id,
                                Name = document.FileName,
                                Type = document.DocumentType,
                                FilePath = document.FileUrl,
                                FileSize = document.FileSize,
                                MimeType = document.ContentType,
                                UploadDate = document.CreatedAt,
                                UploadedByUserId = document.UploadedBy,
                                Description = string.Empty,
                                RelatedEntityId = app.Id,
                                RelatedEntityType = "RentalApplication"
                            };
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting document with ID {DocumentId} for user with ID {UserId}", documentId, userId);
                throw;
            }
        }

        /// <summary>
        /// Uploads a document for a rental application
        /// </summary>
        /// <param name="userId">ID of the user uploading the document</param>
        /// <param name="model">Document upload view model containing metadata</param>
        /// <param name="fileContent">Binary content of the file</param>
        /// <param name="fileName">Original file name</param>
        /// <param name="contentType">MIME type of the file</param>
        /// <returns>ID of the created document</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null</exception>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid or entity not found</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when user doesn't have permission</exception>
        /// <exception cref="InvalidOperationException">Thrown when operation cannot be completed</exception>
        public async Task<Guid> UploadDocumentAsync(string userId, DocumentUploadViewModel model, byte[] fileContent, string fileName, string contentType)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty");
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Document upload model cannot be null");
            }

            if (fileContent == null || fileContent.Length == 0)
            {
                throw new ArgumentException("File content cannot be null or empty", nameof(fileContent));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("Content type cannot be null or empty", nameof(contentType));
            }

            // Validate file size (max 10MB)
            const int maxFileSizeBytes = 10 * 1024 * 1024;
            if (fileContent.Length > maxFileSizeBytes)
            {
                throw new ArgumentException($"File size exceeds the maximum allowed size of {maxFileSizeBytes / (1024 * 1024)}MB", nameof(fileContent));
            }

            // Validate file type (whitelist approach)
            var allowedContentTypes = new[] { "application/pdf", "image/jpeg", "image/png", "image/gif", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
            if (!allowedContentTypes.Contains(contentType.ToLower()))
            {
                throw new ArgumentException($"File type '{contentType}' is not allowed. Allowed types: PDF, JPEG, PNG, GIF, DOC, DOCX", nameof(contentType));
            }

            try
            {
                if (model.RelatedEntityId == null)
                {
                    throw new ArgumentException("Related entity ID is required", nameof(model.RelatedEntityId));
                }

                if (string.IsNullOrEmpty(model.RelatedEntityType))
                {
                    throw new ArgumentException("Related entity type is required", nameof(model.RelatedEntityType));
                }

                if (model.RelatedEntityType != "RentalApplication")
                {
                    throw new ArgumentException($"Invalid related entity type: {model.RelatedEntityType}", nameof(model.RelatedEntityType));
                }

                var application = await _unitOfWork.RentalApplications.GetByIdAsync(model.RelatedEntityId.Value);
                if (application == null)
                {
                    throw new ArgumentException($"Rental application with ID {model.RelatedEntityId} not found", nameof(model.RelatedEntityId));
                }

                if (application.ApplicantUserId != userId)
                {
                    _logger.LogWarning("Unauthorized access attempt: User {UserId} tried to upload document for application {ApplicationId} owned by {OwnerId}", 
                        userId, application.Id, application.ApplicantUserId);
                    throw new UnauthorizedAccessException($"User does not have permission to upload documents for this application");
                }

                // Sanitize and validate file name
                var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || fileExtension.Length > 10)
                {
                    throw new ArgumentException("Invalid file extension", nameof(fileName));
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fileUrl = $"/uploads/documents/{uniqueFileName}";

                // Ensure document type is provided
                if (!Enum.IsDefined(typeof(DocumentType), model.Type))
                {
                    throw new ArgumentException("Valid document type is required", nameof(model.Type));
                }
                
                var docType = model.Type;
                
                // Create the document entity
                var document = new Document
                {
                    FileName = string.IsNullOrEmpty(model.Name) ? Path.GetFileName(fileName) : model.Name,
                    DocumentType = docType,
                    FileUrl = fileUrl,
                    FileSize = fileContent.Length,
                    ContentType = contentType,
                    UploadedBy = userId,
                    RequiresSignature = false,
                    IsSigned = false,
                    RentalApplicationId = application.Id,
                    RentalApplication = application,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                await _unitOfWork.BeginTransactionAsync();

                // Add the document to the application
                if (application.Documents == null)
                {
                    application.Documents = new List<Document>();
                }
                application.Documents.Add(document);
                application.ModifiedAt = DateTime.UtcNow;
                application.ModifiedBy = userId;

                await _unitOfWork.RentalApplications.UpdateAsync(application);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();

                return document.Id;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error uploading document for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteDocumentAsync(Guid documentId, string userId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                Document? documentToDelete = null;
                RentalApplication? applicationWithDocument = null;

                foreach (var app in applications)
                {
                    if (app.Documents != null)
                    {
                        var document = app.Documents.FirstOrDefault(d => d.Id == documentId);
                        if (document != null)
                        {
                            documentToDelete = document;
                            applicationWithDocument = app;
                            break;
                        }
                    }
                }

                if (documentToDelete == null || applicationWithDocument == null)
                {
                    return false;
                }

                await _unitOfWork.BeginTransactionAsync();

                // Remove the document from the application
                applicationWithDocument.Documents.Remove(documentToDelete);
                applicationWithDocument.ModifiedAt = DateTime.UtcNow;
                applicationWithDocument.ModifiedBy = userId;

                await _unitOfWork.RentalApplications.UpdateAsync(applicationWithDocument);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting document with ID {DocumentId} for user with ID {UserId}", documentId, userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<UserProfileViewModel?> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                return new UserProfileViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    // These fields are not available in ApplicationUser, so we use empty strings
                    StreetAddress = string.Empty,
                    PostalCode = string.Empty,
                    City = string.Empty,
                    DateOfBirth = null,
                    PersonalIdentityNumber = user.PersonalNumber ?? string.Empty,
                    HasCompletedOnboarding = user.HasCompletedOnboarding,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    ModifiedAt = user.LastLoginAt, // Using LastLoginAt as ModifiedAt
                    IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user),
                    IsPhoneNumberConfirmed = await _userManager.IsPhoneNumberConfirmedAsync(user),
                    IsTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateUserProfileAsync(string userId, UserProfileViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.PersonalNumber = model.PersonalIdentityNumber;
                // Other fields like StreetAddress, PostalCode, City, DateOfBirth are not available in ApplicationUser

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<MessageViewModel>> GetMessagesAsync(string userId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                var messages = new List<MessageViewModel>();

                foreach (var app in applications)
                {
                    if (app.Messages != null)
                    {
                        messages.AddRange(app.Messages.Where(m => m.RecipientUserId == userId).Select(m => new MessageViewModel
                        {
                            Id = m.Id,
                            SenderName = "BRF Styrelsen", // Default sender name
                            Content = m.Content ?? string.Empty,
                            Date = m.CreatedAt,
                            IsRead = m.IsRead,
                            RelatedEntityId = app.Id,
                            RelatedEntityType = "RentalApplication"
                        }));
                    }
                }

                return messages.OrderByDescending(m => m.Date).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> MarkMessageAsReadAsync(Guid messageId, string userId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByUserIdAsync(userId);
                Message? messageToUpdate = null;
                RentalApplication? applicationWithMessage = null;

                foreach (var app in applications)
                {
                    if (app.Messages != null)
                    {
                        var message = app.Messages.FirstOrDefault(m => m.Id == messageId && m.RecipientUserId == userId);
                        if (message != null)
                        {
                            messageToUpdate = message;
                            applicationWithMessage = app;
                            break;
                        }
                    }
                }

                if (messageToUpdate == null || applicationWithMessage == null)
                {
                    return false;
                }

                await _unitOfWork.BeginTransactionAsync();

                // Update the message
                messageToUpdate.IsRead = true;
                messageToUpdate.ReadAt = DateTime.UtcNow;

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error marking message with ID {MessageId} as read for user with ID {UserId}", messageId, userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Guid> SendMessageAsync(string userId, string recipientUserId, string content, Guid? relatedEntityId = null, string? relatedEntityType = null)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "Sender user ID cannot be null or empty");
            }

            if (string.IsNullOrEmpty(recipientUserId))
            {
                throw new ArgumentNullException(nameof(recipientUserId), "Recipient user ID cannot be null or empty");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Message content cannot be null or empty", nameof(content));
            }

            // Limit message length to prevent abuse
            const int maxContentLength = 5000;
            if (content.Length > maxContentLength)
            {
                throw new ArgumentException($"Message content exceeds maximum length of {maxContentLength} characters", nameof(content));
            }

            try
            {
                // Verify sender exists
                var sender = await _userManager.FindByIdAsync(userId);
                if (sender == null)
                {
                    _logger.LogWarning("Attempted to send message from non-existent user with ID {UserId}", userId);
                    throw new ArgumentException($"User with ID {userId} not found", nameof(userId));
                }

                // Verify recipient exists
                var recipient = await _userManager.FindByIdAsync(recipientUserId);
                if (recipient == null)
                {
                    _logger.LogWarning("Attempted to send message to non-existent user with ID {RecipientUserId}", recipientUserId);
                    throw new ArgumentException($"User with ID {recipientUserId} not found", nameof(recipientUserId));
                }

                // If related entity is specified, verify it exists
                RentalApplication? relatedApplication = null;
                if (relatedEntityId.HasValue && !string.IsNullOrEmpty(relatedEntityType))
                {
                    if (relatedEntityType != "RentalApplication")
                    {
                        throw new ArgumentException($"Invalid related entity type: {relatedEntityType}", nameof(relatedEntityType));
                    }

                    relatedApplication = await _unitOfWork.RentalApplications.GetByIdAsync(relatedEntityId.Value);
                    if (relatedApplication == null)
                    {
                        throw new ArgumentException($"Rental application with ID {relatedEntityId} not found", nameof(relatedEntityId));
                    }

                    // Verify the sender is either the applicant or the owner of the property
                    if (relatedApplication.ApplicantUserId != userId && relatedApplication.OwnerId != userId)
                    {
                        _logger.LogWarning("Unauthorized access attempt: User {UserId} tried to send a message related to application {ApplicationId} owned by {OwnerId} with applicant {ApplicantId}", 
                            userId, relatedApplication.Id, relatedApplication.OwnerId, relatedApplication.ApplicantUserId);
                        throw new UnauthorizedAccessException($"User does not have permission to send messages for this application");
                    }
                }

                await _unitOfWork.BeginTransactionAsync();

                // Create the message entity
                var message = new Message
                {
                    SenderUserId = userId,
                    RecipientUserId = recipientUserId,
                    Content = content,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                // Messages must be associated with a rental application
                if (relatedApplication == null)
                {
                    throw new ArgumentException("Messages must be associated with a rental application", nameof(relatedEntityId));
                }
                
                message.RentalApplicationId = relatedApplication.Id;
                message.RentalApplication = relatedApplication;
                
                // Add the message to the application's messages collection
                if (relatedApplication.Messages == null)
                {
                    relatedApplication.Messages = new List<Message>();
                }
                relatedApplication.Messages.Add(message);
                relatedApplication.ModifiedAt = DateTime.UtcNow;
                relatedApplication.ModifiedBy = userId;
                
                await _unitOfWork.RentalApplications.UpdateAsync(relatedApplication);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return message.Id;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error sending message from user with ID {UserId} to user with ID {RecipientUserId}", userId, recipientUserId);
                throw;
            }
        }
    }
}
