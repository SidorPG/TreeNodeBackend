using Api.Exceptions;
using Api.Models.UserTreeNode;
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
        private readonly ApplicationDbContext _dbContext;

        public UserTreeNodeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <remarks>
        /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
        /// </remarks>
        [Route("/api.user.tree.node.create")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery] UserTreeNodeCreateQueryStringParameters args)
        {
            var tree = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == args.parentNodeId && x.TreeName == args.treeName);
            if (tree == null)
                throw new SecureException(UserTreeNodeErrors.ParentNotFound);
            var node = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.ParentNodeId == args.parentNodeId && x.TreeName == args.treeName && x.Name == args.nodeName);
            if (node != null)
                throw new SecureException(UserTreeNodeErrors.NodeAlredyExist);

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
        public async Task<IActionResult> DeleteAsync([FromQuery] UserTreeNodeDeleteQueryStringParameters args)
        {
            var rootNode = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.ParentNodeId == null && x.TreeName == args.treeName);
            if (rootNode == null)
                throw new SecureException(UserTreeNodeErrors.TreeNotFound);
            var anyNodeChild = _dbContext.TreeNodes.AsNoTracking().Any(x => x.ParentNodeId == args.nodeId && x.TreeName == args.treeName);
            if (anyNodeChild)
                throw new SecureException(UserTreeNodeErrors.ChildNodesShouldBeDeletedFirst);

            var node = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == args.nodeId && x.TreeName == args.treeName);
            if (node != null)
                throw new SecureException(UserTreeNodeErrors.TreeNodeNotFound);

            _dbContext.TreeNodes.Remove(node);
            await _dbContext.SaveChangesAsync();

            return Ok(new { id = node.Id });
        }
        /// <remarks>
        /// Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.
        /// </remarks>
        [Route("/api.user.tree.node.rename")]
        [HttpPost]
        public async Task<IActionResult> RenameAsync([FromQuery] UserTreeNodeRenameQueryStringParameters args)
        {
            var node = await _dbContext.TreeNodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == args.nodeId && x.TreeName == args.treeName);
            if (node != null)
                throw new SecureException(UserTreeNodeErrors.TreeNodeNotFound);
            node.Name = args.newNodeName;
            _dbContext.TreeNodes.Update(node);
            await _dbContext.SaveChangesAsync();

            return Ok(new { id = node.Id });
        }
    }
}
