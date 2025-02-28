using Api.Models.UserTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Controllers
{
    /// <summary>
    /// Represents entire tree API
    /// </summary>
    [ApiController]
    [Route("api.user.tree")]
    public class UserTreeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTreeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <remarks>
        /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
        /// </remarks>
        [Route("/api.user.tree.get")]
        [HttpPost]
        public async Task<UserTree> get([FromQuery] UserTreeQueryStringParameters args)
        {
            var treeNode = _dbContext.TreeNodes.AsNoTracking().FirstOrDefault(x => x.Name == args.treeName);

            if (treeNode == null)
            {
                treeNode = new tree_node() { Name = args.treeName, TreeName = args.treeName };
                await _dbContext.AddAsync(treeNode);
                await _dbContext.SaveChangesAsync();
                return new UserTree { Name = args.treeName, Id = treeNode.Id, Children = new List<UserTree>() };
            }

            var result = new UserTree { Name = args.treeName, Id = treeNode.Id, Children = await getChilder(treeNode.Id) };

            return result;
        }

        private async Task<List<UserTree>> getChilder(Guid parantId)
        {
            var childNodes = _dbContext.TreeNodes.AsNoTracking().Where(x => x.ParentNodeId == parantId).ToList();
            if (childNodes == null) { return new List<UserTree>(); }
            var result = new List<UserTree>();
            childNodes.ForEach(async treeNode =>
              {
                  var resultIterm = new UserTree { Name = treeNode.Name, Id = treeNode.Id, Children = await getChilder(treeNode.Id) };
                  result.Add(resultIterm);
              });
            return result;
        }
    }
}
