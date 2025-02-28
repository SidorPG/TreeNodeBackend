using Api.Exceptions;
using Api.Models;
using Api.Models.UserJournal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Controllers
{
    /// <summary>
    /// Represents journal API
    /// </summary>
    [ApiController]
    [Route("api.user.journal")]
    public class UserJournalController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserJournalController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <remarks>
        /// Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.
        /// </remarks>
        [Route("/api.user.journal.getRange")]
        [HttpPost]
        public PagedList<MJournal> GetRange([FromQuery] UserJournalQueryStringParameters qargs, [FromBody] UserJournalBodyParameters bargs)
        {
            if (qargs.skip < 0)
                throw new SecureException(UserJournalErrors.SkipOnlyPositive);
            if (qargs.take < 0)
                throw new SecureException(UserJournalErrors.TakeOnlyPositive);

            var dtos = _dbContext.JournalMessages.Include(x => x.JournalEvent).AsNoTracking().AsQueryable();

            IQueryable<MJournal> source = dtos
                .Where(x =>
                    (x.JournalEvent.Created > bargs.filter.from || x.JournalEvent.Created <= bargs.filter.to) &&
                    x.Data.ToLower().Contains(bargs.filter.SearchText.ToLower())
                )
                .Select(x => new MJournal()
                {
                    Id = x.Id,
                    CreatedAt = x.JournalEvent.Created,
                    EventId = x.EventId,
                })
                .OrderByDescending(x => x.CreatedAt);

            PagedList<MJournal> docs = PagedList<MJournal>.ToPagedList(source, qargs.skip, qargs.take);

            return docs;
        }

        /// <remarks>
        /// Returns the information about an particular event by ID.
        /// </remarks>
        [HttpPost("/api.user.journal.getSingle")]
        public async Task<MJournalInfo> getSingle([FromQuery] UserJournalGetSingleQueryStringParameters qargs)
        {
            var doc = await _dbContext.JournalMessages.Include(x => x.JournalEvent).FirstOrDefaultAsync(x => x.Id == qargs.id);
            if (doc == null) throw new SecureException(UserJournalErrors.MessageNotFound);
            return new MJournalInfo()
            {
                Id = doc.Id,
                CreatedAt = doc.JournalEvent.Created,
                EventId = doc.EventId,
                Text = doc.Data,
            };
        }
    }
}
