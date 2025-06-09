namespace CatalogingSystem.Services.Implementations;

using AutoMapper;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CatalogService : ICatalogService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private const int MaxPageSize = 100;

    public CatalogService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<CatalogItemDto>> GetCatalogItems(int page = 1, int size = 10)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 10;
        if (size > MaxPageSize) size = MaxPageSize;

        var query = from archivo in _context.ArchivosAdministrativos.AsNoTracking()
                    join identification in _context.Identifications.AsNoTracking()
                        on archivo.expediente equals identification.expediente into identGroup
                    from identification in identGroup.DefaultIfEmpty()
                    join graphicDoc in _context.GraphicDocumentations.AsNoTracking()
                        on archivo.expediente equals graphicDoc.expediente into graphicGroup
                    from graphicDoc in graphicGroup.DefaultIfEmpty()
                    select new
                    {
                        Archivo = archivo,
                        Identification = identification,
                        GraphicDocumentation = graphicDoc
                    };

        int totalItems = await query.CountAsync();

        var results = await query
            .OrderBy(x => x.Archivo.expediente)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var catalogItems = results.Select(x => new CatalogItemDto
        {
            Expediente = x.Archivo.expediente,
            ArchivoAdministrativo = _mapper.Map<ArchivoAdministrativoDto>(x.Archivo),
            Identification = x.Identification != null ? _mapper.Map<IdentificationDto>(x.Identification) : null,
            GraphicDocumentation = x.GraphicDocumentation != null ? _mapper.Map<GraphicDocumentationDto>(x.GraphicDocumentation) : null
        }).ToList();

        return new PagedResultDto<CatalogItemDto>
        {
            Items = catalogItems,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size),
            CurrentPage = page,
            PageSize = size
        };
    }

    public async Task<CatalogItemDto?> GetCatalogItem(long expediente)
    {
        var result = await (from archivo in _context.ArchivosAdministrativos.AsNoTracking()
                           join identification in _context.Identifications.AsNoTracking()
                               on archivo.expediente equals identification.expediente into identGroup
                           from identification in identGroup.DefaultIfEmpty()
                           join graphicDoc in _context.GraphicDocumentations.AsNoTracking()
                               on archivo.expediente equals graphicDoc.expediente into graphicGroup
                           from graphicDoc in graphicGroup.DefaultIfEmpty()
                           where archivo.expediente == expediente
                           select new
                           {
                               Archivo = archivo,
                               Identification = identification,
                               GraphicDocumentation = graphicDoc
                           }).FirstOrDefaultAsync();

        if (result == null || result.Archivo == null) return null;

        return new CatalogItemDto
        {
            Expediente = result.Archivo.expediente,
            ArchivoAdministrativo = _mapper.Map<ArchivoAdministrativoDto>(result.Archivo),
            Identification = result.Identification != null ? _mapper.Map<IdentificationDto>(result.Identification) : null,
            GraphicDocumentation = result.GraphicDocumentation != null ? _mapper.Map<GraphicDocumentationDto>(result.GraphicDocumentation) : null
        };
    }

    public async Task<PagedResultDto<CatalogItemDto>> SearchCatalogItems(
        string? materialName,
        string? authorName,
        string? titleName,
        string? genericClassification,
        int page = 1,
        int size = 10)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 10;
        if (size > MaxPageSize) size = MaxPageSize;

        var query = from archivo in _context.ArchivosAdministrativos.AsNoTracking()
                    join identification in _context.Identifications.AsNoTracking()
                        on archivo.expediente equals identification.expediente into identGroup
                    from identification in identGroup.DefaultIfEmpty()
                    join graphicDoc in _context.GraphicDocumentations.AsNoTracking()
                        on archivo.expediente equals graphicDoc.expediente into graphicGroup
                    from graphicDoc in graphicGroup.DefaultIfEmpty()
                    select new
                    {
                        Archivo = archivo,
                        Identification = identification,
                        GraphicDocumentation = graphicDoc
                    };

        if (!string.IsNullOrEmpty(materialName) || !string.IsNullOrEmpty(authorName) ||
            !string.IsNullOrEmpty(titleName) || !string.IsNullOrEmpty(genericClassification))
        {
            query = query.Where(x => x.Identification != null &&
                (string.IsNullOrEmpty(materialName) || EF.Functions.ILike(x.Identification.material.MaterialName, $"%{materialName}%")) &&
                (string.IsNullOrEmpty(authorName) || EF.Functions.ILike(x.Identification.author.Name, $"%{authorName}%")) &&
                (string.IsNullOrEmpty(titleName) || EF.Functions.ILike(x.Identification.title.Name, $"%{titleName}%")) &&
                (string.IsNullOrEmpty(genericClassification) || EF.Functions.ILike(x.Identification.genericClassification, $"%{genericClassification}%")));
        }

        int totalItems = await query.CountAsync();

        var results = await query
            .OrderBy(x => x.Archivo.expediente)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var catalogItems = results.Select(x => new CatalogItemDto
        {
            Expediente = x.Archivo.expediente,
            ArchivoAdministrativo = _mapper.Map<ArchivoAdministrativoDto>(x.Archivo),
            Identification = x.Identification != null ? _mapper.Map<IdentificationDto>(x.Identification) : null,
            GraphicDocumentation = x.GraphicDocumentation != null ? _mapper.Map<GraphicDocumentationDto>(x.GraphicDocumentation) : null
        }).ToList();

        return new PagedResultDto<CatalogItemDto>
        {
            Items = catalogItems,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size),
            CurrentPage = page,
            PageSize = size
        };
    }
    public async Task<bool> DeleteCatalogItem(long expediente)
    {
        var archivo = await _context.ArchivosAdministrativos
            .FirstOrDefaultAsync(a => a.expediente == expediente);
        if (archivo == null) return false;

        var graphicDoc = await _context.GraphicDocumentations
            .FirstOrDefaultAsync(g => g.expediente == expediente);
        if (graphicDoc != null)
        {
            _context.GraphicDocumentations.Remove(graphicDoc);
        }

        var identification = await _context.Identifications
            .FirstOrDefaultAsync(i => i.expediente == expediente);
        if (identification != null)
        {
            _context.Identifications.Remove(identification);
        }

        _context.ArchivosAdministrativos.Remove(archivo);

        await _context.SaveChangesAsync();
        return true;
    }
}