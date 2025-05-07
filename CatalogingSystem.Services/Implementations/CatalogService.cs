namespace CatalogingSystem.Services.Implementations;

using AutoMapper;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CatalogService : ICatalogService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CatalogService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatalogItemDto>> GetCatalogItems()
    {
        var archivos = await _context.ArchivosAdministrativos.ToListAsync();

        var catalogItems = new List<CatalogItemDto>();
        foreach (var archivo in archivos)
        {
            var identification = await _context.Identifications
                .FirstOrDefaultAsync(i => i.expediente == archivo.expediente);

            var graphicDocumentation = await _context.GraphicDocumentations
                .FirstOrDefaultAsync(g => g.expediente == archivo.expediente);

            catalogItems.Add(new CatalogItemDto
            {
                Expediente = archivo.expediente,
                ArchivoAdministrativo = _mapper.Map<ArchivoAdministrativoDto>(archivo),
                Identification = identification != null ? _mapper.Map<IdentificationDto>(identification) : null,
                GraphicDocumentation = graphicDocumentation != null ? _mapper.Map<GraphicDocumentationDto>(graphicDocumentation) : null
            });
        }

        return catalogItems;
    }

    public async Task<CatalogItemDto?> GetCatalogItem(long expediente)
    {
        var archivo = await _context.ArchivosAdministrativos
            .FirstOrDefaultAsync(a => a.expediente == expediente);

        if (archivo == null) return null;

        var identification = await _context.Identifications
            .FirstOrDefaultAsync(i => i.expediente == expediente);

        var graphicDocumentation = await _context.GraphicDocumentations
            .FirstOrDefaultAsync(g => g.expediente == expediente);

        return new CatalogItemDto
        {
            Expediente = archivo.expediente,
            ArchivoAdministrativo = _mapper.Map<ArchivoAdministrativoDto>(archivo),
            Identification = identification != null ? _mapper.Map<IdentificationDto>(identification) : null,
            GraphicDocumentation = graphicDocumentation != null ? _mapper.Map<GraphicDocumentationDto>(graphicDocumentation) : null
        };
    }

    public async Task<IEnumerable<CatalogItemDto>> SearchCatalogItems(
        string? materialName,
        string? authorName,
        string? titleName,
        string? genericClassification)
    {
        var query = _context.ArchivosAdministrativos.AsQueryable();

        // Filtrar por criterios de búsqueda usando joins implícitos
        if (!string.IsNullOrEmpty(materialName) || !string.IsNullOrEmpty(authorName) ||
            !string.IsNullOrEmpty(titleName) || !string.IsNullOrEmpty(genericClassification))
        {
            query = query.Where(a => _context.Identifications.Any(i =>
                i.expediente == a.expediente &&
                (string.IsNullOrEmpty(materialName) || i.material.MaterialName.Contains(materialName)) &&
                (string.IsNullOrEmpty(authorName) || i.author.Name.Contains(authorName)) &&
                (string.IsNullOrEmpty(titleName) || i.title.Name.Contains(titleName)) &&
                (string.IsNullOrEmpty(genericClassification) || i.genericClassification.Contains(genericClassification))));
        }

        var archivos = await query.ToListAsync();

        var catalogItems = new List<CatalogItemDto>();
        foreach (var archivo in archivos)
        {
            var identification = await _context.Identifications
                .FirstOrDefaultAsync(i => i.expediente == archivo.expediente);

            var graphicDocumentation = await _context.GraphicDocumentations
                .FirstOrDefaultAsync(g => g.expediente == archivo.expediente);

            catalogItems.Add(new CatalogItemDto
            {
                Expediente = archivo.expediente,
                ArchivoAdministrativo = _mapper.Map<ArchivoAdministrativoDto>(archivo),
                Identification = identification != null ? _mapper.Map<IdentificationDto>(identification) : null,
                GraphicDocumentation = graphicDocumentation != null ? _mapper.Map<GraphicDocumentationDto>(graphicDocumentation) : null
            });
        }

        return catalogItems;
    }
}