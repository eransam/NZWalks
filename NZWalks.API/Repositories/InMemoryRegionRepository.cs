using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{

    //קלאס זה הוא רק לדוג' של איך אפשר בקלות לשנות רפוזטורי מהקובץ הראשי
    //class implements an interface in C#, it must provide implementations for all the
    //members (methods, properties, events, indexers) defined in that interface.
    public class InMemoryRegionRepository : IRegionRepository
    {
        public Task<Region> CreateAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "sam",
                    Name = "Name",
                }
            };
        }

        public Task<List<Region?>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> UpdateAsync(Guid id, Region region)
        {
            throw new NotImplementedException();
        }
    }
}