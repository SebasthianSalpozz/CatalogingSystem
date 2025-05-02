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
            .Include(i => i.ArchivoAdministrativo) // Incluir la relación
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<IdentificationDto>>(identifications);
        foreach (var dto in dtos)
        {
            var archivo = await _context.ArchivosAdministrativos
                .FirstOrDefaultAsync(a => a.expediente == dto.Expediente);
            if (archivo != null)
            {
                dto.Unit = archivo.institucion;
            }
        }

        return dtos;
    }

    public async Task<IdentificationDto?> GetIdentification(long expediente)
    {
        var identification = await _context.Identifications
            .Include(i => i.ArchivoAdministrativo) // Incluir la relación
            .FirstOrDefaultAsync(i => i.expediente == expediente);

        if (identification == null) return null;

        var dto = _mapper.Map<IdentificationDto>(identification);
        var archivo = await _context.ArchivosAdministrativos
            .FirstOrDefaultAsync(a => a.expediente == dto.Expediente);
        if (archivo != null)
        {
            dto.Unit = archivo.institucion;
        }

        return dto;
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

    public async Task<bool> UpdateIdentification(long expediente, IdentificationDto dto)
    {
        var identification = await _context.Identifications.FirstOrDefaultAsync(i => i.expediente == expediente);
        if (identification == null) return false;

        var originalExpediente = identification.expediente;
        _mapper.Map(dto, identification);
        identification.expediente = originalExpediente;
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