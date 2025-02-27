using Api.Models;
using Api.Models.TreeNode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Controllers
{
    /// <summary>
    /// Represents tree node API
    /// </summary>
    [ApiController]
    [Route("api.user.tree.node")]
    public class UserTreeNodeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;

        public UserTreeNodeController(ILoggerProvider loggerProvider, ApplicationDbContext dbContext)
        {
            _logger = loggerProvider.CreateLogger("JournalMessageController") ?? throw new ArgumentNullException(nameof(loggerProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <remarks>
        /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
        /// </remarks>
        [Route("/api.user.tree.node.create")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery] TreeNodeCreateQueryStringParameters args)
        {
            var tree = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == args.parentNodeId && x.TreeName == args.treeName);
            if (tree == null)
                throw new SecureException("Parent node dosn't exist", queryParameters: args.ToString());
            var node = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.ParentNodeId == args.parentNodeId && x.TreeName == args.treeName && x.Name == args.nodeName);
            if (node != null)
                throw new SecureException("Node alredy exist", queryParameters: args.ToString());

            node = new tree_node()
            {
                Name = args.nodeName,
                TreeName = args.treeName,
                ParentNodeId = args.parentNodeId,
            };
            await _dbContext.TreeNodes.AddAsync(node);
            await _dbContext.SaveChangesAsync();

            return Ok(new { id = node.Id });
        }
        /// <remarks>
        /// Delete an existing node in your tree. You must specify a node ID that belongs your tree.
        /// </remarks>
        [Route("/api.user.tree.node.delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync([FromQuery] TreeNodeDeleteQueryStringParameters args)
        {
            var anyNodeChild = _dbContext.TreeNodes.AsNoTracking().Any(x => x.ParentNodeId == args.nodeId && x.TreeName == args.treeName);
            if (anyNodeChild)
                throw new SecureException("You have to delete all children nodes first", queryParameters: args.ToString());

            var node = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == args.nodeId && x.TreeName == args.treeName);
            if (node != null)
                throw new SecureException("Tree dosn't exist", queryParameters: args.ToString());

            _dbContext.TreeNodes.Remove(node);
            await _dbContext.SaveChangesAsync();

            return Ok(new { id = node.Id });
        }
        /// <remarks>
        /// Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.
        /// </remarks>
        [Route("/api.user.tree.node.rename")]
        [HttpPost]
        public async Task<IActionResult> RenameAsync([FromQuery] TreeNodeRenameQueryStringParameters args)
        {
            var node = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == args.nodeId && x.Name == args.treeName);
            if (node != null)
                throw new SecureException("Tree dosn't exist", args.ToString());
            node.Name = args.newNodeName;
            _dbContext.TreeNodes.Update(node);
            await _dbContext.SaveChangesAsync();

            return Ok(new { id = node.Id });
        }
    }
}
