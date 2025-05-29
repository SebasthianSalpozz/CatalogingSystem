using Microsoft.AspNetCore.Identity;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Interfaces;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.Services.Interfaces;
using CatalogingSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CatalogingSystem.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly ICurrentTenantService _tenantService;
    private readonly BaseDbContext _baseDbContext;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICurrentTenantService tenantService, BaseDbContext baseDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tenantService = tenantService;
        _baseDbContext = baseDbContext;
    }

    public async Task<User> CreateUserAsync(CreateUserRequestDto request)
    {
        // Validar que el rol sea permitido
        if (!Enum.TryParse<UserRole>(request.Role, true, out _))
        {
            throw new InvalidOperationException($"Rol inválido: {request.Role}. Solo se permiten 'Director' e 'Investigador'.");
        }

        var user = new User
        {
            UserName = request.Username,
            TenantId = request.TenantId
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Error al crear usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // Crear el rol si no existe
        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(request.Role));
        }

        // Asignar el rol al usuario
        await _userManager.AddToRoleAsync(user, request.Role);

        return user;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("No tenant context available.");
        }

        return await _userManager.Users
            .Where(u => u.TenantId == _tenantService.TenantId)
            .ToListAsync();
    }
}