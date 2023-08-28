using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Runtime.InteropServices;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existWalk == null)
            {
                return null;
            }

            nZWalksDbContext.Walks.Remove(existWalk);
            await nZWalksDbContext.SaveChangesAsync();

            return existWalk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
           return await nZWalksDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await nZWalksDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walkDomainModel)
        {
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
            {
                return null;
            }

            walk.Name = walkDomainModel.Name;
            walk.Description = walkDomainModel.Description;
            walk.DifficultyId = walkDomainModel.DifficultyId;
            walk.RegionId = walkDomainModel.RegionId;
            walk.WalkImageUrl = walkDomainModel.WalkImageUrl;
            walk.LengthInKm = walkDomainModel.LengthInKm;

            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }
    }
}
