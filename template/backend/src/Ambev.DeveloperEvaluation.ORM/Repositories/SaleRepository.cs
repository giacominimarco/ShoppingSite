using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for Sale entity operations.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the SaleRepository class.
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the repository.
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its sale number.
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Retrieves all sales with optional filtering and pagination.
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sales</returns>
    public async Task<IEnumerable<Sale>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var skip = (page - 1) * size;
        return await _context.Sales
            .Include(s => s.Items)
            .OrderByDescending(s => s.CreatedAt)
            .Skip(skip)
            .Take(size)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves sales with filtering and pagination.
    /// </summary>
    /// <param name="filters">The filters to apply</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sales matching the filters</returns>
    public async Task<IEnumerable<Sale>> GetFilteredAsync(SaleFilters filters, int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filters.Customer))
        {
            query = query.Where(s => s.Customer.Contains(filters.Customer));
        }

        if (!string.IsNullOrWhiteSpace(filters.Branch))
        {
            query = query.Where(s => s.Branch.Contains(filters.Branch));
        }

        if (!string.IsNullOrWhiteSpace(filters.Status))
        {
            if (Enum.TryParse<SaleStatus>(filters.Status, out var status))
            {
                query = query.Where(s => s.Status == status);
            }
        }

        if (filters.MinDate.HasValue)
        {
            // Converter para início do dia (00:00:00) em UTC para incluir todas as vendas do dia
            var minDate = filters.MinDate.Value.Date;
            var utcMinDate = DateTime.SpecifyKind(minDate, DateTimeKind.Utc);
            query = query.Where(s => s.SaleDate >= utcMinDate);
        }

        if (filters.MaxDate.HasValue)
        {
            // Converter para fim do dia (23:59:59.999) em UTC para incluir todas as vendas do dia
            var maxDate = filters.MaxDate.Value.Date.AddDays(1).AddTicks(-1);
            var utcMaxDate = DateTime.SpecifyKind(maxDate, DateTimeKind.Utc);
            query = query.Where(s => s.SaleDate <= utcMaxDate);
        }

        if (filters.MinTotalAmount.HasValue)
        {
            query = query.Where(s => s.TotalAmount >= filters.MinTotalAmount.Value);
        }

        if (filters.MaxTotalAmount.HasValue)
        {
            query = query.Where(s => s.TotalAmount <= filters.MaxTotalAmount.Value);
        }

        var skip = (page - 1) * size;
        return await query
            .Include(s => s.Items)
            .OrderByDescending(s => s.CreatedAt)
            .Skip(skip)
            .Take(size)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the total count of sales matching the filters.
    /// </summary>
    /// <param name="filters">The filters to apply</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of sales matching the filters</returns>
    public async Task<int> GetFilteredCountAsync(SaleFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filters.Customer))
        {
            query = query.Where(s => s.Customer.Contains(filters.Customer));
        }

        if (!string.IsNullOrWhiteSpace(filters.Branch))
        {
            query = query.Where(s => s.Branch.Contains(filters.Branch));
        }

        if (!string.IsNullOrWhiteSpace(filters.Status))
        {
            if (Enum.TryParse<SaleStatus>(filters.Status, out var status))
            {
                query = query.Where(s => s.Status == status);
            }
        }

        if (filters.MinDate.HasValue)
        {
            // Converter para início do dia (00:00:00) em UTC para incluir todas as vendas do dia
            var minDate = filters.MinDate.Value.Date;
            var utcMinDate = DateTime.SpecifyKind(minDate, DateTimeKind.Utc);
            query = query.Where(s => s.SaleDate >= utcMinDate);
        }

        if (filters.MaxDate.HasValue)
        {
            // Converter para fim do dia (23:59:59.999) em UTC para incluir todas as vendas do dia
            var maxDate = filters.MaxDate.Value.Date.AddDays(1).AddTicks(-1);
            var utcMaxDate = DateTime.SpecifyKind(maxDate, DateTimeKind.Utc);
            query = query.Where(s => s.SaleDate <= utcMaxDate);
        }

        if (filters.MinTotalAmount.HasValue)
        {
            query = query.Where(s => s.TotalAmount >= filters.MinTotalAmount.Value);
        }

        if (filters.MaxTotalAmount.HasValue)
        {
            query = query.Where(s => s.TotalAmount <= filters.MaxTotalAmount.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale in the repository.
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Deletes a sale from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Gets the total count of sales.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of sales</returns>
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sales.CountAsync(cancellationToken);
    }
}
