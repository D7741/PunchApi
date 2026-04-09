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

        public async Task<PunchRecord?> GetPunchRecordByIdAsync(long punchRecordId)
        {
            return await _context.PunchRecords.FirstOrDefaultAsync(p => p.PunchRecordId == punchRecordId);
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

        public async Task<PunchRecord> CreatePunchRecordAsync(PunchRecord punchRecord)
        {
            await _context.PunchRecords.AddAsync(punchRecord);
            await _context.SaveChangesAsync();
            return punchRecord;
        }

        public async Task<PunchRecord?> UpdatePunchRecordAsync(PunchRecord punchRecord)
        {
            var existingRecord = await _context.PunchRecords.FindAsync(punchRecord.PunchRecordId);

            if (existingRecord == null)
            {
                return null;
            }

            existingRecord.UserId = punchRecord.UserId;
            existingRecord.PunchInTime = punchRecord.PunchInTime;
            existingRecord.PunchOutTime = punchRecord.PunchOutTime;

            await _context.SaveChangesAsync();
            return existingRecord;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}