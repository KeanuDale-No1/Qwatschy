
using Microsoft.EntityFrameworkCore;

namespace VoiceChat.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly VoiceChatDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(VoiceChatDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}