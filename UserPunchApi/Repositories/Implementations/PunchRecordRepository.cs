using Microsoft.EntityFrameworkCore;
using UserPunchApi.Data;
using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;

namespace UserPunchApi.Repositories.Implementations
{
    public class PunchRecordRepository : IPunchRecordRepository
    {
        private readonly AppDbContext _context;

        public PunchRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PunchRecord>> GetAllPunchRecordAsync()
        {
            return await _context.PunchRecords.ToListAsync();
        }

        public async Task<PunchRecord?> GetPunchRecordByIdAsync(long id)
        {
            return await _context.PunchRecords.FirstOrDefaultAsync(p => p.PunchRecordId == id);
        }

        public async Task<IEnumerable<PunchRecord>> GetPunchRecordByUserIdAsync(long userId)
        {
            return await _context.PunchRecords
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<PunchRecord?> GetOpenPunchRecordByUserIdAsync(long userId)
        {
            return await _context.PunchRecords
                .Where(p => p.UserId == userId && p.PunchOutTime == null)
                .OrderByDescending(p => p.PunchInTime)
                .FirstOrDefaultAsync();
        }

        public async Task<PunchRecord> CreateAsync(PunchRecord punchRecord)
        {
            await _context.PunchRecords.AddAsync(punchRecord);
            await _context.SaveChangesAsync();
            return punchRecord;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}