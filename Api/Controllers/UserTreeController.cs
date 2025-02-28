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
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;

        public UserTreeController(ILoggerProvider loggerProvider, ApplicationDbContext dbContext)
        {
            _logger = loggerProvider.CreateLogger("JournalMessageController") ?? throw new ArgumentNullException(nameof(loggerProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <remarks>
        /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
        /// </remarks>
        [Route("/api.user.tree.get")]
        [HttpPost]
        public async Task<TreeNode> get([FromQuery] TreeQueryStringParameters args)
        {
            var treeNode = _dbContext.TreeNodes.AsNoTracking().FirstOrDefault(x => x.Name == args.treeName);

            if (treeNode == null)
            {
                treeNode = new tree_node() { Name = args.treeName, TreeName = args.treeName };
                await _dbContext.AddAsync(treeNode);
                await _dbContext.SaveChangesAsync();
                return new TreeNode { Name = args.treeName, Id = treeNode.Id, Children = new List<TreeNode>() };
            }

            var result = new TreeNode { Name = args.treeName, Id = treeNode.Id, Children = await getChilder(treeNode.Id) };
            _logger.Log(LogLevel.None, $"Returned {result} docs from database.");
            return result;

        }

        private async Task<List<TreeNode>> getChilder(Guid parantId)
        {
            var childNodes = _dbContext.TreeNodes.AsNoTracking().Where(x => x.ParentNodeId == parantId).ToList();
            if (childNodes == null) { return new List<TreeNode>(); }
            var result = new List<TreeNode>();
            childNodes.ForEach(async treeNode =>
              {
                  var resultIterm = new TreeNode { Name = treeNode.Name, Id = treeNode.Id, Children = await getChilder(treeNode.Id) };
                  result.Add(resultIterm);
              });
            return result;
        }
    }
}
