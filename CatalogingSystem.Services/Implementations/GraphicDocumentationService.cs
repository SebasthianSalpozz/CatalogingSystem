namespace CatalogingSystem.Services.Implementations;

using AutoMapper;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using Microsoft.EntityFrameworkCore;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.Services.Interfaces;

public class GraphicDocumentationService : IGraphicDocumentationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GraphicDocumentationService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GraphicDocumentationDto>> GetGraphicDocumentations()
    {
        var graphicDocs = await _context.GraphicDocumentations
            .Include(g => g.ArchivoAdministrativo)
            .ToListAsync();
        return _mapper.Map<IEnumerable<GraphicDocumentationDto>>(graphicDocs);
    }

    public async Task<GraphicDocumentationDto?> GetGraphicDocumentation(long expediente)
    {
        var graphicDoc = await _context.GraphicDocumentations
            .Include(g => g.ArchivoAdministrativo)
            .FirstOrDefaultAsync(g => g.expediente == expediente);
        return graphicDoc == null ? null : _mapper.Map<GraphicDocumentationDto>(graphicDoc);
    }

    public async Task<GraphicDocumentation> CreateGraphicDocumentation(GraphicDocumentationDto dto)
    {
        // Validar que el expediente exista en ArchivosAdministrativos
        var archivo = await _context.ArchivosAdministrativos
            .FirstOrDefaultAsync(a => a.expediente == dto.Expediente);
        if (archivo == null)
        {
            throw new InvalidOperationException($"No existe un archivo administrativo con el número de expediente {dto.Expediente}");
        }

        // Obtener el inventory desde Identification basado en el expediente
        var identification = await _context.Identifications
            .FirstOrDefaultAsync(i => i.expediente == dto.Expediente);
        if (identification == null)
        {
            throw new InvalidOperationException($"No existe una identificación asociada al expediente {dto.Expediente}");
        }

        // Validar unicidad del expediente
        bool exists = await _context.GraphicDocumentations
            .AnyAsync(g => g.expediente == dto.Expediente);
        if (exists)
        {
            throw new InvalidOperationException($"Ya existe una documentación gráfica para el expediente {dto.Expediente}");
        }

        // Validar que, si se proporcionan imágenes, haya al menos una
        if (dto.ImageUrls != null && !dto.ImageUrls.Any())
        {
            throw new InvalidOperationException("Si se proporcionan imágenes, debe haber al menos una URL válida.");
        }

        var graphicDoc = _mapper.Map<GraphicDocumentation>(dto);
        graphicDoc.Id = Guid.NewGuid();
        graphicDoc.inventory = identification.inventory; // Asignamos el inventory desde Identification

        _context.GraphicDocumentations.Add(graphicDoc);
        await _context.SaveChangesAsync();

        return graphicDoc;
    }

    public async Task<bool> UpdateGraphicDocumentation(long expediente, GraphicDocumentationDto dto)
    {
        var graphicDoc = await _context.GraphicDocumentations
            .FirstOrDefaultAsync(g => g.expediente == expediente);
        if (graphicDoc == null) return false;

        // Validar que el expediente exista en ArchivosAdministrativos
        var archivo = await _context.ArchivosAdministrativos
            .FirstOrDefaultAsync(a => a.expediente == dto.Expediente);
        if (archivo == null)
        {
            throw new InvalidOperationException($"No existe un archivo administrativo con el número de expediente {dto.Expediente}");
        }

        // Obtener el inventory desde Identification basado en el expediente
        var identification = await _context.Identifications
            .FirstOrDefaultAsync(i => i.expediente == dto.Expediente);
        if (identification == null)
        {
            throw new InvalidOperationException($"No existe una identificación asociada al expediente {dto.Expediente}");
        }

        // Validar que, si se proporcionan imágenes, haya al menos una
        if (dto.ImageUrls != null && !dto.ImageUrls.Any())
        {
            throw new InvalidOperationException("Si se proporcionan imágenes, debe haber al menos una URL válida.");
        }

        _mapper.Map(dto, graphicDoc);
        graphicDoc.inventory = identification.inventory; // Actualizamos el inventory desde Identification

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteGraphicDocumentation(long expediente)
    {
        var graphicDoc = await _context.GraphicDocumentations
            .FirstOrDefaultAsync(g => g.expediente == expediente);
        if (graphicDoc == null) return false;

        _context.GraphicDocumentations.Remove(graphicDoc);
        await _context.SaveChangesAsync();
        return true;
    }
}