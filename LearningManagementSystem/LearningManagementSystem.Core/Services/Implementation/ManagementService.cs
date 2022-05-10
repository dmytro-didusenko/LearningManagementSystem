using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class ManagementService : IManagementService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<ManagementService> _logger;

        public ManagementService(AppDbContext context, IUserService userService, IMapper mapper, ILogger<ManagementService> logger)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

    }
}
