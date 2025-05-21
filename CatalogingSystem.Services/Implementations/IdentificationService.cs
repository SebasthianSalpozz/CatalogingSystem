namespace CatalogingSystem.Services.Implementations;

using AutoMapper;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CatalogingSystem.Data.DbContext;

public class IdentificationService : IIdentificationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public IdentificationService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<IdentificationDto>> GetIdentifications()
    {
        var identifications = await _context.Identifications
            .Include(i => i.ArchivoAdministrativo)
            .ToListAsync();

        return _mapper.Map<IEnumerable<IdentificationDto>>(identifications);
    }

    public async Task<IdentificationDto?> GetIdentification(long expediente)
    {
        var identification = await _context.Identifications
            .Include(i => i.ArchivoAdministrativo)
            .FirstOrDefaultAsync(i => i.expediente == expediente);

        return identification == null ? null : _mapper.Map<IdentificationDto>(identification);
    }

    public async Task<Identification> CreateIdentification(IdentificationDto dto)
    {
        // Validar que el expediente exista en ArchivosAdministrativos
        var archivo = await _context.ArchivosAdministrativos
            .FirstOrDefaultAsync(a => a.expediente == dto.Expediente);
        if (archivo == null)
        {
            throw new InvalidOperationException($"No existe un archivo administrativo con el número de expediente {dto.Expediente}");
        }

        // Validar que el expediente no exista ya en Identifications
        bool existsExpediente = await _context.Identifications.AnyAsync(i => i.expediente == dto.Expediente);
        if (existsExpediente)
        {
            throw new InvalidOperationException($"Ya existe una identificación con el número de expediente {dto.Expediente}");
        }

        var identification = _mapper.Map<Identification>(dto);
        identification.Id = Guid.NewGuid();

        _context.Identifications.Add(identification);
        await _context.SaveChangesAsync();

        return identification;
    }

    public async Task<bool> UpdateIdentification(long expediente, UpdateIdentificationDto dto)
    {
        var identification = await _context.Identifications.FirstOrDefaultAsync(i => i.expediente == expediente);
        if (identification == null) return false;

        _mapper.Map(dto, identification);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteIdentification(long expediente)
    {
        var identification = await _context.Identifications.FirstOrDefaultAsync(i => i.expediente == expediente);
        if (identification == null) return false;

        _context.Identifications.Remove(identification);
        await _context.SaveChangesAsync();
        return true;
    }
}