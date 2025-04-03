namespace CatalogingSystem.Services.Implementations;

using AutoMapper;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CatalogingSystem.Data.DbContext;

public class ArchivoAdministrativoService : IArchivoAdministrativoService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ArchivoAdministrativoService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArchivoAdministrativoDto>> GetArchivosAdministrativos()
    {
        var archivos = await _context.ArchivosAdministrativos.ToListAsync();
        return _mapper.Map<IEnumerable<ArchivoAdministrativoDto>>(archivos);
    }

    public async Task<ArchivoAdministrativoDto?> GetArchivoAdministrativo(long expediente)
    {
        var archivo = await _context.ArchivosAdministrativos.FirstOrDefaultAsync(a => a.expediente == expediente);
        return archivo == null ? null : _mapper.Map<ArchivoAdministrativoDto>(archivo);
    }

    public async Task<ArchivoAdministrativo> CreateArchivoAdministrativo(ArchivoAdministrativoDto dto)
    {
        bool existeExpediente = await _context.ArchivosAdministrativos.AnyAsync(a => a.expediente == dto.Expediente);
        if (existeExpediente)
        {
            throw new InvalidOperationException($"Ya existe un archivo con el número de expediente {dto.Expediente}");
        }

        var archivo = _mapper.Map<ArchivoAdministrativo>(dto);
        archivo.Id = Guid.NewGuid();

        _context.ArchivosAdministrativos.Add(archivo);
        await _context.SaveChangesAsync();

        return archivo;
    }

    public async Task<bool> UpdateArchivoAdministrativo(long expediente, ArchivoAdministrativoDto dto)
    {
        var archivo = await _context.ArchivosAdministrativos.FirstOrDefaultAsync(a => a.expediente == expediente);
        if (archivo == null) return false;

        var expedienteOriginal = archivo.expediente;
        _mapper.Map(dto, archivo);
        archivo.expediente = expedienteOriginal;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteArchivoAdministrativo(long expediente)
    {
        var archivo = await _context.ArchivosAdministrativos.FirstOrDefaultAsync(a => a.expediente == expediente);
        if (archivo == null) return false;

        _context.ArchivosAdministrativos.Remove(archivo);
        await _context.SaveChangesAsync();
        return true;
    }
}