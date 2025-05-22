using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Enums;
using CatalogingSystem.Core.Interfaces;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogingSystem.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentTenantService _tenantService;

    public UserService(ApplicationDbContext context, IMapper mapper, ICurrentTenantService tenantService)
    {
        _context = context;
        _mapper = mapper;
        _tenantService = tenantService;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createDto)
    {
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("No se ha especificado un tenant");
        }

        if (await _context.Users.AnyAsync(u => u.Username == createDto.Username && u.TenantId == _tenantService.TenantId))
        {
            throw new InvalidOperationException("El nombre de usuario ya existe en este tenant");
        }

        var user = new User
        {
            Username = createDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createDto.Password),
            Role = Role.Investigador,
            AccessLevel = createDto.AccessLevel,
            IsActive = true,
            TenantId = _tenantService.TenantId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("No se ha especificado un tenant");
        }

        var users = await _context.Users
            .Where(u => u.TenantId == _tenantService.TenantId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserAsync(Guid id)
    {
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("No se ha especificado un tenant");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == _tenantService.TenantId);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto updateDto)
    {
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("No se ha especificado un tenant");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == _tenantService.TenantId);
        if (user == null || user.Role == Role.Director)
        {
            return false;
        }

        if (updateDto.AccessLevel.HasValue)
        {
            user.AccessLevel = updateDto.AccessLevel.Value;
        }

        if (updateDto.IsActive.HasValue)
        {
            user.IsActive = updateDto.IsActive.Value;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("No se ha especificado un tenant");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == _tenantService.TenantId);
        if (user == null || user.Role == Role.Director)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}