using ApplicationAPI.Interfaces;
using AutoMapper;

namespace ApplicationAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public IUserRepository UserRepository => new UserRepository(_db, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_db, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_db);

        public UnitOfWork(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        
        public async Task<bool> Complete()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _db.ChangeTracker.HasChanges();
        }
    }
}